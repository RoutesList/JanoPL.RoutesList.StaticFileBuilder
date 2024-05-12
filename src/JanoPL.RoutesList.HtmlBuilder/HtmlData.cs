namespace JanoPL.RoutesList.HtmlBuilder;

public sealed class HtmlData
{
    private static HtmlData? _instance;
    private static Dictionary<Type, object> Data { get; } = new();

    public static HtmlData? GetInstance()
    {
        return _instance ??= new HtmlData();
    }

    public static void Add<T>(object data) where T : class
    {
        var type = typeof(T);

        if (!Data.TryAdd(type, data)) Update<T>(data);
    }

    public static void Update<T>(object data) where T : class
    {
        var type = typeof(T);

        if (Data.ContainsKey(type)) Data[type] = data;
    }

    public static T GetData<T>() where T : class
    {
        var type = typeof(T);
        return (T)Data[type];
    }
}