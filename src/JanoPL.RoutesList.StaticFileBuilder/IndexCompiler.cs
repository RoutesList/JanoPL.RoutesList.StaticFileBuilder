using System;
using System.Collections.Generic;
using System.Text;
using JanoPL.RoutesList.StaticFileBuilder.Models;

namespace JanoPL.RoutesList.StaticFileBuilder;

public class IndexCompiler(StringBuilder stringBuilder, ConfigDto config)
{
    private Dictionary<string, string>? _body;
    private Dictionary<string, string>? _classes;
    private Dictionary<string, string>? _footer;
    private Dictionary<string, string>? _header;

    private static string BodyContent => string.Empty;
    private static string AdditionalHeader => string.Empty;


    public StringBuilder CompileIndex(bool compileHeader = true)
    {
        if (!compileHeader) return stringBuilder;

        GetIndexHeader();
        ReplaceTag(_header);

        return stringBuilder;
    }

    public StringBuilder CompileIndex(bool compileHeader, bool compileBody)
    {
        if (compileHeader) {
            GetIndexHeader();
            ReplaceTag(_header);
        }

        if (compileBody) {
            GetIndexBody();
            ReplaceTag(_body);
        }

        return stringBuilder;
    }

    public StringBuilder CompileIndex(bool compileHeader, bool compileBody, bool compileFooter)
    {
        if (compileHeader) GetIndexHeader();

        if (compileBody) {
            GetIndexBody();
            ReplaceTag(_body);
        }

        if (compileFooter) {
            GetIndexFooter();
            ReplaceTag(_footer);
        }

        return stringBuilder;
    }

    public StringBuilder CompileIndex(
        bool compileHeader,
        bool compileBody,
        bool compileFooter,
        bool compileClasses
    )
    {
        if (compileHeader) {
            GetIndexHeader();
            ReplaceTag(_header);
        }

        if (compileBody) {
            GetIndexBody();
            ReplaceTag(_body);
        }

        if (compileFooter) {
            GetIndexFooter();
            ReplaceTag(_footer);
        }

        if (compileClasses) {
            GetIndexClass();
            ReplaceTag(_classes);
        }

        return stringBuilder;
    }

    private void ReplaceTag(Dictionary<string, string>? data)
    {
        if (data == null) return;

        foreach (var item in data) {
            stringBuilder.Replace(item.Key, item.Value);
        }
    }

    private void GetIndexBody()
    {
        _body = new Dictionary<string, string>
        {
            { "$(body)", BodyContent }
        };
    }

    private void GetIndexHeader()
    {
        _header = new Dictionary<string, string>
        {
            { "$(charsetEncoding)", ConfigDto.CharSet},
            { "$(title)", ConfigDto.Title},
            { "$(additionalHead)", AdditionalHeader },
            { "$(description)", ConfigDto.Description }
        };
    }

    private void GetIndexFooter()
    {
        _footer = new Dictionary<string, string>
        {
            { "$(footer-link)", ConfigDto.FooterLink },
            { "$(footer-text)", ConfigDto.FooterText },
            { "$(footer-year)", DateTime.Now.Year.ToString() }
        };
    }

    private void GetIndexClass()
    {
        _classes = new Dictionary<string, string>
        {
            { "$(table-classes)", config.GetClasses() }
        };
    }
}