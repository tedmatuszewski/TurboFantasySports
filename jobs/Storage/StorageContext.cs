using System.Security.Cryptography.X509Certificates;
using Azure;
using Azure.Data.Tables;
using jobs.Models;
using Microsoft.Extensions.Logging;
using TurboFantasySports;

public class StorageContext
{
    string partition = "1";
    string accountName = "tedpersonalwebsite";
    string storageAccountKey = "fVAszloqYcVBsKrqpzKOgdnYeInUZCHsX6bIU1l5h5oJ86oyMZzl159Q9o5Xuk4fWB97TQkK02Yv+ASt4/Zw3A==";
    string storageUri;
    TableClient resultsClient;
    TableClient teamsClient;
    TableClient racesClient;
    TableClient outcomesClient;
    TableClient ridersClient;
    TableClient entriesClient;
    private readonly ILogger _logger;
    
    public StorageContext(ILogger logger)
    {
        _logger = logger;
        storageUri = $"https://{accountName}.table.core.windows.net";
        var credential = new TableSharedKeyCredential(accountName, storageAccountKey);
        resultsClient = new TableClient(new Uri(storageUri), "Results", credential);
        teamsClient = new TableClient(new Uri(storageUri), "Teams", credential);
        racesClient = new TableClient(new Uri(storageUri), "Races", credential);
        outcomesClient = new TableClient(new Uri(storageUri), "Outcomes", credential);
        ridersClient = new TableClient(new Uri(storageUri), "Riders", credential);
        entriesClient = new TableClient(new Uri(storageUri), "Entries", credential);
    }

    public RaceModel GetNextRace()
    {
        var entity = racesClient.Query<TableEntity>()
                .Where(t => DateTime.Parse(t.GetString("Date")) > DateTime.Now)
                .OrderBy(t => DateTime.Parse(t.GetString("Date")))
                .FirstOrDefault();

        return new RaceModel(entity);
    }

    public RaceModel GetLastRace()
    {
        var entity = racesClient.Query<TableEntity>()
                .Where(t => DateTime.Parse(t.GetString("Date")) < DateTime.Now)
                .OrderBy(t => DateTime.Parse(t.GetString("Date")))
                .LastOrDefault();

        return new RaceModel(entity);
    }

    public RaceModel GetRace(string key)
    {
        var entity = racesClient.Query<TableEntity>()
                    .Where(e => e.GetString("Racerx") == key)
                    .FirstOrDefault();

        return new RaceModel(entity);
    }

    public List<OutcomeModel> GetOutcomes()
    {
        var outcomes = outcomesClient.Query<TableEntity>().ToList().ConvertAll(e => new OutcomeModel(e));
        
        return outcomes;
    }

    public void CreateOutcome(OutcomeModel row)
    {
        outcomesClient.AddEntity(row.ToTableEntity());
    }

    public List<RiderRow> GetRiders()
    {
        var data = ridersClient.Query<TableEntity>().ToList().ConvertAll(e => new RiderRow(e));

        return data;
    }

    public void CreateRider(RiderRow row)
    {
        ridersClient.AddEntity(row.ToTableEntity());
    }

    public void UpdateRider(RiderRow row)
    {
        ridersClient.UpdateEntity(row.ToTableEntity(), ETag.All);
    }

    public List<EntryRow> GetEntries()
    {
        var data = entriesClient.Query<TableEntity>().ToList().ConvertAll(e => new EntryRow(e));

        return data;
    }

    public List<TeamRow> GetTeams()
    {
        var data = teamsClient.Query<TableEntity>().ToList().ConvertAll(e => new TeamRow(e));

        return data;
    }

    public void CreateEntry(EntryRow row)
    {
        entriesClient.AddEntity(row.ToTableEntity());
    }

    public void CreateResult(ResultRow row)
    {
        resultsClient.AddEntity(row.ToTableEntity());
    }
}