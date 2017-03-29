using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CB.CSharp.Extentions.Object
{
    public static class ObjectExtender
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> List, string TableName)
        {
            var PropertyInfoArray = typeof(T).GetProperties();

            var DataTable = new DataTable(TableName);

            DataTable.Columns.AddRange(
              PropertyInfoArray.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray()
            );

            List.ToList().ForEach(
                i => DataTable.Rows.Add(PropertyInfoArray.Select(p => p.GetValue(i, null)).ToArray())
                );

            return DataTable;
        }
    }
}