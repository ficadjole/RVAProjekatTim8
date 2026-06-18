using Common.Models;
using System.Collections.Generic;

namespace Statistics.Interfaces
{
    public interface IStatisticsStrategy
    {
        string GetName();
        string Calculate(List<ArtworkMonitoring> monitorings);
    }
}
