using System;
using Azure.Data.Tables;

namespace jobs.Models;

public class TeamRow : Base
{
    public TeamRow()
    {
        
    }

    public TeamRow(TableEntity entity): base(entity)
    {
        this.League = entity.GetString("League");
        this.Member = entity.GetString("Member");
        this.Rider = entity.GetString("Rider");
    }

    public string League { get; set; }

    public string Member { get; set; }

    public string Rider { get; set; }
}
