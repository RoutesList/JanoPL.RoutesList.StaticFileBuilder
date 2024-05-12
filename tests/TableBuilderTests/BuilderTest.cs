using System.Reflection;
using JanoPL.RoutesList.TableBuilder;
using JetBrains.Annotations;

namespace TableBuilderTests;

[TestSubject(typeof(Builder))]
public class BuilderTest
{
    private readonly Builder _tableBuilder = new();
    
    [Fact]
    public void GenerateDataTableTest()
    {
        // Arrange
        PropertyInfo[] properties = typeof(DataHelper.TestModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        IList<DataHelper.TestModel> list = new List<DataHelper.TestModel>();
        for (int i = 1; i <= 3; i++) {
            string text = $"test{i}";
            
            DataHelper.TestModel helper = new()
            {
                Property1 = text,
                Property2 = text,
                Property3 = text
            };
            
            list.Add(helper);
        }

        // Act
        var results = _tableBuilder.GenerateDataTable(list);
        
        // Assert
        Assert.Equal(3, results.Rows.Count);
        
        for (int i = 0; i < properties.Length; i++) {
            Assert.Equal(properties[i].Name, results.Columns[i].ColumnName);   
        }
    }
}