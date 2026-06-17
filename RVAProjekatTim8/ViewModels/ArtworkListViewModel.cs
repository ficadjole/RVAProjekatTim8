using RVAProjekatTim8.Commands;
using RVAProjekatTim8.Helpers;
using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;
using RVAProjekatTim8.Repositories;
using RVAProjekatTim8.Services;
using RVAProjekatTim8.Validators;
using RVAProjekatTim8.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RVAProjekatTim8.ViewModels
{
    public class ArtworkListViewModel : INotifyPropertyChanged
    {
        private readonly CommandHistory _commandHistory;
        private readonly IDialogService _dialogService;
        private readonly IValidator<Artwork> _artworkValidator;
        private readonly ArtworkRepository _artworkRepository;

        /// <summary>
        /// Izlaže direktno repository kolekciju. DataGrid u ArtworkListView.xaml
        /// bind-uje se na ovo svojstvo i automatski reaguje na CollectionChanged
        /// (Add/Remove/Replace) jer je tip ObservableCollection.
        /// </summary>
        public ObservableCollection<Artwork> Artworks => _artworkRepository.Artworks;

        public ICommand AddArtworkCommand { get; }
        public ICommand DeleteArtworkCommand { get; }
        public ICommand EditArtworkCommand { get; }

        private Artwork? _selectedArtwork;
        public Artwork? SelectedArtwork
        {
            get => _selectedArtwork;
            set => SetProperty(ref _selectedArtwork, value);
        }

        public ArtworkListViewModel(
            CommandHistory commandHistory,
            IDialogService dialogService,
            IValidator<Artwork> artworkValidator,
            ArtworkRepository artworkRepository)
        {
            _commandHistory = commandHistory;
            _dialogService = dialogService;
            _artworkValidator = artworkValidator;
            _artworkRepository = artworkRepository;

            AddArtworkCommand = new RelayCommand(ExecuteAddArtwork);
            DeleteArtworkCommand = new RelayCommand(ExecuteDeleteArtwork, CanDeleteArtwork);
            EditArtworkCommand = new RelayCommand(ExecuteEditArtwork, CanEditArtwork);
        }

        private void ExecuteAddArtwork(object? parameter)
        {
            var newArtwork = new Artwork { Id = Guid.NewGuid() };
            var editViewModel = new ArtworkEditViewModel(newArtwork, _artworkValidator);

            var dialogResult = _dialogService.ShowDialog(editViewModel);

            if (dialogResult == true)
            {
                var createdArtwork = editViewModel.ToModel();
                var command = new AddArtworkCommand(Artworks, createdArtwork);
                _commandHistory.ExecuteCommand(command);
            }
        }

        private void ExecuteDeleteArtwork(object? parameter)
        {
            if (SelectedArtwork is null)
            {
                return;
            }

            var command = new DeleteArtworkCommand(Artworks, SelectedArtwork);
            _commandHistory.ExecuteCommand(command);
            SelectedArtwork = null;
        }

        private bool CanDeleteArtwork(object? parameter) => SelectedArtwork is not null;

        private void ExecuteEditArtwork(object? parameter)
        {
            if (SelectedArtwork is null)
            {
                return;
            }

            var originalSnapshot = CloneArtwork(SelectedArtwork);
            var editableModel = CloneArtwork(SelectedArtwork);
            var editViewModel = new ArtworkEditViewModel(editableModel, _artworkValidator);

            var dialogResult = _dialogService.ShowDialog(editViewModel);

            if (dialogResult == true)
            {
                var updatedSnapshot = editViewModel.ToModel();
                var command = new EditArtworkCommand(Artworks, originalSnapshot, updatedSnapshot);
                _commandHistory.ExecuteCommand(command);
            }
        }

        private bool CanEditArtwork(object? parameter) => SelectedArtwork is not null;

        private static Artwork CloneArtwork(Artwork source) => new()
        {
            Id = source.Id,
            Title = source.Title,
            Artist = source.Artist,
            Medium = source.Medium,
            YearCreated = source.YearCreated,
            Style = source.Style
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
            (DeleteArtworkCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (EditArtworkCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
