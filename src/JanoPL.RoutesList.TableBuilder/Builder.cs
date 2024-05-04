using System.Data;
using System.Reflection;
using JanoPL.RoutesList.TableBuilder.Interfaces;

namespace JanoPL.RoutesList.TableBuilder;

public class Builder : ITableBuilder
{
    // public void PrepareColumns()
    // {
    //     CreateIdColumn();
    //
    //     foreach (var dataColumn in DataColumns) {
    //         var column = new DataColumn();
    //         column.ColumnName = dataColumn?.Name;
    //         column.DataType = Type.GetType("System.string");
    //         column.AutoIncrement = false;
    //         column.ReadOnly = false;
    //     }
    // }

    

    // private void CreateIdColumn()
    // {
    //     using DataColumn columnId = new();
    //     columnId.AutoIncrement = true;
    //     columnId.ColumnName = "ID";
    //     columnId.ReadOnly = true;
    //     columnId.Unique = true;
    //     columnId.DataType = Type.GetType("System.Int32");
    //
    //     Table.Columns.Add(columnId);
    // }

    public DataTable GenerateDataTable<T>(IList<T> list)
    {
        DataTable table = new DataTable();

        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties) {
            table.Columns.Add(prop.Name, prop.PropertyType);
        }

        foreach (var item in list) {
            var values = new object?[properties.Length];

            for (int i = 0; i < properties.Length; i++) {
                values[i] = properties[i].GetValue(item, null);
            }

            table.Rows.Add(values);
        }

        return table;
    }
}