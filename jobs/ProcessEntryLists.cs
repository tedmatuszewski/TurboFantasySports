using System.Text.Json;
using Azure;
using Azure.Data.Tables;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TurboFantasySports
{
    public class ProcessEntryLists
    {
        private readonly ILogger<ProcessEntryLists> _logger;

        public ProcessEntryLists(ILogger<ProcessEntryLists> logger)
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

        [Function("ProcessEntryLists")]
        public async Task<OkObjectResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            IngestRaceResults();
            
            return new OkObjectResult("Successfully ran function");
        }

        private void IngestRaceResults(string race = null) 
        {    
            var entity = racesClient.Query<TableEntity>()
                .Where(t => DateTime.Parse(t.GetString("Date")) < DateTime.Now)
                .OrderBy(t => DateTime.Parse(t.GetString("Date")))
                .LastOrDefault();
            var riders = ridersClient.Query<TableEntity>();

            if( race == null) 
            {
                race = entity.GetString("Racerx");
            }

            var result450Link = "//*[@id=\"content\"]/div[2]/div/nav/ul/li[2]/ul/li[13]/a";
            var result250Link = "//*[@id=\"content\"]/div[2]/div/nav/ul/li[3]/ul/li[13]/a";
            var burl = "https://racerxonline.com";
            var url = $"{burl}/sx/2025/{race}";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var result450Href = doc.DocumentNode.SelectSingleNode(result450Link).Attributes["href"].Value;
            var result250Href = doc.DocumentNode.SelectSingleNode(result250Link).Attributes["href"].Value;
            var result450 = GetEntryList($"{burl}{result450Href}", 450);
            var result250 = GetEntryList($"{burl}{result250Href}", 250);
            var results = result250.Concat(result450).ToList();
            var teams = ridersClient.Query<TableEntity>();

            foreach(var result in results)
            {
                var rider = riders.FirstOrDefault(t => t.RowKey == result.Rider);

                if(rider == null)
                {
                    _logger.LogInformation($"Rider {result.Rider} not found in database.");

                    var tableEntity = new TableEntity("1", result.Rider)
                    {
                        { "Name", result.Name },
                        { "Number", result.Number },
                        { "ImageUrl", result.ImageUrl },
                        { "Injury", result.Injury },
                        { "Class", result.Class }
                    };

                    ridersClient.AddEntity(tableEntity);
                    
                    _logger.LogInformation($"Added rider {result.Rider} to database.");
                }
                    else if(rider.GetString("Name") != result.Name || rider.GetInt32("Number") != result.Number || rider.GetInt32("Class") != result.Class || rider.GetString("ImageUrl") != result.ImageUrl || rider.GetString("Injury") != result.Injury)
                {    
                    rider["Name"] = result.Name;
                    rider["Number"] = result.Number;
                    rider["ImageUrl"] = result.ImageUrl;
                    rider["Injury"] = result.Injury;
                    rider["Class"] = result.Class;

                    ridersClient.UpdateEntity(rider, ETag.All);
                    
                    _logger.LogInformation($"Updated rider {result.Rider} in database.");
                }
                else
                {
                    _logger.LogInformation($"Rider {result.Rider} already in database.");
                }
            }    

            _logger.LogInformation($"Successfully processed results entry list for {race}.");
        }

        private List<RiderRow> GetEntryList(string url, int classId)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var xpath = "//*[@id=\"content\"]/div[3]/div/table/tbody/tr";
            var table = doc.DocumentNode.SelectNodes(xpath);
            var result = new List<RiderRow>();

            foreach(var row in table)
            {
                var number = 0;
                var cells = row.SelectNodes("td").ToList();
                var numberText = cells[0].InnerText;
                var cell1 = cells[1].InnerHtml;
                var htmlDoc = new HtmlDocument();
                var injury = (string)null;

                int.TryParse(numberText, out number);

                htmlDoc.LoadHtml(cell1);

                var riderTag = htmlDoc.DocumentNode.SelectSingleNode("//a[1]");
                var imageTag = htmlDoc.DocumentNode.SelectSingleNode("//a[1]/img");
                var nameTag = htmlDoc.DocumentNode.SelectSingleNode("//a[2]");

                if(riderTag == null || imageTag == null || nameTag == null)
                {
                    continue;
                }

                var rider = riderTag.Attributes["href"].Value.Replace("/rider/", "").Replace("/races", "");
                var image = imageTag.Attributes["data-src"].Value;
                var name = nameTag.InnerText;
                

                var injuryTag = riderTag.SelectSingleNode("//div");
                if(injuryTag != null)
                {
                    injury = injuryTag.Attributes["title"].Value;
                }

                result.Add(new RiderRow {
                    Number = number,
                    Rider = rider?.Trim(),
                    ImageUrl = image?.Trim(),
                    Name = name?.Trim(),
                    Injury = injury?.Trim(),
                    Class = classId
                });

                _logger.LogInformation($"Processed {rider}");
            }

            return result;
        }
    }

    public class RiderRow
    {
        public int? Number {get; set; }
        public string? Rider {get; set; }
        public string? ImageUrl {get; set; }
        public string? Injury {get; set; }
        public string? Name { get; set; }
        public int? Class {get; set;}
    }
}
