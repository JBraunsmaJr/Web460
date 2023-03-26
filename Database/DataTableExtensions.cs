using System.Data;
using System.Linq;

namespace Database
{
    public static class DataTableExtensions
    {
        public static bool Insert<T>(this DataTable table, T item)
        {
            if (!BadgerDbContext._tables.TryGetValue(typeof(T), out var info))
                return false;

            var row = table.NewRow();
            row.BeginEdit();

            foreach (var col in info.Columns)
            {
                var value = col.Property.GetValue(item, null);

                if (value != null)
                    row[col.Name] = value;
            }
            
            row.EndEdit();
            table.Rows.Add(row);
            return true;
        }

        public static bool Update<T>(this DataTable table, T item)
        {
            if (!BadgerDbContext._tables.TryGetValue(typeof(T), out var info))
                return false;

            var primaryCol = info.Columns.FirstOrDefault(x => x.IsPrimary);
            var id = primaryCol.Property.GetValue(item, null);
            
            foreach (DataRow row in table.Rows)
            {
                if (row[primaryCol.Name] != id)
                    continue;
                
                row.BeginEdit();
                foreach (var col in info.Columns)
                {
                    var value = col.Property.GetValue(item, null);
                    if (value != null)
                        row[col.Name] = value;
                }
                row.EndEdit();
                
                break;
            }

            return true;
        }
    }
}