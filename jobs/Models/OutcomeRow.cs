using System;
using Azure.Data.Tables;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace jobs.Models;

public class OutcomeModel : Base
{
    public OutcomeModel()
    {
        
    }

    public OutcomeModel(TableEntity entity) : base(entity)
    { 
        this.Rider = entity.GetString("Rider") ?? string.Empty;
        this.Race = entity.GetString("Race") ?? string.Empty;
        this.Place = entity.GetInt32("Place") ?? 0;
        //this.Points = entity.GetInt32("Points") ?? 0;
    }

    public string Rider { get; set; } = string.Empty;

    public string Race { get; set; } = string.Empty;

    public int Place { get; set; } = 0;

    public int Points 
    { 
        get
        {
            switch (this.Place)
            {
                case 1:
                    return 25;
                case 2:
                    return 22;
                case 3:
                    return 20;
                case 4:
                    return 18;
                case 5:
                    return 17;
                case 6:
                    return 16;
                case 7:
                    return 15;
                case 8:
                    return 14;
                case 9:
                    return 13;
                case 10:
                    return 12;
                case 11:
                    return 11;
                case 12:
                    return 10;
                case 13:
                    return 9;
                case 14:
                    return 8;
                case 15:
                    return 7;
                case 16:
                    return 6;
                case 17:
                    return 5;
                case 18:
                    return 4;
                case 19:
                    return 3;
                case 20:
                    return 2;
                case 21:
                    return 1;
                default:
                    return 0;
            }
        }
    }
    
    public TableEntity ToTableEntity()
    {
        var entity = new TableEntity(this.PartitionKey, this.RowKey)
        {
            { "Rider", this.Rider },
            { "Race", this.Race },
            { "Place", this.Place },
            { "Points", this.Points }
        };

        return entity;
    }
}
