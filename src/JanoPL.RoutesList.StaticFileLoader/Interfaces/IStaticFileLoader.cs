using System.Reflection;

namespace JanoPL.RoutesList.StaticFileLoader.Interfaces;

public interface IStaticFileLoader : IDisposable
{
    public Stream LoadFile(string name);
    public IStaticFileLoader SetAssembly(Assembly? assembly);
}