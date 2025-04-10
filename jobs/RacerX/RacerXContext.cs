using HtmlAgilityPack;
using jobs.Models;

namespace jobs.RacerX;

public class RacerXContext
{
    private HtmlWeb web;

    public RacerXContext() 
    {
        this.web = new HtmlWeb();
    }

    public List<EntryListLink> GetEntryListLinks(string race)
    {
        var burl = "https://racerxonline.com";
        var url = $"{burl}/sx/2025/{race}";
        var web = new HtmlWeb();
        var doc = web.Load(url);
        var entryListLinks = doc.DocumentNode.SelectNodes("//a[contains(@href, 'entry-list')]");
        var result = new List<EntryListLink>();

        foreach(var link in entryListLinks) {
            var href = link.Attributes["href"].Value;
            var segments = href.Split('/');
            var lites = segments.FirstOrDefault(s => s.Contains("250"));
            var heavies = segments.FirstOrDefault(s => s.Contains("450"));
            
            if(lites != null) {
                result.Add(new EntryListLink($"{burl}{href}", lites));
            } 
            
            if(heavies != null) {
                result.Add(new EntryListLink($"{burl}{href}", heavies));
            }
        }

        return result;
    }

    public List<RiderRow> GetEntryList(string url, string lass)
    {
        var web = new HtmlWeb();
        var doc = web.Load(url);
        var xpath = "//*[@id=\"content\"]/div[3]/div/table/tbody/tr";
        var table = doc.DocumentNode.SelectNodes(xpath);
        var result = new List<RiderRow>();

        foreach(var row in table)
        {
            var cells = row.SelectNodes("td").ToList();
            var numberText = cells[0].InnerText;
            var cell1 = cells[1].InnerHtml;
            var htmlDoc = new HtmlDocument();
            var injury = (string)null;

            int number;
            int.TryParse(numberText, out number);

            htmlDoc.LoadHtml(cell1);

            var riderTag = htmlDoc.DocumentNode.SelectSingleNode("//a[1]");
            var imageTag = htmlDoc.DocumentNode.SelectSingleNode("//a[1]/img");
            var nameTag = htmlDoc.DocumentNode.SelectSingleNode("//a[2]");

            if(riderTag == null)
            {
                continue;
            }

            var injuryTag = riderTag.SelectSingleNode("//div");
            var rider = riderTag.Attributes["href"].Value.Replace("/rider/", "").Replace("/races", "");
            var image = imageTag.Attributes["data-src"].Value;
            var name = nameTag.InnerText;

            if(injuryTag != null)
            {
                injury = injuryTag.Attributes["title"].Value;
            }

            result.Add(new RiderRow {
                Number = number,
                RowKey = rider?.Trim(),
                Class = lass,
                ImageUrl = image?.Trim(),
                Name = name?.Trim(),
                Injury = injury?.Trim()
            });

            //_logger.LogInformation($"Processed {rider}");
        }

        return result;
    }

    public List<RiderRow> GetSxTeams() 
    {
        var riders = new List<RiderRow>();
        var url = "https://racerxonline.com/sx/teams";
        var doc = web.Load(url);
        var teams = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[2]/div/div");

        foreach(var team in teams) 
        {
            var lass = team.SelectSingleNode("h3").InnerText;
            var riderTags = team.SelectNodes("ul/li");

            foreach(var rider in riderTags) {
                var riderRow = new RiderRow();
                var riderTag = rider.SelectSingleNode("a");
                var imageTag = rider.SelectSingleNode("a/span/img");
                var badge = rider.SelectSingleNode("a/span/div[@class=\"badge\"]");

                riderRow.Rider = riderTag.Attributes["href"].Value.Replace("/rider/", "");
                riderRow.ImageUrl = imageTag.Attributes["data-src"].Value;
                riderRow.Name = rider.SelectSingleNode("a/span[2]").InnerText.Trim();
                riderRow.Class = lass;
                riderRow.Number = null;

                if(badge != null) {
                    riderRow.Injury = badge.Attributes["title"].Value;
                } else {
                    riderRow.Injury = null;
                }

                riders.Add(riderRow);
            }
        }

        return riders;
    }
}