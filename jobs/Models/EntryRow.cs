using Azure.Data.Tables;

namespace jobs.Models;

public class EntryRow : Base
{
    public EntryRow(TableEntity entity): base(entity)
    {
        this.Race = entity.GetString("Race") ?? string.Empty;
        this.Rider = entity.GetString("Rider") ?? string.Empty;
        this.Class = entity.GetString("Class") ?? string.Empty;
    }

    public EntryRow(string race, string rider, string clas) : base()
    {
        this.Race = race;
        this.Rider = rider;
        this.Class = clas;
    }

    public string Race { get; set; } = string.Empty;

    public string Rider { get; set; } = string.Empty;

    public string Class { get; set; } = string.Empty;

    public TableEntity ToTableEntity() 
    {
        return new TableEntity(this.PartitionKey, this.RowKey)
        {
            { "Rider", this.Rider },
            { "Class", this.Class },
            { "Race", this.Race }
        };
    }
}
