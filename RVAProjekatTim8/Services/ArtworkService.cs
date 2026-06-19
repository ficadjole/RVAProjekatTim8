using Common.Interfaces;
using Common.Models;
using RVAProjekatTim8.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;


namespace RVAProjekatTim8.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ArtworkService : IArtworkService
    {
        private readonly ArtworkRepository _artworkRepository;
        private readonly ArtworkMonitoringRepository _monitoringRepository;

        public ArtworkService(ArtworkRepository artworkRepository,
                              ArtworkMonitoringRepository monitoringRepository)
        {
            _artworkRepository = artworkRepository;
            _monitoringRepository = monitoringRepository;
        }

        public List<Artwork> GetAllArtworks()
        {
            return new List<Artwork>(_artworkRepository.Artworks);
        }


        public List<ArtworkMonitoring> GetMonitoringsByArtworkAndMonth(Guid artworkId, int month, int year)
        {
            return _monitoringRepository.Monitorings
                .Where(m => m.ArtworkId == artworkId
                         && m.MonitoringTime.Month == month
                         && m.MonitoringTime.Year == year)
                .ToList();
        }
    }
}
