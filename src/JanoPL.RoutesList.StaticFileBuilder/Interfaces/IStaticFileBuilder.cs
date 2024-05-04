namespace JanoPL.RoutesList.StaticFileBuilder.Interfaces;

public interface IStaticFileBuilder
{
    public void Build();

    public string Results { get; set; }
}