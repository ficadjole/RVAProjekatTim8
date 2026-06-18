using RVAProjekatTim8.Commands;
using RVAProjekatTim8.Helpers;
using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;
using RVAProjekatTim8.Repositories;
using RVAProjekatTim8.Services;
using RVAProjekatTim8.Validators;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RVAProjekatTim8.ViewModels
{
    public class ArtworkMonitoringListViewModel : INotifyPropertyChanged
    {
        private readonly CommandHistory _commandHistory;
        private readonly ArtworkMonitoringRepository _monitoringRepository;
        private readonly ReadOnlyObservableCollection<Artwork> _availableArtworks;
        private readonly IDialogService _dialogService;
        private readonly IValidator<ArtworkMonitoring> _monitoringValidator;

        public ObservableCollection<ArtworkMonitoring> Monitorings => _monitoringRepository.Monitorings;

        public ICommand AddMonitoringCommand { get; }
        public ICommand EditMonitoringCommand { get; }
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
            ReadOnlyObservableCollection<Artwork> availableArtworks,
            IDialogService dialogService,
            IValidator<ArtworkMonitoring> monitoringValidator)
        {
            _commandHistory = commandHistory;
            _monitoringRepository = monitoringRepository;
            _availableArtworks = availableArtworks;
            _dialogService = dialogService;
            _monitoringValidator = monitoringValidator;

            AddMonitoringCommand = new RelayCommand(ExecuteAddMonitoring);
            EditMonitoringCommand = new RelayCommand(ExecuteEditMonitoring, CanEditMonitoring);
            DeleteMonitoringCommand = new RelayCommand(ExecuteDeleteMonitoring, CanDeleteMonitoring);
        }

        private void ExecuteAddMonitoring(object? parameter)
        {

            var newMonitoring = new ArtworkMonitoring
            {
                ArtworkId = _availableArtworks.FirstOrDefault()?.Id ?? Guid.Empty,
                MonitoringTime = DateTime.Now
            };

            var editViewModel = new ArtworkMonitoringEditViewModel(
                newMonitoring, _monitoringValidator, _availableArtworks);

            var dialogResult = _dialogService.ShowDialog(editViewModel);

            if (dialogResult == true)
            {
                var addedMonitoring = editViewModel.ToModel();
                var command = new AddArtworkMonitoringCommand(Monitorings, _availableArtworks, addedMonitoring);

                try
                {
                    _commandHistory.ExecuteCommand(command);
                }
                catch (InvalidOperationException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void ExecuteEditMonitoring(object? parameter)
        {
            if (SelectedMonitoring is null)
            {
                return;
            }

            var originalSnapshot = CloneMonitoring(SelectedMonitoring);
            var editableModel = CloneMonitoring(SelectedMonitoring);
            var editViewModel = new ArtworkMonitoringEditViewModel(
                editableModel, _monitoringValidator, _availableArtworks);

            var dialogResult = _dialogService.ShowDialog(editViewModel);

            if (dialogResult == true)
            {
                var updatedSnapshot = editViewModel.ToModel();
                var command = new EditArtworkMonitoringCommand(
                    Monitorings, _availableArtworks, originalSnapshot, updatedSnapshot);

                try
                {
                    _commandHistory.ExecuteCommand(command);
                }
                catch (InvalidOperationException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private bool CanEditMonitoring(object? parameter) => SelectedMonitoring is not null;

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

        private static ArtworkMonitoring CloneMonitoring(ArtworkMonitoring source) => new()
        {
            Id = source.Id,
            ArtworkId = source.ArtworkId,
            MonitoringTime = source.MonitoringTime,
            LightExposure = source.LightExposure,
            AirPollution = source.AirPollution,
            State = source.State
        };

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
            (EditMonitoringCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
