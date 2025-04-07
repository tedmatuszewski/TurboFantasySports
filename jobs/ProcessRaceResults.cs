using System.Globalization;
using System.Text.Json;
using Azure;
using Azure.Data.Tables;
using CsvHelper;
using HtmlAgilityPack;
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
            storageUri = $"https://{accountName}.table.core.windows.net";
            var credential = new TableSharedKeyCredential(accountName, storageAccountKey);
            resultsClient = new TableClient(new Uri(storageUri), "Results", credential);
            teamsClient = new TableClient(new Uri(storageUri), "Teams", credential);
            racesClient = new TableClient(new Uri(storageUri), "Races", credential);
            outcomesClient = new TableClient(new Uri(storageUri), "Outcomes", credential);
            ridersClient = new TableClient(new Uri(storageUri), "Riders", credential);
        }

        string partition = "1";
        string accountName = "tedpersonalwebsite";
        string storageAccountKey = "fVAszloqYcVBsKrqpzKOgdnYeInUZCHsX6bIU1l5h5oJ86oyMZzl159Q9o5Xuk4fWB97TQkK02Yv+ASt4/Zw3A==";
        string storageUri;
        TableClient resultsClient;
        TableClient teamsClient;
        TableClient racesClient;
        TableClient outcomesClient;
        TableClient ridersClient;

        [Function("ProcessRaceResults")]
        public async Task Run([TimerTrigger("0 0 12 * * 0")] Microsoft.Azure.WebJobs.TimerInfo timer)
        {
            //CreateRaces();
            // https://www.nuget.org/packages/Azure.Data.Tables/

            // var races = racesClient.Query<TableEntity>()
            //     .Where(t => DateTime.Parse(t.GetString("Date")) < DateTime.Now)
            //     .OrderBy(t => DateTime.Parse(t.GetString("Date")))
            //     .ToList();

            // foreach(var race in races) {
            //     var rx = race.GetString("Racerx");
            //     IngestRaceResults(rx);
            // }
            //UpdateTableData();
            IngestRaceResults();
            //IngestJsonFile();
        }

        private void CreateRaces()
        {
            string jsonFilePath = "C:\\Users\\tznqxt\\source\\repos\\Github\\TurboFantasySports\\client\\src\\data\\races.json";
            string jsonString = File.ReadAllText(jsonFilePath);
            var races = JsonSerializer.Deserialize<List<Race>>(jsonString);

            foreach(var race in races) 
            {
                string name = race.name;
                
                var tableEntity = new TableEntity("1", Guid.NewGuid().ToString())
                {
                    { "Name", name },
                    { "Date", race.date },
                    { "Lites", race.@class },
                    { "Racerx", race.key }
                };

                racesClient.AddEntity(tableEntity);
            }
        }

        private void IngestRaceResults(string race = null) 
        {    
            var races = racesClient.Query<TableEntity>()
                .Where(t => DateTime.Parse(t.GetString("Date")) < DateTime.Now)
                .OrderBy(t => DateTime.Parse(t.GetString("Date")))
                .ToList();

            TableEntity raceKey = null;

            if( race == null) 
            {
                raceKey = races.LastOrDefault();
            }
            else 
            {
                raceKey = races.LastOrDefault(r => r.GetString("Racerx") == race);
            }

            _logger.LogInformation($"Found race {raceKey}");

            var result450Link = "//*[@id=\"content\"]/div[2]/div/nav/ul/li[2]/ul/li[1]/a";
            var result250Link = "//*[@id=\"content\"]/div[2]/div/nav/ul/li[3]/ul/li[1]/a";
            var burl = "https://racerxonline.com";
            var url = $"{burl}/sx/2025/{raceKey.GetString("Racerx")}";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var result450Href = doc.DocumentNode.SelectSingleNode(result450Link).Attributes["href"].Value;
            var result250Href = doc.DocumentNode.SelectSingleNode(result250Link).Attributes["href"].Value;
            var result450 = GetRaceResults($"{burl}{result450Href}");
            var result250 = GetRaceResults($"{burl}{result250Href}");
            var results = result250.Concat(result450).ToList();
            var teams = teamsClient.Query<TableEntity>();

            _logger.LogInformation($"Gathered data");

            foreach(var result in results)
            {
                outcomesClient.AddEntity(new TableEntity(partition, Guid.NewGuid().ToString())
                {
                    { "Rider", result.Value },
                    { "Race", raceKey.RowKey },
                    { "Place", result.Key },
                    { "Points", ConvertPositionToPoints(result.Key) },
                });

               _logger.LogInformation($"Adding race outcome for {result.Value} in position {result.Key}.");
            }    

            foreach(var team in teams) 
            {
                var rider = team.GetString("Rider");
                var league = team.GetString("League");
                var member = team.GetString("Member");
                var result = results.SingleOrDefault(r => r.Value == rider);
                var position = 0;
                var points = 0;

                if(result.Equals(default(KeyValuePair<int, string>)) == false) 
                {
                    position = result.Key;
                    points = ConvertPositionToPoints(position);
                }

                var tableEntity = new TableEntity(partition, Guid.NewGuid().ToString())
                {
                    { "Rider", rider },
                    { "League", league },
                    { "Member", member },
                    { "Place", position },
                    { "Points", points },
                    { "Race", raceKey.RowKey }
                };

                resultsClient.AddEntity(tableEntity);
                
               _logger.LogInformation($"Processed result for rider {rider} in position {position} with {points} points.");
            }

            _logger.LogInformation($"Successfully processed results request for {race}.");
        }

        private void UpdateTableData()
        {
            var queryResultsFilter = resultsClient.Query<TableEntity>();
            var races = racesClient.Query<TableEntity>();

            foreach (TableEntity qEntity in queryResultsFilter)
            {
                var old = qEntity.GetString("Race");
                var race = races.SingleOrDefault(r => r.GetString("Racerx") == old);
   
                qEntity["Race"] = race.RowKey;

                resultsClient.UpdateEntity(qEntity, ETag.All);
            }
        }

        private int ConvertPositionToPoints(int position) 
        {
            switch (position)
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

        private List<KeyValuePair<int, string>> GetRaceResults(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var xpath = "//*[@id=\"content\"]/div[3]/div/table/tbody/tr";
            var table = doc.DocumentNode.SelectNodes(xpath);
            var result = new List<KeyValuePair<int, string>>();

            foreach(var row in table) 
            {
                var cells = row.SelectNodes("td").ToList();
                var position = cells[0].InnerText;
                var i = cells[1].InnerHtml;
                var htmlDoc = new HtmlDocument();
                var intPosition = 22; // Likely dnf. Make their position 22 since that is last place.

                int.TryParse(position, out intPosition);

                htmlDoc.LoadHtml(i);

                var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//a[@class='headshot']");
                var ii = htmlBody.Attributes["href"].Value.Replace("/rider/", "").Replace("/races", "");

                result.Add(new KeyValuePair<int, string>(intPosition, ii));
                _logger.LogInformation($"{position} {ii}");
            }

            return result;
        }

        private void IngestJsonFile()
        {
            string jsonFilePath = "C:\\Users\\tznqxt\\source\\repos\\Github\\TurboFantasySports\\client\\src\\data\\riders.json";
            string jsonString = File.ReadAllText(jsonFilePath);
            var json = JsonSerializer.Deserialize<List<Race>>(jsonString);

            foreach (var record in json)
            {
                var tableEntity = new TableEntity("1", record.id)
                {
                    { "Name", record.name },
                    { "Number", record.number },
                    { "Class", record.@class },
                    { "IsInjured", false }
                };

                ridersClient.AddEntity(tableEntity);
            }
        }
    }

    public class Race 
    {
        public string id {get;set;}
        public string name { get; set; }
        public string date { get; set; }
        public int number {get;set;}
        public int @class { get; set; }
        public string key { get; set; }
    }
}
