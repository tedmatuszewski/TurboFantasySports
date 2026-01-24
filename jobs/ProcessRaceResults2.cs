using System;
using jobs.Models;
using jobs.RacerX;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TFS
{
    public class ProcessRaceResults2
    {
        private readonly ILogger _logger;
        private readonly RacerXContext _racerXContext;

        private readonly StorageContext _storageContext;

        public ProcessRaceResults2(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProcessRaceResults2>();
            _racerXContext = new RacerXContext();
            _storageContext = new StorageContext(_logger);
        }

        [Function("ProcessRaceResults2")]
        public void Run([TimerTrigger("0 0 12 * * 1")] TimerInfo myTimer)
        {
            var race =_storageContext.GetLastRace();
            _logger.LogInformation($"Found race {race.Name}");

            var links = _racerXContext.GetRaceResultLinks(race.Racerx);
            _logger.LogInformation($"Found {links.Count} links for race results");

            var teams = _storageContext.GetTeams().Where(r => r.IsBench == false).ToList();
            _logger.LogInformation($"Found {teams.Count} teams");

            var outcomes = new List<OutcomeModel>();
            
            foreach(var link in links) 
            {
                var list = _racerXContext.GetResultList(link.Href, race.RowKey);
                _logger.LogInformation($"Found {list.Count} results for {link.Class}");

                outcomes.AddRange(list);
            }

            _logger.LogInformation($"Gathered {outcomes.Count} outcomes from RacerX");

            foreach(var outcome in outcomes)
            {
                _logger.LogInformation($"Creating outcome for {outcome.Race}, {outcome.Place}, {outcome.Rider}");
                _storageContext.CreateOutcome(outcome);
            }

            foreach(var team in teams)
            {
                _logger.LogInformation($"Creating result for {team.Rider} in {team.League}");

                var result = new ResultRow();
                var outcome = outcomes.SingleOrDefault(o => o.Rider == team.Rider);

                result.League = team.League;
                result.Member = team.Member;
                result.Race = race.RowKey;
                result.Place = outcome?.Place ?? 0;
                result.Points = outcome?.Points ?? 0;
                result.Rider = team.Rider;

                _storageContext.CreateResult(result);
            }
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
