using System.Data;

namespace JanoPL.RoutesList.TableBuilder.Interfaces;

public interface ITableBuilder
{
    public DataTable GenerateDataTable<T>(IList<T> list);
}