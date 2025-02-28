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

            //UpdateTableData(resultsClient, teamsClient);

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
            return new OkObjectResult("Successfully ran function");
        }

        
        private void UpdateTableData(TableClient resultsClient, TableClient teamsClient)
        {
            Pageable<TableEntity> queryResultsFilter = resultsClient.Query<TableEntity>();

            var mapping = new Dictionary<string, string> {
                { "madeup4", "095a3bae-4f35-4725-950f-1ef6c517be2b" },
                { "madeup5", "297462b0-b36c-4a6e-aa33-454d8c445f8a" },
                { "madeup7", "6a078a96-e2f4-4caf-8ae0-a38547a1cbba" },
                { "madeup2", "6a7395a7-5ab9-423e-bf64-7f2a830b441f" },
                { "madeup6", "74389f62-332a-47f5-9291-32f87ba8f3c4" },
                { "facebook|10230824544294701", "7c485c51-1671-4df3-8349-85ec3149ca1e" },
                { "facebook|10221474549733347", "9c348298-43e5-4b4a-864d-3eb12163d142" },
                { "madeup1", "b15cd754-a18c-4f0a-9838-5aa7ad25dc1c" },
                { "madeup3", "cad4701c-34c2-4272-8322-17cd8fb12a79" },
                { "facebook|10231970315453351", "4b5693ce-f34a-4ba0-b71d-4302c7cb17b6" }
            };


            foreach (TableEntity qEntity in queryResultsFilter)
            {
                var old = qEntity.GetString("Member");

                if(mapping.ContainsKey(old) == false)
                {
                    continue;
                }

                var member = mapping[old];

                qEntity["Member"] = member;

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

                htmlDoc.LoadHtml(i);

                var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//a[@class='headshot']");
                var ii = htmlBody.Attributes["href"].Value.Replace("/rider/", "").Replace("/races", "");

                result.Add(new KeyValuePair<int, string>(int.Parse(position), ii));
                _logger.LogInformation($"{position} {ii}");
            }

            return result;
        }

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
    }
}
