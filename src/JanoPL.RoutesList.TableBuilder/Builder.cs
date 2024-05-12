using System.Data;
using System.Reflection;
using JanoPL.RoutesList.TableBuilder.Interfaces;

namespace JanoPL.RoutesList.TableBuilder;

public class Builder : ITableBuilder
{
    public DataTable GenerateDataTable<T>(IList<T> list)
    {
        var table = new DataTable();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties) table.Columns.Add(prop.Name, prop.PropertyType);

        foreach (var item in list) {
            var values = new object?[properties.Length];

            for (var i = 0; i < properties.Length; i++) values[i] = properties[i].GetValue(item, null);

            table.Rows.Add(values);
        }

        return table;
    }
}