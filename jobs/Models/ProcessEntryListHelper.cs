using System;

namespace jobs.Models;

public class ProcessEntryListHelper
{
    public RaceModel Race { get; set; }

    public List<OutcomeModel> Outcomes { get; set; }

    public List<EntryRow> Entries { get; set; }

    public List<RiderRow> Riders { get; set; }

    public bool TryUpdateStatistics(RiderRow entry)
    {
        var localEntry = entry;
        var entity = this.Riders.FirstOrDefault(t => t.RowKey == localEntry.RowKey);

        if(entity == null)
        {
            return false;
        }

        var riderOutcomes = this.Outcomes.Where(t => t.Rider == entry.RowKey).ToList();

        entry.Entries = Entries.Count(t => t.Rider == entry.RowKey) + 1;
        entry.TotalPoints = riderOutcomes.Sum(t => t.Points);
        entry.TotalPlaces = riderOutcomes.Sum(t => t.Place);
        entry.Wins = riderOutcomes.Count(t => t.Place == 1);
        entry.Podiums = riderOutcomes.Count(t => t.Place <= 3);
        entry.TopFives = riderOutcomes.Count(t => t.Place <= 5);
        entry.TopTens = riderOutcomes.Count(t => t.Place <= 10);
        entry.TotalOutcomes = riderOutcomes.Count();
        entry.AveragePoints = entry.TotalOutcomes > 0 ? Math.Round((double)entry.TotalPoints / (double)entry.TotalOutcomes, 2) : 0;
        entry.AveragePlace = entry.TotalOutcomes > 0 ? Math.Round((double)entry.TotalPlaces / (double)entry.TotalOutcomes, 2) : 0;
        entry.PartitionKey = entity.PartitionKey;

        if (entry.Class.Contains("showdown"))
        {
            entry.Class = entity.Class;
        }

        return true;
    }
}
