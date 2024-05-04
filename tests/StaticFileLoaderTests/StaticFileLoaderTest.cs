using JanoPL.RoutesList.StaticFileLoader;
using JanoPL.RoutesList.StaticFileLoader.Interfaces;
using JetBrains.Annotations;

namespace StaticFileLoaderTests;

[TestSubject(typeof(StaticFileLoader))]
public class StaticFileLoaderTest : IDisposable
{
    private readonly IStaticFileLoader _fileLoader = new StaticFileLoader();

    [Fact]
    public void LoadFileTest()
    {
        // Arrange
        const string nameTest = "DataHelper.Data.index.html";
        var stream = LoadTestFile(nameTest);
        var helper = new DataHelper.DataHelper();
        _fileLoader.SetAssembly(helper.GetType().Assembly);

        // Act
        var actualStream = _fileLoader.LoadFile(nameTest);

        // Assert
        Assert.NotNull(actualStream);
        Assert.Equal(stream.Length, actualStream.Length);
    }
    
    private Stream LoadTestFile(string name)
    {
        var helper = new DataHelper.DataHelper();
        var stream = helper.GetType().Assembly.GetManifestResourceStream(name);
        
        Assert.NotNull(stream);

        return stream;
    }

    public void Dispose()
    {
        _fileLoader.Dispose();
    }
}