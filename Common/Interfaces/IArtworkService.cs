using Common.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace Common.Interfaces
{
    [ServiceContract]
    public interface IArtworkService
    {
        [OperationContract]
        List<Artwork> GetAllArtworks();
        [OperationContract]
        List<ArtworkMonitoring> GetMonitoringsByArtworkAndMonth(Guid artworkId, int month, int year);
    }
}
