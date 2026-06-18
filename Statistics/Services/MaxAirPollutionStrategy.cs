using Common.Models;
using Statistics.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace Statistics.Services
{
    public class MaxAirPollutionStrategy : IStatisticsStrategy
    {
        public string GetName() => "Maksimalna zagađenost vazduha po delima";

        public string Calculate(List<ArtworkMonitoring> monitorings) =>
            monitorings.Any() ? $"Maksimalna zagađenost vazduha: {monitorings.Max(m => m.AirPollution):F2}" : "Nema podataka.";
        public override string ToString() => GetName();
    }
}
