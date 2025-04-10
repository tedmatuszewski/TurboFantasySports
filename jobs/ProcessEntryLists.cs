// using Azure;
// using Azure.Data.Tables;
// using HtmlAgilityPack;
// using jobs.Models;
// using Microsoft.Azure.Functions.Worker;
// using Microsoft.Extensions.Logging;
// //using Microsoft.Azure.WebJobs;

// namespace TurboFantasySports
// {
//     public class ProcessEntryLists
//     {
//         private readonly ILogger<ProcessEntryLists> _logger;

//         public ProcessEntryLists(ILogger<ProcessEntryLists> logger)
//         {
//             _logger = logger;
//             storageUri = $"https://{accountName}.table.core.windows.net";
//             var credential = new TableSharedKeyCredential(accountName, storageAccountKey);
//             resultsClient = new TableClient(new Uri(storageUri), "Results", credential);
//             teamsClient = new TableClient(new Uri(storageUri), "Teams", credential);
//             racesClient = new TableClient(new Uri(storageUri), "Races", credential);
//             outcomesClient = new TableClient(new Uri(storageUri), "Outcomes", credential);
//             ridersClient = new TableClient(new Uri(storageUri), "Riders", credential);
//             entriesClient = new TableClient(new Uri(storageUri), "Entries", credential);
//         }

//         string partition = "1";
//         string accountName = "tedpersonalwebsite";
//         string storageAccountKey = "fVAszloqYcVBsKrqpzKOgdnYeInUZCHsX6bIU1l5h5oJ86oyMZzl159Q9o5Xuk4fWB97TQkK02Yv+ASt4/Zw3A==";
//         string storageUri;
//         TableClient resultsClient;
//         TableClient teamsClient;
//         TableClient racesClient;
//         TableClient outcomesClient;
//         TableClient ridersClient;
//         TableClient entriesClient;

//         [Function("ProcessEntryList")]
//         public async Task Run([TimerTrigger("0 0 6 * * 4")] Microsoft.Azure.WebJobs.TimerInfo timer)
//         {
//             //var riderList = GetRacerxRiderList();
//             // var outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // var data = ridersClient.Query<TableEntity>().ToList();
//             // var entryList = GetEntryList("anaheim-1");
//             // UpdateData(entryList, data, outcomes);
            
//             // outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // data = ridersClient.Query<TableEntity>().ToList();
//             // entryList = GetEntryList("san-diego");
//             // UpdateData(entryList, data, outcomes);

//             // outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // data = ridersClient.Query<TableEntity>().ToList();
//             // entryList = GetEntryList("anaheim-2");
//             // UpdateData(entryList, data, outcomes);
            
//             // outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // data = ridersClient.Query<TableEntity>().ToList();
//             // entryList = GetEntryList("glendale");
//             // UpdateData(entryList, data, outcomes);
            
//             // outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // data = ridersClient.Query<TableEntity>().ToList();
//             // entryList = GetEntryList("tampa");
//             // UpdateData(entryList, data, outcomes);
            
//             // outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // data = ridersClient.Query<TableEntity>().ToList();
//             // entryList = GetEntryList("detroit");
//             // UpdateData(entryList, data, outcomes);
            
//             // outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // data = ridersClient.Query<TableEntity>().ToList();
//             // entryList = GetEntryList("arlington");
//             // UpdateData(entryList, data, outcomes);
            
//             // outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // data = ridersClient.Query<TableEntity>().ToList();
//             // entryList = GetEntryList("daytona");
//             // UpdateData(entryList, data, outcomes);
            
//             // outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // data = ridersClient.Query<TableEntity>().ToList();
//             // entryList = GetEntryList("indianapolis");
//             // UpdateData(entryList, data, outcomes);
            
//             // outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // data = ridersClient.Query<TableEntity>().ToList();
//             // entryList = GetEntryList("birmingham");
//             // UpdateData(entryList, data, outcomes);
            
//             // outcomes = outcomesClient.Query<TableEntity>().ToList();
//             // data = ridersClient.Query<TableEntity>().ToList();
//             // entryList = GetEntryList("seattle");
//             // UpdateData(entryList, data, outcomes);
            
//             var outcomes = outcomesClient.Query<TableEntity>().ToList();
//             var data = ridersClient.Query<TableEntity>().ToList();
//             var entryList = GetEntryList();

//             UpdateData(entryList, data, outcomes);

//             _logger.LogInformation($"Successfully processed riders");

//             //return new OkObjectResult("Successfully ran function");
//         }

//         private void UpdateData(List<RiderRow> entryList, List<TableEntity> data, List<TableEntity> outcomes) 
//         {
//             foreach(var entry in entryList)
//             {
//                 var entity = data.FirstOrDefault(t => t.RowKey == entry.Rider);
//                 var riderOutcomes = outcomes.Where(t => t.GetString("Rider") == entry.Rider).ToList();
//                 var entries = entity != null ? entity.GetInt32("Entries"): 1; // + 1;
//                 var totalPoints = riderOutcomes.Sum(t => t.GetInt32("Points")) ?? 0;
//                 var totalPlaces = riderOutcomes.Sum(t => t.GetInt32("Place")) ?? 0;
//                 var totalWins = riderOutcomes.Count(t => t.GetInt32("Place") == 1);
//                 var totalPodiums = riderOutcomes.Count(t => t.GetInt32("Place") <= 3);
//                 var totalTop5 = riderOutcomes.Count(t => t.GetInt32("Place") <= 5);
//                 var totalTop10 = riderOutcomes.Count(t => t.GetInt32("Place") <= 10);
//                 var totalOutcomes = riderOutcomes.Count();
//                 var averagePoints = totalOutcomes > 0 ? Math.Round((float)totalPoints / (float)totalOutcomes, 2) : 0;
//                 var averagePlace = totalOutcomes > 0 ? Math.Round((float)totalPlaces / (float)totalOutcomes, 2) : 0;
                
