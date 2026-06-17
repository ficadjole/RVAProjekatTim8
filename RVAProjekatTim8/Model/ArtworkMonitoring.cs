using RVAProjekatTim8.Enums;
using System;

namespace RVAProjekatTim8.Model
{
    public class ArtworkMonitoring
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ArtworkId { get; set; }
        public DateTime MonitoringTime { get; set; } = DateTime.Now;
        public double LightExposure { get; set; }
        public double AirPollution { get; set; }
        public ArtworkCondition State { get; set; }
    }
}
