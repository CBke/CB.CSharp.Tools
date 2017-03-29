using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CB.CSharp.Extentions
{
    public static class DataTableExtender
    {
        public static IEnumerable<string> GetColumnNames(this DataTable DataTable) =>
            DataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName);
    }
}