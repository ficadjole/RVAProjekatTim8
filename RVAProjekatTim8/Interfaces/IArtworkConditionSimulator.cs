using Common.Models;
using System;


namespace RVAProjekatTim8.Interfaces
{
    public interface IArtworkConditionSimulator
    {
        void StartSimulating(ArtworkMonitoring monitoring);
        void StopSimulating(Guid monitoringId);
    }
}
