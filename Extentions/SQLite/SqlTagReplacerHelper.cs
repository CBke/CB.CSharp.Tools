using CB.CSharp.Tools.SQLite;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;

namespace CB.CSharp.Extentions
{
    public static class SqlTagReplacerHelper
    {
        public static string GenerateTag(this SqlTagReplacer SqlTagReplacer, string Value) =>
            $"{SqlTagReplacer.Tag}{Value}";

        public static void RewriteQuery(this SqlTagReplacer SqlTagReplacer, DbCommand cmd)
        {
            var Parameters = cmd.Parameters.Cast<DbParameter>()
                .Where(x => x.Value != DBNull.Value)
                .Where(x => x.Value is string)
                .Where(x => Regex.Match((string)x.Value, SqlTagReplacer.Tag).Success);

            foreach (var Parameter in Parameters)
            {
                Parameter.Size = 4096;
                Parameter.DbType = DbType.AnsiStringFixedLength;
                Parameter.Value = ((string)Parameter.Value).Replace(SqlTagReplacer.Tag, "");

                cmd.CommandText = Regex.Replace(cmd.CommandText,
                    $"= @{Parameter.ParameterName}",
                    string.Format(SqlTagReplacer.Replacement, Parameter.ParameterName));
            }
        }
    }
}