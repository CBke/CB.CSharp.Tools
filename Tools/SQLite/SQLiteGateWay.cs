using System.Data;
using System.Data.Entity;
using System.Data.SQLite;

namespace CB.CSharp.Tools.SQLite
{
    public class SQLiteGateWay
    {
        public string ConnectionString { get; private set; }
        public SQLiteConnection SQLiteConnection { get; private set; }

        public SQLiteGateWay(DbContext DbContext)
        {
            ConnectionString = DbContext.Database.Connection.ConnectionString;

            SQLiteConnection = new SQLiteConnection(ConnectionString);
            SQLiteConnection.Open();
        }

        public int Update(string SqlStatement, params SQLiteParameter[] SQLiteParameters)
        {
            using (var selectCommand = new SQLiteCommand(SqlStatement, SQLiteConnection))
            {
                foreach (var SQLiteParameter in SQLiteParameters)
                    selectCommand.Parameters.Add(SQLiteParameter);

                return selectCommand.ExecuteNonQuery();
            }
        }

        public DataTable GetDt(string SqlStatement, params SQLiteParameter[] SQLiteParameters)
        {
            using (var selectCommand = new SQLiteCommand(SqlStatement, SQLiteConnection))
            {
                foreach (var SQLiteParameter in SQLiteParameters)
                    selectCommand.Parameters.Add(SQLiteParameter);

                using (var SQLiteDataAdapter = new SQLiteDataAdapter(selectCommand))
                {
                    var DataTable = new DataTable();
                    SQLiteDataAdapter.Fill(DataTable);
                    return DataTable;
                }
            }
        }

        public void Dispose()
        {
            if (SQLiteConnection == null)
                return;

            SQLiteConnection.Close();
            SQLiteConnection.Dispose();
        }

        public int Insert(string sql, params SQLiteParameter[] parameters)
        {
            int ret = -1;

            using (var SQLiteCommand = new SQLiteCommand(sql, SQLiteConnection))
            {
                if (parameters != null)
                    foreach (var SQLiteParameter in parameters)
                        SQLiteCommand.Parameters.Add(SQLiteParameter);

                SQLiteCommand.ExecuteNonQuery();
            }

            using (var SQLiteDataAdapter = new SQLiteDataAdapter("SELECT last_insert_rowid()", SQLiteConnection))
            {
                try
                {
                    var DataTable = new DataTable();
                    SQLiteDataAdapter.Fill(DataTable);
                    if (DataTable.Rows.Count == 1)
                        ret = int.Parse(DataTable.Rows[0][0].ToString());
                }
                catch
                {
                    return -1;
                }
            }

            return ret;
        }
    }
}