using System.Data;
using System.Text;

namespace JanoPL.RoutesList.HtmlBuilder.Structures;

public class TableStructure(StringBuilder sb) : ITableStructure
{
    public string? TableData { get; private set; }

    public void Build()
    {
        //TODO implement build with generic type <T>
        var table = HtmlData.GetData<DataTable>();

        const string theadTag = "$(thead-trow)";
        const string tbodyTag = "$(tbody-trow-data)";


        sb.Replace(theadTag, GetTableColumnTag(table.Columns));
        sb.Replace(tbodyTag, GetTableRowTag(table.Rows));

        TableData = sb.ToString();
    }

    private static string GetTableColumnTag(DataColumnCollection columns)
    {
        var sb = new StringBuilder();
        var last = columns.Count - 1;

        for (var index = 0; index < columns.Count; index++) {
            var column = columns[index];
            var tag = $@"<th scope=""col"">{column}</th>";

            sb.Append(tag);
        }

        return sb.ToString();
    }

    private static string GetTableRowTag(DataRowCollection rowCollection)
    {
        var sb = new StringBuilder();

        foreach (DataRow row in rowCollection) {
            sb.Append("<tr>");

            foreach (var itemRow in row.ItemArray) {
                var tag = $"<td>{(itemRow != null ? itemRow.ToString() : "empty")}</td>";

                sb.Append(tag);
            }

            sb.Append("</tr>");
        }

        return sb.ToString();
    }
}