using RVAProjekatTim8.Enums;
using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;
using System;
using System.Collections.Generic;

namespace RVAProjekatTim8.Services
{
    public class ArtworkMonitoringInitialDataProvider : IInitialDataProvider<ArtworkMonitoring>
    {
        private readonly IReadOnlyList<Artwork> _existingArtworks;

        public ArtworkMonitoringInitialDataProvider(IReadOnlyList<Artwork> existingArtworks)
        {
            _existingArtworks = existingArtworks;
        }

        public List<ArtworkMonitoring> GetInitialData()
        {
            if (_existingArtworks.Count < 3)
            {
                throw new InvalidOperationException(
                    "Potrebno je najmanje 3 Artwork instance za generisanje početnih merenja.");
            }

            return new List<ArtworkMonitoring>
        {
            new()
            {
                ArtworkId = _existingArtworks[0].Id,
                MonitoringTime = DateTime.Now.AddDays(-2),
                LightExposure = 180.5,
                AirPollution = 8.2,
                State = ArtworkCondition.Pristine
            },
            new()
            {
                ArtworkId = _existingArtworks[1].Id,
                MonitoringTime = DateTime.Now.AddDays(-1),
                LightExposure = 320.0,
                AirPollution = 15.7,
                State = ArtworkCondition.MinorWear
            },
            new()
            {
                ArtworkId = _existingArtworks[2].Id,
                MonitoringTime = DateTime.Now,
                LightExposure = 410.5,
                AirPollution = 12.4,
                State = ArtworkCondition.Deteriorating
            }
        };
        }
    }
}
