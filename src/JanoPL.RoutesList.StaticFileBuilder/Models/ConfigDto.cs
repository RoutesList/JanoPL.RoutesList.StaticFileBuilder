using Microsoft.CSharp.RuntimeBinder;

namespace JanoPL.RoutesList.StaticFileBuilder.Models;

public class ConfigDto
{
    private dynamic _classes = "table";
    public static string Title => "RoutesList";
    public static string CharSet => "UTF-8";
    public static string FooterLink => "https://github.com/JanoPL/Routeslist";
    public static string FooterText => "RoutesList";

    public static string Description =>
        "Routing debugger for DotNet Core applications. A list of all routes in the formatted table";

    public dynamic GetClasses()
    {
        return _classes;
    }

    public void SetClasses(dynamic value)
    {
        _classes = value switch
        {
            string text => text,
            string[] array => array,
            null => new[] { "table" },
            _ => throw new RuntimeBinderException(
                $"It should be one of type string of string[], you provide: {value.GetType().Name}")
        };
    }
}