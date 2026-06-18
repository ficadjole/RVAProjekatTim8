using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Statistics.Clients
{
    public class ArtworkServiceClient
    {
        private readonly IArtworkService _channel;

        public ArtworkServiceClient()
        {
            var binding = new NetTcpBinding();
            var endpoint = new EndpointAddress("net.tcp://localhost:8523/ArtworkService");
            var factory = new ChannelFactory<IArtworkService>(binding, endpoint);
            _channel = factory.CreateChannel();
        }

        public List<Artwork> GetAllArtworks()
        {
            return _channel.GetAllArtworks();
        }

        public List<ArtworkMonitoring> GetAllMonitorings()
        {
            return _channel.GetAllMonitorings();
        }

        public List<ArtworkMonitoring> GetMonitoringsByArtworkAndMonth(Guid artworkId, int month, int year)
        {
            return _channel.GetMonitoringsByArtworkAndMonth(artworkId, month, year);
        }
    }
}
