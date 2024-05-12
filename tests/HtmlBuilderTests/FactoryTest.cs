using System.Data;
using System.Text;
using DataHelper;
using JanoPL.RoutesList.HtmlBuilder;
using JanoPL.RoutesList.TableBuilder;

namespace HtmlBuilder;

public class FactoryTest
{
    private readonly IHtmlStructuresFactory _factory = new HtmlStructuresFactory();
    private readonly Builder _tableBuilder = new();

    private Stream LoadTestFile()
    {
        const string name = "DataHelper.Data.TablePartialView.html";
        
        var helper = new DataHelper.TestModel();
        var stream = helper.GetType().Assembly.GetManifestResourceStream(name);
        
        Assert.NotNull(stream);

        return stream;
    }
    
    [Fact]
    public void HtmlStructureFactoryTest()
    {
        // Arrange
        var testModel = new TestModel();
        
        var table = _tableBuilder.GenerateDataTable(testModel.CreateModelList());
        AddRowsToTable(ref table);
        
        var stringBuilder = new StringBuilder(new StreamReader(LoadTestFile()).ReadToEnd());
        var expectedStringBuilder = LoadExpectedPartialTableView();

        // HTMLData helper
        HtmlData.GetInstance();
        HtmlData.Add<DataTable>(table);

        // Act
        var tableStructure = _factory.CreateTableStructures(stringBuilder);
        tableStructure.Build();
        var results = tableStructure.TableData;

        // Assert
        Assert.IsType<string>(results);
        Assert.Equal(expectedStringBuilder.ToString(), results);
    }

    private void AddRowsToTable(ref DataTable table)
    {
        var row = table.NewRow();
        row["Property1"] = "row property 1";
        row["Property2"] = "row property 2";
        row["Property3"] = "row property 3";
        
        table.Rows.Add(row);
    }

    private StringBuilder LoadExpectedPartialTableView()
    {
        var testModel = new TestModel();
        const string file = "DataHelper.Data.PartialTableViewExpected.html";
        var stream = testModel.GetType().Assembly.GetManifestResourceStream(file);

        Assert.NotNull(stream);
        
        StringBuilder sb = new(new StreamReader(stream).ReadToEnd());

        return sb;
    }
}