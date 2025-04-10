using System;
using Azure.Data.Tables;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace jobs.Models;

public class OutcomeModel : Base
{
    public OutcomeModel(TableEntity entity) : base(entity)
    { 
        this.Rider = entity.GetString("Rider") ?? string.Empty;
        this.Race = entity.GetString("Race") ?? string.Empty;
        this.Place = entity.GetInt32("Place") ?? 0;
        this.Points = entity.GetInt32("Points") ?? 0;
    }

    public string Rider { get; set; } = string.Empty;

    public string Race { get; set; } = string.Empty;

    public int Place { get; set; } = 0;

    public int Points { get; set; } = 0;
}
