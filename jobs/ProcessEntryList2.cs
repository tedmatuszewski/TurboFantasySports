using System;
using jobs.Models;
using jobs.RacerX;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TurboFantasySports;

namespace TFS
{
    public class ProcessEntryList2
    {
        private readonly ILogger _logger;

        private readonly RacerXContext _racerXContext;

        private readonly StorageContext _storageContext;

        public ProcessEntryList2(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProcessEntryList2>();
            _racerXContext = new RacerXContext();
            _storageContext = new StorageContext(_logger);
        }

        [Function("ProcessEntryList2")]
        public void Run([TimerTrigger("0 0 12 * * 4")] TimerInfo myTimer)
        {
            var helper = new ProcessEntryListHelper();
            var entries = new List<RiderRow>();

            helper.Race = _storageContext.GetNextRace();
            helper.Outcomes = _storageContext.GetOutcomes();
            helper.Riders = _storageContext.GetRiders();
            helper.Entries = _storageContext.GetEntries();

            var links = _racerXContext.GetEntryListLinks(helper.Race.Racerx);
            
            foreach(var link in links) 
            {
                var list = _racerXContext.GetEntryList(link.Href, link.Class);

                entries.AddRange(list);
            }

            entries.ForEach(entry =>
            {
                _storageContext.CreateEntry(new EntryRow(helper.Race.RowKey, entry.RowKey, entry.Class));

                if(helper.TryUpdateStatistics(entry)) 
                {
                    _storageContext.UpdateRider(entry);
                }
                else 
                {
                    _storageContext.CreateRider(entry);
                }
            });
        }
    }
}