using Common.Models;
using Statistics.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Statistics.Services
{
    public class CriticalStateStrategy : IStatisticsStrategy
    {
        public string GetName() => "Broj puta u kritičnom stanju (Critical)";

        public string Calculate(List<ArtworkMonitoring> monitorings) =>
            $"Broj merenja u kritičnom stanju: {monitorings.Count(m => m.State == Common.Enums.ArtworkCondition.Critical)}";
        public override string ToString() => GetName();
    }
}
