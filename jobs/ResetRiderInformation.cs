using System;
using jobs.Models;
using jobs.RacerX;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace TFS
{
    public class ResetRiderInformation
    {
        private readonly ILogger _logger;

        private readonly RacerXContext _racerXContext;

        private readonly StorageContext _storageContext;

        public ResetRiderInformation(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProcessRaceResults2>();
            _racerXContext = new RacerXContext();
            _storageContext = new StorageContext(_logger);
        }

        [Function("ResetRiderInformation")]
        public void Run([HttpTrigger] HttpRequestData req)
        {
            var riders = _storageContext.GetRiders();
            
            foreach(var rider in riders)
            {
                rider.AveragePoints = 0.0;
                rider.Injury = "";
                rider.AveragePlace = 0;
                rider.Podiums = 0;
                rider.TopFives = 0;
                rider.TopTens = 0;
                rider.TotalOutcomes = 0;
                rider.TotalPlaces = 0;
                rider.Wins = 0;
                rider.Entries = 0;
                
                _logger.LogInformation($"Updating rider {rider.Name}");
                _storageContext.UpdateRider(rider);
            }
        }
    }
}
