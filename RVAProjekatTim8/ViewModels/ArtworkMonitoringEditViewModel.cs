using RVAProjekatTim8.Enums;
using RVAProjekatTim8.Helpers;
using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;
using RVAProjekatTim8.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RVAProjekatTim8.ViewModels
{
    public class ArtworkMonitoringEditViewModel : INotifyPropertyChanged, IDataErrorInfo, ICloseable
    {
        private readonly IValidator<ArtworkMonitoring> _validator;
        private readonly ArtworkMonitoring _monitoring;

        private Guid _artworkId;
        private DateTime _monitoringTime;
        private double _lightExposure;
        private double _airPollution;
        private ArtworkCondition _state;

        public ArtworkMonitoringEditViewModel(
            ArtworkMonitoring monitoring,
            IValidator<ArtworkMonitoring> validator,
            ReadOnlyObservableCollection<Artwork> availableArtworks)
        {
            _monitoring = monitoring;
            _validator = validator;
            AvailableArtworks = availableArtworks;

            _artworkId = monitoring.ArtworkId;
            _monitoringTime = monitoring.MonitoringTime;
            _lightExposure = monitoring.LightExposure;
            _airPollution = monitoring.AirPollution;
            _state = monitoring.State;

            SaveCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, true), _ => IsValid);
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, false));
        }

        /// <summary>Izvor za ComboBox — korisnik bira delo po nazivu, ne po Guid-u.</summary>
        public ReadOnlyObservableCollection<Artwork> AvailableArtworks { get; }

        public Guid ArtworkId
        {
            get => _artworkId;
            set => SetProperty(ref _artworkId, value);
        }

        public DateTime MonitoringTime
        {
            get => _monitoringTime;
            set => SetProperty(ref _monitoringTime, value);
        }

        public double LightExposure
        {
            get => _lightExposure;
            set => SetProperty(ref _lightExposure, value);
        }

        public double AirPollution
        {
            get => _airPollution;
            set => SetProperty(ref _airPollution, value);
        }

        public ArtworkCondition State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        /// <summary>Izvor za State ComboBox — sve vrednosti enuma, generisano automatski.</summary>
        public IReadOnlyList<ArtworkCondition> AvailableStates { get; } = Enum.GetValues(typeof(ArtworkCondition)).Cast<ArtworkCondition>().ToList();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public event EventHandler<bool?>? CloseRequested;

        public bool IsValid =>
            string.IsNullOrEmpty(((IDataErrorInfo)this)[nameof(ArtworkId)])
            && string.IsNullOrEmpty(((IDataErrorInfo)this)[nameof(MonitoringTime)])
            && string.IsNullOrEmpty(((IDataErrorInfo)this)[nameof(LightExposure)])
            && string.IsNullOrEmpty(((IDataErrorInfo)this)[nameof(AirPollution)]);

        public ArtworkMonitoring ToModel()
        {
            _monitoring.ArtworkId = ArtworkId;
            _monitoring.MonitoringTime = MonitoringTime;
            _monitoring.LightExposure = LightExposure;
            _monitoring.AirPollution = AirPollution;
            _monitoring.State = State;
            return _monitoring;
        }

        string IDataErrorInfo.Error => string.Empty;

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                var snapshot = new ArtworkMonitoring
                {
                    ArtworkId = ArtworkId,
                    MonitoringTime = MonitoringTime,
                    LightExposure = LightExposure,
                    AirPollution = AirPollution,
                    State = State
                };

                return _validator.ValidateProperty(snapshot, propertyName);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
            {
                return;
            }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
        }
    }
}
