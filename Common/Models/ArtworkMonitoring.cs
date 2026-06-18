using Common.Enums;
using System;
using System.ComponentModel;

namespace Common.Models
{
    public class ArtworkMonitoring : INotifyPropertyChanged
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ArtworkId { get; set; }
        public DateTime MonitoringTime { get; set; } = DateTime.Now;
        public double LightExposure { get; set; }
        public double AirPollution { get; set; }

        private ArtworkCondition _state;
        public ArtworkCondition State
        {
            get => _state;
            set
            {
                if(_state == value)
                {
                    return;
                }

                _state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