//                 if(entry.Class.Contains("showdown"))
//                 {
//                     entry.Class = entity.GetString("Class");
//                 }

//                 entriesClient.AddEntity(new TableEntity(partition, Guid.NewGuid().ToString())
//                 {
//                     { "Rider", entry.Rider },
//                     { "Class", entry.Class },
//                     { "Race", entry.Race }
//                 });

//                 if(entity == null)
//                 {
//                     _logger.LogInformation($"Rider {entry.Rider} not found in database.");

//                     var tableEntity = new TableEntity(partition, entry.Rider)
//                     {
//                         { "Name", entry.Name },
//                         { "Number", entry.Number },
//                         { "ImageUrl", entry.ImageUrl },
//                         { "Injury", entry.Injury },
//                         { "Class", entry.Class },
//                         { "Entries", 1 },
//                         { "TotalPoints",  totalPoints },
//                         { "TotalPlaces", totalPlaces },
//                         { "Wins",  totalWins },
//                         { "Podiums",  totalPodiums },
//                         { "TopFives",  totalTop5 },
//                         { "TopTens",  totalTop10 },
//                         { "AveragePoints",  averagePoints },
//                         { "AveragePlace",  averagePlace },
//                         { "TotalOutcomes", totalOutcomes }
//                     };

//                     ridersClient.AddEntity(tableEntity);
                    
//                     _logger.LogInformation($"Added rider {entry.Rider} to database.");
//                 }
//                 else
//                 {    
//                     entity["Name"] = entry.Name;
//                     entity["Number"] = entry.Number;
//                     entity["ImageUrl"] = entry.ImageUrl;
//                     entity["Injury"] = entry.Injury;
//                     entity["Class"] = entry.Class;
//                     entity["Entries"] = entries + 1;
//                     entity["TotalPoints"] = totalPoints;
//                     entity["TotalPlaces"] = totalPlaces;
//                     entity["Wins"] = totalWins;
//                     entity["Podiums"] = totalPodiums;
//                     entity["TopFives"] = totalTop5;
//                     entity["TopTens"] = totalTop10;
//                     entity["AveragePoints"] = averagePoints;
//                     entity["AveragePlace"] = averagePlace;
//                     entity["TotalOutcomes"] = totalOutcomes;

//                     ridersClient.UpdateEntity(entity, ETag.All);
                    
//                     _logger.LogInformation($"Updated rider {entry.Rider} in database.");
//                 }
//             }    
//         }

        

//         private List<RiderRow> GetEntryList(string race = null)
//         {  
//             TableEntity entity = null;

//             if( race == null) 
//             {
//                 entity = racesClient.Query<TableEntity>()
//                 .Where(t => DateTime.Parse(t.GetString("Date")) > DateTime.Now)
//                 .OrderBy(t => DateTime.Parse(t.GetString("Date")))
//                 .FirstOrDefault();
//             }
//             else
//             {
//                 entity = racesClient.Query<TableEntity>().FirstOrDefault(e => e.GetString("Racerx") == race);
//             }

//             race = entity.GetString("Racerx");
//             var burl = "https://racerxonline.com";
//             var url = $"{burl}/sx/2025/{race}";
//             var web = new HtmlWeb();
//             var doc = web.Load(url);
//             var entryListLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'entry-list')]");
//             var result250 = new List<RiderRow>();
//             var result450 = new List<RiderRow>();

//             foreach(var link in entryListLinks) {
//                 var href = link.Attributes["href"].Value;
//                 var segments = href.Split('/');
//                 var lites = segments.FirstOrDefault(s => s.Contains("250"));
//                 var heavies = segments.FirstOrDefault(s => s.Contains("450"));
                
//                 if(lites != null) {
//                     result250 = ProcessEntryList($"{burl}{href}", lites);
//                 } else {
//                     result450 = ProcessEntryList($"{burl}{href}", heavies);
//                 }
//             }
            
//             var results = result250.Concat(result450).ToList();

//             results.ForEach(r => r.Race = entity.RowKey);

//             // var result450Link = entryListLinks.FirstOrDefault(l => l.Attributes["href"].Value.Contains("450")).Attributes["href"].Value;
//             // var result250Link = entryListLinks.FirstOrDefault(l => l.Attributes["href"].Value.Contains("250")).Attributes["href"].Value;
//             // var result450 = ProcessEntryList($"{burl}{result450Link}", 450);
//             // var result250 = ProcessEntryList($"{burl}{result250Link}", 250);
//             // var results = result250.Concat(result450).ToList();

//             return results;
//         }

        
//     }
// }
