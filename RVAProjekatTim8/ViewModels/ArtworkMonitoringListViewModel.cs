using RVAProjekatTim8.Commands;
using RVAProjekatTim8.Helpers;
using RVAProjekatTim8.Model;
using RVAProjekatTim8.Repositories;
using RVAProjekatTim8.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RVAProjekatTim8.ViewModels
{
    public class ArtworkMonitoringListViewModel : INotifyPropertyChanged
    {
        private readonly CommandHistory _commandHistory;
        private readonly ArtworkMonitoringRepository _monitoringRepository;
        private readonly ReadOnlyObservableCollection<Artwork> _availableArtworks;

        public ObservableCollection<ArtworkMonitoring> Monitorings => _monitoringRepository.Monitorings;

        public ICommand AddMonitoringCommand { get; }
        public ICommand DeleteMonitoringCommand { get; }

        private ArtworkMonitoring? _selectedMonitoring;
        public ArtworkMonitoring? SelectedMonitoring
        {
            get => _selectedMonitoring;
            set => SetProperty(ref _selectedMonitoring, value);
        }

        public ArtworkMonitoringListViewModel(
            CommandHistory commandHistory,
            ArtworkMonitoringRepository monitoringRepository,
            ReadOnlyObservableCollection<Artwork> availableArtworks)
        {
            _commandHistory = commandHistory;
            _monitoringRepository = monitoringRepository;
            _availableArtworks = availableArtworks;

            AddMonitoringCommand = new RelayCommand(ExecuteAddMonitoring);
            DeleteMonitoringCommand = new RelayCommand(ExecuteDeleteMonitoring, CanDeleteMonitoring);
        }

        private void ExecuteAddMonitoring(object? parameter)
        {
            if (parameter is not ArtworkMonitoring newMonitoring)
            {
                return;
            }

            var command = new AddArtworkMonitoringCommand(Monitorings, _availableArtworks, newMonitoring);

            try
            {
                _commandHistory.ExecuteCommand(command);
            }
            catch (InvalidOperationException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void ExecuteDeleteMonitoring(object? parameter)
        {
            if (SelectedMonitoring is null)
            {
                return;
            }

            var command = new DeleteArtworkMonitoringCommand(Monitorings, SelectedMonitoring);
            _commandHistory.ExecuteCommand(command);
            SelectedMonitoring = null;
        }

        private bool CanDeleteMonitoring(object? parameter) => SelectedMonitoring is not null;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
            {
                return;
            }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            (DeleteMonitoringCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
