using System.Text;
using JanoPL.RoutesList.HtmlBuilder.Structures;

namespace JanoPL.RoutesList.HtmlBuilder;

public interface IHtmlStructuresFactory
{
    ITableStructure CreateTableStructures(StringBuilder sb);
}