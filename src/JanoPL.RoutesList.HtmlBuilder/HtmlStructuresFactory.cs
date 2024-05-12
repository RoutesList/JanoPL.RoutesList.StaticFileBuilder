using System.Text;
using JanoPL.RoutesList.HtmlBuilder.Structures;

namespace JanoPL.RoutesList.HtmlBuilder;

public class HtmlStructuresFactory : IHtmlStructuresFactory
{
    public ITableStructure CreateTableStructures(StringBuilder sb)
    {
        return new TableStructure(sb);
    }
}