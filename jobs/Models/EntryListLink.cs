using System;

namespace jobs.Models;

public class EntryListLink
{
    public EntryListLink(string href, string className)
    {
        Href = href;
        Class = className;
    }
    
    public string Href { get; set; }

    public string Class { get; set; }
}
