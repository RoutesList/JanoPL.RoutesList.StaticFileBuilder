// TODO to refactor

using System.Text;

namespace JanoPL.RoutesList.HtmlBuilder.Structures
{
    public interface ITableStructure
    {
        public string? TableData { get;}
        public void Build();
    }
}
