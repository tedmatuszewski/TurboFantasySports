using System.Globalization;
using Azure;
using Azure.Data.Tables;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TurboFantasySports
{
    public class ProcessRaceResults
    {
        private readonly ILogger<ProcessRaceResults> _logger;

        public ProcessRaceResults(ILogger<ProcessRaceResults> logger)
        {
            _logger = logger;
        }

        [Function("ProcessRaceResults")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            // https://www.nuget.org/packages/Azure.Data.Tables/
            var accountName = "tedpersonalwebsite";
            var storageAccountKey = "fVAszloqYcVBsKrqpzKOgdnYeInUZCHsX6bIU1l5h5oJ86oyMZzl159Q9o5Xuk4fWB97TQkK02Yv+ASt4/Zw3A==";
            var storageUri = $"https://{accountName}.table.core.windows.net";
            var credential = new TableSharedKeyCredential(accountName, storageAccountKey);
            var resultsClient = new TableClient(new Uri(storageUri), "Results", credential);
            var teamsClient = new TableClient(new Uri(storageUri), "Teams", credential);
            //var race = new Detroit();


            // using (var reader = new StreamReader("C:\\Users\\tznqxt\\Downloads\\teams.csv"))
            // using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            // {
            //     var records = csv.GetRecords<dynamic>().ToList();

            //      foreach (var record in records)
            //      {
            //         try
            //         {
            //             // Process each record
            //             string rider = record.Rider;
            //             string league = record.League;
            //             string member = record.Team;

            //             var tableEntity = new TableEntity("1", Guid.NewGuid().ToString())
            //             {
            //                 { "Rider", rider },
            //                 { "League", league },
            //                 { "Member", member }
            //             };

            //             teamsClient.AddEntity(tableEntity);
            //         }
            //         catch (Exception ex)
            //         {
            //             Console.WriteLine(ex.Message);
            //         }
            //     }
            // }

            Pageable<TableEntity> queryResultsFilter = resultsClient.Query<TableEntity>();

            foreach (TableEntity qEntity in queryResultsFilter)
            {
                var rider = qEntity.GetString("Rider");
                var owner = qEntity.GetString("Member");
                var race = qEntity.GetString("Race");
                var place = qEntity.GetInt32("Place");
                var points = qEntity.GetInt32("Points");
                var key = qEntity.RowKey;

                var tableEntity = new TableEntity("1", key)
                {
                    { "Rider", rider },
                    { "League", "aaf63116-02e6-473d-c778-c55287563a82" },
                    { "Member", owner },
                    { "Race", race },
                    { "Place", place },
                    { "Points", points }
                };

                resultsClient.UpdateEntity(tableEntity, ETag.All);
            }

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
