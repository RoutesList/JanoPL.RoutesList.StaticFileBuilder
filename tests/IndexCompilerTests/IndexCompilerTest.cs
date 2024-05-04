using System.Text;
using JanoPL.RoutesList.StaticFileBuilder;
using JanoPL.RoutesList.StaticFileBuilder.Models;
using JanoPL.RoutesList.StaticFileLoader.Interfaces;
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
        staticFileLoaderMock.Setup(loader =>
            loader.LoadFile(nameTest)).Returns(_stream);
        
        var sb = new StringBuilder(new StreamReader(_stream).ReadToEnd());

        var configDto = new ConfigDto();

        // Act
        var actualIndexCompiler = new IndexCompiler(sb, configDto);
        var actualSb = actualIndexCompiler.CompileIndex();

        // Assert
        
        Assert.Equal(sb.ToString(), actualSb.ToString());
    }

    private static Stream LoadTestFile(string name)
    {
        var helper = new DataHelper.DataHelper();
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