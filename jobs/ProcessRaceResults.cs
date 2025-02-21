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
            var storageAccountKey = "";
            var storageUri = $"https://{accountName}.table.core.windows.net";
            var credential = new TableSharedKeyCredential(accountName, storageAccountKey);
            var resultsClient = new TableClient(new Uri(storageUri), "Results", credential);
            var teamsClient = new TableClient(new Uri(storageUri), "Teams", credential);
            //var race = new Detroit();


            using (var reader = new StreamReader("C:\\Users\\tznqxt\\Downloads\\teams.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>().ToList();

                 foreach (var record in records)
                 {
                    try
                    {
                        // Process each record
                        string rider = record.Rider;
                        string league = record.League;
                        string member = record.Team;

                        var tableEntity = new TableEntity("1", Guid.NewGuid().ToString())
                        {
                            { "Rider", rider },
                            { "League", league },
                            { "Member", member }
                        };

                        teamsClient.AddEntity(tableEntity);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            //Pageable<TableEntity> queryResultsFilter = teamsClient.Query<TableEntity>();

            // foreach (TableEntity qEntity in queryResultsFilter)
            // {
            //     Console.WriteLine($"{qEntity.GetString("League")}: {qEntity.GetInt32("Rider")}: {qEntity.GetString("Owner")}");
            //     var owner = qEntity.GetString("Owner");
            //     var rider = qEntity.GetInt32("Rider") ?? 0;
            //     var league = qEntity.GetString("League");
            //     var result = race.Results.Find(r => r.Rider == rider);

            //     var tableEntity = new TableEntity("1", Guid.NewGuid().ToString())
            //     {
            //         { "Rider", rider },
            //         { "Race", race.Name },
            //         { "Position", result?.Position ?? 0 },
            //         { "Points", result?.Points ?? 0 },
            //         { "League", league },
            //         { "Owner", owner },
            //         { "Class", result?.Class }
            //     };

            //     resultsClient.AddEntity(tableEntity);
            // }

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
