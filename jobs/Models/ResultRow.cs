using System;
using Azure.Data.Tables;

namespace jobs.Models;

public class ResultRow : Base
{
    public ResultRow()
    {
        
    }

    public ResultRow(TableEntity entity) : base(entity)
    {
        this.League = entity.GetString("League");
        this.Member = entity.GetString("Member");
        this.Race = entity.GetString("Race");
        this.Place = entity.GetInt32("Place") ?? 0;
        this.Points = entity.GetInt32("Points") ?? 0;
        this.Rider = entity.GetString("Rider");
    }

    public string League { get; set; }

    public string Member { get; set; }

    public string Race { get; set; }

    public int Place { get; set; }

    public int Points { get; set; }

    public string Rider { get; set; }

    public TableEntity ToTableEntity() 
    {
        return new TableEntity(this.PartitionKey, this.RowKey)
        {
            { "League", this.League },
            { "Member", this.Member },
            { "Place", this.Place },
            { "Points", this.Points },
            { "Rider", this.Rider },
            { "Race", this.Race }
        };
    }
}
