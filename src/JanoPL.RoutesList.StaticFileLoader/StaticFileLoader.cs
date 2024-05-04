using System.Reflection;
using JanoPL.RoutesList.StaticFileLoader.Interfaces;

namespace JanoPL.RoutesList.StaticFileLoader;

public class StaticFileLoader : IStaticFileLoader
{
    private Assembly? _assembly;

    private Stream? _stream;

    public Stream LoadFile(string name)
    {
        _stream = _assembly?.GetManifestResourceStream(name);

        return _stream ?? throw new FileNotFoundException("File cannot be loaded");
    }

    public IStaticFileLoader SetAssembly(Assembly? assembly)
    {
        _assembly = assembly;
        
        return this;
    }

    public void Dispose()
    {
        _stream?.Dispose();
    }
}