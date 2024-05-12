using System.Text;
using JanoPL.RoutesList.StaticFileBuilder;
using JanoPL.RoutesList.StaticFileBuilder.Models;
using JanoPL.RoutesList.StaticFileLoader.Interfaces;
using JanoPL.RoutesList.TableBuilder;
using JanoPL.RoutesList.TableBuilder.Interfaces;
using JetBrains.Annotations;
using Moq;

namespace IndexCompilerTests;

[TestSubject(typeof(IndexCompiler))]
public class IndexCompilerTest : IDisposable
{
    private readonly Stream _stream = LoadTestFile("DataHelper.Data.index.html");
    
    [Fact]
    public void IndexCompilerHeader()
    {
        // Arrange
        const string nameTest = "JanoPL.RoutesList.StaticFileBuilder.Resources.StaticFile.index.html";
        var staticFileLoaderMock = new Mock<IStaticFileLoader>();
        var sb = new StringBuilder(new StreamReader(_stream).ReadToEnd());
        var configDto = new ConfigDto();
        
        staticFileLoaderMock.Setup(loader => loader.LoadFile(nameTest)).Returns(_stream);

        // Act
        var actualIndexCompiler = new IndexCompiler(sb, configDto);
        var actualSb = actualIndexCompiler.CompileIndex();

        // Assert
        Assert.Equal(sb.ToString(), actualSb.ToString());
    }

    [Fact]
    public void IndexCompilerBody()
    {
        // Arrange
        const string nameTest = "JanoPL.RoutesList.StaticFileBuilder.Resources.StaticFile.index.html";

        var staticFileLoaderMock = new Mock<IStaticFileLoader>();
        // var tableBuilderMock = new Mock<ITableBuilder>();
        var tableBuilder = new Builder();
        var sb = new StringBuilder(new StreamReader(_stream).ReadToEnd());
        var configDto = new ConfigDto();
        List<DataHelper.TestModel> list = new();
        var testModel = new DataHelper.TestModel()
        {
            Property1 = "test",
            Property2 = "test",
            Property3 = "test"
        };
        list.Add(testModel);
        var table = tableBuilder.GenerateDataTable(list);

        staticFileLoaderMock.Setup(loader => loader.LoadFile(nameTest)).Returns(_stream);
        
        //TODO add HtmlBuilder here to test index compiler

        // Act
        var actualIndexCompiler = new IndexCompiler(sb, configDto);
        // actualIndexCompiler.BodyContent = bodyContent;
        var actualSb = actualIndexCompiler.CompileIndex(false, true);

        // Assert
        Assert.Equal(sb.ToString(), actualSb.ToString());
    }

    private static Stream LoadTestFile(string name)
    {
        var helper = new DataHelper.TestModel();
        var stream = helper.GetType().Assembly
            .GetManifestResourceStream(name);
        
        Assert.NotNull(stream);

        return stream;
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}