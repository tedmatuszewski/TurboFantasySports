using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace jobs.Models;

public class RaceModel
{
    public RaceModel(TableEntity? entity)
    {
        if (entity == null)
        {
            return;
        }

        RowKey = entity.RowKey;
        Name = entity.GetString("Name") ?? string.Empty;
        Date = DateTime.Parse(entity.GetString("Date") ?? string.Empty);
        Lites = entity.GetString("Lites") ?? string.Empty;
        Racerx = entity.GetString("Racerx") ?? string.Empty;
    }

    public string RowKey { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Lites { get; set; } = string.Empty;
    
    public string Racerx { get; set; } = string.Empty;
}