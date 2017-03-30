using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CB.CSharp.Extentions
{
    public static class ExcelWorksheetExtender
    {
        public static readonly Func<ExcelRangeBase, string> TextOnly = (x) => x.Text;

        public static IEnumerable<string> GetHeaders(this ExcelWorksheet ExcelWorksheet) =>
            ExcelWorksheet
            .Cells[ExcelWorksheet.Dimension.Start.Row, ExcelWorksheet.Dimension.Start.Column, 1, ExcelWorksheet.Dimension.End.Column]
            .Select(x => x.Text);

        public static IEnumerable<string> GetColumns(this ExcelWorksheet ExcelWorksheet) =>
            ExcelWorksheet
            .Cells[ExcelWorksheet.Dimension.Start.Row, ExcelWorksheet.Dimension.Start.Column, 1, ExcelWorksheet.Dimension.End.Column]
            .Select(x => x.Text);

        public static void AddDate(this ExcelPackage ExcelPackage, DataTable DataTable)
        {
            ExcelWorksheet worksheet = ExcelPackage.Workbook.Worksheets.Add(DataTable.TableName);

            if (DataTable.Columns.Count > 0)
                worksheet.Cells["A1"].LoadFromDataTable(DataTable, true);
        }
        public static void RemoveColumns(this ExcelWorksheet ExcelWorksheet, params string[] UnwantedColumns)
        {
            if (ExcelWorksheet == null)
                return;

            if (UnwantedColumns == null)
                return;

           var UnWantedColumnsIndexList = ExcelWorksheet.Cells[ExcelWorksheet.Dimension.Start.Row, ExcelWorksheet.Dimension.Start.Column, 1, ExcelWorksheet.Dimension.End.Column]
                           .Where(x => UnwantedColumns.Contains(x.Text))
                           .Select(x => x.Columns)
                           .ToList();

            foreach (int IndexToDelete in UnWantedColumnsIndexList)
                ExcelWorksheet.DeleteColumn(IndexToDelete);

        }
        public static void FormatExcelWorksheetDefault(this ExcelWorksheet ExcelWorksheet)
        {
            ExcelWorksheet.Cells[ExcelWorksheet.Dimension.Address].AutoFilter = true;
            ExcelWorksheet.Cells[ExcelWorksheet.Dimension.Address].AutoFitColumns();

            ExcelWorksheet.View.FreezePanes(2, 1);
        }
    }
}