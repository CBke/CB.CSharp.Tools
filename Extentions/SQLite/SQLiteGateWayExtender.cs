using CB.CSharp.Tools.SQLite;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace CB.CSharp.Extentions
{
    public static class SQLiteGateWayExtender
    {
        public static bool TableExists(this SQLiteGateWay SQLiteGateWay, string TableName) =>
            SQLiteGateWay
            .GetDt("SELECT name FROM sqlite_master WHERE type='table' AND name=:TableName", new SQLiteParameter("TableName", TableName))
            .Rows
            .Count == 1;

        public static bool ViewExists(this SQLiteGateWay SQLiteGateWay, string TableName) =>
            SQLiteGateWay
            .GetDt("SELECT name FROM sqlite_master WHERE type='view' AND name=:TableName", new SQLiteParameter("TableName", TableName))
            .Rows
            .Count == 1;

        public static int AddColumn(this SQLiteGateWay SQLiteGateWay, string TableName, string ColumnName, string type = "varchar(1000)") =>
            SQLiteGateWay.Update($"alter table {TableName} add column {ColumnName} {type}");

        public static void AddColumns(this SQLiteGateWay SQLiteGateWay, string TableName, IEnumerable<string> ColumnNames, string type = "varchar(1000)")
        {
            foreach (var ColumnName in ColumnNames)
                SQLiteGateWay.AddColumn(TableName, ColumnName, type);
        }

        public static int BackupTable(this SQLiteGateWay SQLiteGateWay, string TableName, string prefix = "", string suffix_format = "yyyyMMddHHmmssffff") =>
            SQLiteGateWay.Update($"create table {DateTime.Now.ToString(suffix_format)}{TableName}{2} as select * from {TableName}");

        public static void CreateView(this SQLiteGateWay SQLiteGateWay, string ViewName, string sql, bool replace = false)
        {
            if (SQLiteGateWay.ViewExists(ViewName))
            {
                if (!replace)
                    return;
                SQLiteGateWay.DropView(ViewName);
            }

            SQLiteGateWay.Update($"create view {ViewName} as {sql}");
        }

        public static int DropTable(this SQLiteGateWay SQLiteGateWay, string TableName) =>
            SQLiteGateWay.Update($"drop table {TableName}");

        public static void DropView(this SQLiteGateWay SQLiteGateWay, string TableName) =>
            SQLiteGateWay.Update($"drop view {TableName}");

        private static IEnumerable<string> GetObjectOfType(this SQLiteGateWay SQLiteGateWay, string ObjectType, string regexp = "%") =>
            SQLiteGateWay
            .GetDt("SELECT name FROM sqlite_master WHERE type =:ObjectType AND name like :name order by name",
                new SQLiteParameter("name", "%" + regexp + "%"),
                new SQLiteParameter("ObjectType", ObjectType))
            .Rows
            .Cast<DataRow>()
            .Select(x => x[0].ToString());

        public static IEnumerable<string> GetTableNames(this SQLiteGateWay SQLiteGateWay, string regexp = "%") =>
            SQLiteGateWay.GetObjectOfType("table", regexp);

        public static IEnumerable<string> GetViews(this SQLiteGateWay SQLiteGateWay, string regexp = "%") =>
            SQLiteGateWay.GetObjectOfType("view", regexp);

        public static IEnumerable<string> GetTablesAndViews(this SQLiteGateWay SQLiteGateWay, string RegExpTables = "%", string RegExpViews = "%") =>
            SQLiteGateWay
            .GetObjectOfType("view", RegExpViews)
            .Concat(SQLiteGateWay.GetObjectOfType("table", RegExpTables));

        public static ExcelPackage WriteDataBaseToExcel(this SQLiteGateWay SQLiteGateWay, IEnumerable<string> TableNames)
        {
            var ExcelPackage = new ExcelPackage();

            foreach (var TableName in TableNames.ToList())
            {
                var worksheet = ExcelPackage.Workbook.Worksheets.Add(TableName);

                var DataTable = SQLiteGateWay.GetDt(string.Format("select * from '{0}'", TableName));

                if (DataTable.Columns.Count > 0)
                    worksheet.Cells["A1"].LoadFromDataTable(DataTable, true);
            }
            return ExcelPackage;
        }

        public static ExcelPackage WriteDataBaseToExcel(this SQLiteGateWay SQLiteGateWay, string RegExpTables = "%", string RegExpViews = "%") =>
            SQLiteGateWay.WriteDataBaseToExcel(SQLiteGateWay.GetTablesAndViews(RegExpTables, RegExpViews));

        public static int InsertExcelWorksheetIntoTable(this SQLiteGateWay SQLiteGateWay, ExcelWorksheet ws, string TableName, Func<ExcelRangeBase, string> Format, bool CreateColumns = false)
        {
            int RowsAdded = 0;

            var WantedColumns = ws.GetColumns();

            SQLiteGateWay.EnsureColumnsExists(WantedColumns, TableName, CreateColumns);

            var insertStatement = $"insert into {TableName} ({WantedColumns.ToJoinedString()}) values({WantedColumns.Select(x => ":" + x).ToJoinedString()})";

            var SQLiteParameters = WantedColumns
                .Select(x => new SQLiteParameter(x))
                .ToArray();

            for (var rowNum = 2; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                for (var colNum = 1; colNum <= ws.Dimension.End.Column; colNum++)
                {
                    string val = Format(ws.Cells[rowNum, colNum]);

                    if (string.IsNullOrEmpty(val))
                        SQLiteParameters[colNum - 1].Value = DBNull.Value;
                    else
                        SQLiteParameters[colNum - 1].Value = val;
                }
                try
                {
                    RowsAdded += SQLiteGateWay.Update(insertStatement, SQLiteParameters);
                }
                catch { }
            }
            return RowsAdded;
        }

        public static int InsertDataRowIntoTable(this SQLiteGateWay SQLiteGateWay, DataRow DataRow, bool CreateColumns = false)
        {
            var ColumnNames = DataRow.Table.GetColumnNames();

            SQLiteGateWay.EnsureColumnsExists(ColumnNames, DataRow.Table.TableName, CreateColumns);

            var Fields = ColumnNames
                .ToJoinedString();

            var ParameterNames = ColumnNames
                .Select(ColumnName => ":" + ColumnName)
                .ToJoinedString();

            var ColumnNamesParameter = ColumnNames
                .Select(ColumnName => new SQLiteParameter(":" + ColumnName, DataRow[ColumnName]))
                .ToArray();

            return SQLiteGateWay.Insert($"insert into {DataRow.Table.TableName} ({Fields}) values ({ParameterNames})", ColumnNamesParameter);
        }

        public static int InsertDataTableIntoTable(this SQLiteGateWay SQLiteGateWay, System.Data.DataTable DataTable, bool CreateColumns = false)
        {
            SQLiteGateWay.EnsureColumnsExists(DataTable.GetColumnNames(), DataTable.TableName, CreateColumns);

            return DataTable
                .Rows
                .Cast<DataRow>()
                .ToList()
                .Select(x => SQLiteGateWay.InsertDataRowIntoTable(x))
                .Sum();
        }

        public static void EnsureColumnsExists(this SQLiteGateWay SQLiteGateWay, IEnumerable<string> WantedColumns, string TableName, bool CreateMissingColumns = false)
        {
            var AvailableColumns = SQLiteGateWay.GetColumnNames(TableName);
            var MissingColumns = WantedColumns.Except(AvailableColumns).ToArray();

            if (!MissingColumns.Any())
                return;

            if (CreateMissingColumns)
                SQLiteGateWay.AddColumns(TableName, MissingColumns);
            else
                throw new NotAllColumnsFoundException(MissingColumns);
        }

        public static IEnumerable<string> GetColumnNames(this SQLiteGateWay SQLiteGateWay, string TableName) =>
            SQLiteGateWay
            .GetDt($"select * from '{TableName}' where 1=0")
            .GetColumnNames();

        public static void BeginTransaction(this SQLiteGateWay SQLiteGateWay) =>
            SQLiteGateWay.Update("BEGIN TRANSACTION");

        public static void EndTransaction(this SQLiteGateWay SQLiteGateWay) =>
            SQLiteGateWay.Update("COMMIT TRANSACTION");
    }
}