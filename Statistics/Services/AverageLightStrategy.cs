using Common.Models;
using Statistics.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Statistics.Services
{
    public class AverageLightStrategy : IStatisticsStrategy
    {
        public string GetName() => "Prosečna izloženost svetlosti po delima";

        public string Calculate(List<ArtworkMonitoring> monitorings) =>
            monitorings.Any() ? $"Prosečna izloženost svetlosti: {monitorings.Average(m => m.LightExposure):F2}" : "Nema podataka.";

        public override string ToString() => GetName();
    }
}
