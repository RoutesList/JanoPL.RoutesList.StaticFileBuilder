using System.Collections.Generic;

namespace DataHelper;

public class TestModel
{
    public string? Property1 { get; set; }
    public string? Property2 { get; set; }
    public string? Property3 { get; set; }

    public IList<TestModel> CreateModelList()
    {
        var test = new TestModel()
        {
            Property1 = "test 1",
            Property2 = "test 2",
            Property3 = "test 3",
        };

        IList<TestModel> list = new List<TestModel>();
        list.Add(test);

        return list;
    }
}