using Azure.Data.Tables;

namespace jobs.Models;

public class RiderRow : Base
{
    public RiderRow()
    {
        
    }

    public RiderRow(TableEntity entity)
    { 
        this.RowKey = entity.RowKey;
        this.PartitionKey = entity.PartitionKey;
        this.Number = entity.GetInt32("Number") ?? 0;
        this.Rider = entity.GetString("Rider") ?? string.Empty;
        this.ImageUrl = entity.GetString("ImageUrl") ?? string.Empty;
        this.Injury = entity.GetString("Injury") ?? string.Empty;
        this.Name = entity.GetString("Name") ?? string.Empty;
        this.Class = entity.GetString("Class") ?? string.Empty;
        this.Entries = entity.GetInt32("Entries") ?? 1;
        this.AveragePlace = entity.GetDouble("AveragePlace");
        this.AveragePoints = entity.GetDouble("AveragePoints");
        this.Podiums = entity.GetInt32("Podiums");
        this.TopFives = entity.GetInt32("TopFives");
        this.TopTens = entity.GetInt32("TopTens");
        this.TotalOutcomes = entity.GetInt32("TotalOutcomes") ??0;
        this.TotalPlaces = entity.GetInt32("TotalPlaces") ?? 0;
        this.TotalPoints = entity.GetInt32("TotalPoints") ?? 0;
        this.Wins = entity.GetInt32("Wins");
    }
    
    public int? Number {get; set; }
    public string? Rider {get; set; }
    public string? ImageUrl {get; set; }
    public string? Injury {get; set; }
    public string? Name { get; set; }
    public string? Class {get; set;}
    public int Entries {get; set;} =1;
    public double? AveragePlace {get;set;}
    public double? AveragePoints {get;set;}
    public int? Podiums {get;set;}
    public int? TopFives {get;set;}
    public int? TopTens {get;set;}
    public int TotalOutcomes {get;set;}
    public int TotalPlaces {get;set;}
    public int TotalPoints {get;set;}
    public int? Wins {get;set;}

    public TableEntity ToTableEntity()
    {
        var entity = new TableEntity(this.PartitionKey, this.RowKey)
        {
            { "Number", this.Number },
            { "Rider", this.Rider },
            { "ImageUrl", this.ImageUrl },
            { "Injury", this.Injury },
            { "Name", this.Name },
            { "Class", this.Class },
            { "Entries", this.Entries },
            { "AveragePlace", this.AveragePlace },
            { "AveragePoints", this.AveragePoints },
            { "Podiums", this.Podiums },
            { "TopFives", this.TopFives },
            { "TopTens", this.TopTens },
            { "TotalOutcomes", this.TotalOutcomes },
            { "TotalPlaces", this.TotalPlaces },
            { "TotalPoints", this.TotalPoints },
            { "Wins", this.Wins }
        };

        return entity;
    }
}