using RVAProjekatTim8.Helpers;
using RVAProjekatTim8.Interfaces;
using Common.Models;
using RVAProjekatTim8.Validators;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RVAProjekatTim8.ViewModels
{
    public class ArtworkEditViewModel : INotifyPropertyChanged, IDataErrorInfo, ICloseable
    {
        private readonly IValidator<Artwork> _validator;
        private readonly Artwork _artwork;

        private string _title;
        private string _artist;
        private string _medium;
        private int _yearCreated;
        private string _style;

        public event EventHandler<bool?>? CloseRequested;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ArtworkEditViewModel(Artwork artwork, IValidator<Artwork> validator)
        {
            _artwork = artwork;
            _validator = validator;

            _title = artwork.Title;
            _artist = artwork.Artist;
            _medium = artwork.Medium;
            _yearCreated = artwork.YearCreated;
            _style = artwork.Style;

            SaveCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, true), _ => IsValid);
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, false));
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        public string Medium
        {
            get => _medium;
            set => SetProperty(ref _medium, value);
        }

        public int YearCreated
        {
            get => _yearCreated;
            set => SetProperty(ref _yearCreated, value);
        }

        public string Style
        {
            get => _style;
            set => SetProperty(ref _style, value);
        }

        /// <summary>
        /// True ako su sva polja trenutno validna. Bind-uje se na IsEnabled
        /// dugmeta "Sačuvaj" da bi se onemogućilo čuvanje neispravnih podataka.
        /// </summary>
        public bool IsValid => string.IsNullOrEmpty(((IDataErrorInfo)this)["Title"])
                                && string.IsNullOrEmpty(((IDataErrorInfo)this)["Artist"])
                                && string.IsNullOrEmpty(((IDataErrorInfo)this)["Medium"])
                                && string.IsNullOrEmpty(((IDataErrorInfo)this)["YearCreated"])
                                && string.IsNullOrEmpty(((IDataErrorInfo)this)["Style"]);

        /// <summary>
        /// Prenosi trenutne vrednosti iz ViewModel-a u Model. Pozvati samo
        /// nakon što je IsValid potvrđeno kao true, neposredno pre čuvanja.
        /// </summary>
        public Artwork ToModel()
        {
            _artwork.Title = Title;
            _artwork.Artist = Artist;
            _artwork.Medium = Medium;
            _artwork.YearCreated = YearCreated;
            _artwork.Style = Style;
            return _artwork;
        }

        // --- IDataErrorInfo implementacija ---

        // Error nije korišćen u WPF binding mehanizmu (rezerviran za scenarije
        // validacije na nivou cele instance), ali interfejs ga zahteva.
        string IDataErrorInfo.Error => string.Empty;

        /// <summary>
        /// WPF poziva ovaj indeksator nakon svake promene bindovanog svojstva.
        /// Validacija se delegira na _validator preko privremenog snapshot-a
        /// trenutnih (još nepotvrđenih) vrednosti iz ViewModel-a.
        /// </summary>
        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                var snapshot = new Artwork
                {
                    Title = Title,
                    Artist = Artist,
                    Medium = Medium,
                    YearCreated = YearCreated,
                    Style = Style
                };

                return _validator.ValidateProperty(snapshot, propertyName);
            }
        }

        // --- INotifyPropertyChanged implementacija ---

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
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
