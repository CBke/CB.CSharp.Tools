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
            using (SQLiteCommand selectCommand = new SQLiteCommand(SqlStatement, SQLiteConnection))
            {
                foreach (SQLiteParameter SQLiteParameter in SQLiteParameters)
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
                    DataTable DataTable = new DataTable();
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

            using (SQLiteCommand cmd = new SQLiteCommand(sql, SQLiteConnection))
            {
                if (parameters != null)
                    foreach (SQLiteParameter p in parameters)
                        cmd.Parameters.Add(p);

                cmd.ExecuteNonQuery();
            }

            using (SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT last_insert_rowid()", SQLiteConnection))
            {
                try
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 1)
                        ret = int.Parse(dt.Rows[0][0].ToString());
                }
                catch
                {
                    return -1;
                }
            }

            return ret;
        }

        /*
            public bool CreateIndex(Index Index)
            {
                string SqlStatement = string.Format("CREATE INDEX {0} ON {1} ({2}) ",
                    Index.Name,
                    Index.TabelName,
                    Index.Columns.ToJoinedString()
                   );

                return GetDt(SqlStatement).Rows.Count > 0;
            }
    */
    }
}
