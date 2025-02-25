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
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

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
        public async Task<OkObjectResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            // https://www.nuget.org/packages/Azure.Data.Tables/
            var accountName = "tedpersonalwebsite";
            var storageAccountKey = "fVAszloqYcVBsKrqpzKOgdnYeInUZCHsX6bIU1l5h5oJ86oyMZzl159Q9o5Xuk4fWB97TQkK02Yv+ASt4/Zw3A==";
            var storageUri = $"https://{accountName}.table.core.windows.net";
            var credential = new TableSharedKeyCredential(accountName, storageAccountKey);
            var resultsClient = new TableClient(new Uri(storageUri), "Results", credential);
            var teamsClient = new TableClient(new Uri(storageUri), "Teams", credential);
            var partition = "1";
            var result450Link = "//*[@id=\"content\"]/div[2]/div/nav/ul/li[2]/ul/li[1]/a";
            var result250Link = "//*[@id=\"content\"]/div[2]/div/nav/ul/li[3]/ul/li[1]/a";
            var burl = "https://racerxonline.com";
            var race = "arlington";
            var url = $"{burl}/sx/2025/{race}";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var result450Href = doc.DocumentNode.SelectSingleNode(result450Link).Attributes["href"].Value;
            var result250Href = doc.DocumentNode.SelectSingleNode(result250Link).Attributes["href"].Value;
            var result450 = GetRaceResults($"{burl}{result450Href}");
            var result250 = GetRaceResults($"{burl}{result250Href}");
            var results = result250.Concat(result450).ToList();
            var teams = teamsClient.Query<TableEntity>();

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
                    { "Race", race }
                };

                resultsClient.AddEntity(tableEntity);
                
                _logger.LogInformation($"Processed result for rider {rider} in position {position} with {points} points.");
            }

            _logger.LogInformation($"Successfully processed results request for {race}.");
            return new OkObjectResult("Welcome to Azure Functions!");
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

                htmlDoc.LoadHtml(i);

                var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//a[@class='headshot']");
                var ii = htmlBody.Attributes["href"].Value.Replace("/rider/", "").Replace("/races", "");

                result.Add(new KeyValuePair<int, string>(int.Parse(position), ii));
                _logger.LogInformation($"{position} {ii}");
            }

            return result;
        }

            // using (PdfDocument document = PdfDocument.Open(@"C:\Users\tznqxt\Downloads\Overall_Results_861585.pdf"))
            // {
            //     foreach (Page page in document.GetPages())
            //     {
            //         string pageText = page.Text;

            //         foreach (Word word in page.GetWords())
            //         {
            //             Console.WriteLine(word.Text);
            //         }
            //     }
            // }

            // string jsonFilePath = "C:\\Users\\tznqxt\\source\\repos\\Github\\TurboFantasySports\\jobs\\data\\Races\\2025\\arlington.json";
            // string jsonString = File.ReadAllText(jsonFilePath);
            // var json = JsonSerializer.Deserialize<List<dynamic>>(jsonString);

            // foreach (var record in json)
            // {
            //     try
            //     {
            //         // Process each record
            //         string number = record.number;
            //         string position = record.position;

            //         var tableEntity = new TableEntity("1", Guid.NewGuid().ToString())
            //         {
            //             { "League", "aaf63116-02e6-473d-c778-c55287563a82" },
            //             { "Rider", rider },
            //             { "Member", member }

            //             { "Rider", rider },
            //             { "Member", owner },
            //             { "Race", race },
            //             { "Place", place },
            //             { "Points", points }
            //         };

            //         resultsClient.AddEntity(tableEntity);
            //     }
            //     catch (Exception ex)
            //     {
            //         Console.WriteLine(ex.Message);
            //     }
            // }

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
            //         }b
            //         catch (Exception ex)
            //         {
            //             Console.WriteLine(ex.Message);
            //         }
            //     }
            // }

            // Pageable<TableEntity> queryResultsFilter = resultsClient.Query<TableEntity>();

            // foreach (TableEntity qEntity in queryResultsFilter)
            // {
            //     var rider = qEntity.GetString("Rider");
            //     var owner = qEntity.GetString("Member");
            //     var race = qEntity.GetString("Race");
            //     var place = qEntity.GetInt32("Place");
            //     var points = qEntity.GetInt32("Points");
            //     var key = qEntity.RowKey;

            //     var tableEntity = new TableEntity("1", key)
            //     {
            //         { "Rider", rider },
            //         { "League", "aaf63116-02e6-473d-c778-c55287563a82" },
            //         { "Member", owner },
            //         { "Race", race },
            //         { "Place", place },
            //         { "Points", points }
            //     };

            //     resultsClient.UpdateEntity(tableEntity, ETag.All);
            // }
    }
}
