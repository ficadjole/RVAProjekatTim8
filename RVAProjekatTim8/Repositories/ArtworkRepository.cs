using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;

using System.Collections.ObjectModel;


namespace RVAProjekatTim8.Repositories
{
    public class ArtworkRepository
    {
        private readonly IPersistenceService<Artwork> _persistenceService;
        private readonly IInitialDataProvider<Artwork> _initialDataProvider;

        public ObservableCollection<Artwork> Artworks { get; private set; } = new();

        public ArtworkRepository(
            IPersistenceService<Artwork> persistenceService,
            IInitialDataProvider<Artwork> initialDataProvider)
        {
            _persistenceService = persistenceService;
            _initialDataProvider = initialDataProvider;
        }

        /// <summary>
        /// Učitava podatke iz datoteke. Ako je datoteka prazna ili ne postoji,
        /// puni kolekciju default instancama i odmah ih upisuje na disk —
        /// tako da svaki sledeći Load (npr. nakon ručnog brisanja fajla) ima
        /// stabilan, ponovljiv rezultat.
        /// </summary>
        public void Load()
        {
            var loadedItems = _persistenceService.Load();

            if (loadedItems.Count == 0)
            {
                loadedItems = _initialDataProvider.GetInitialData();
                _persistenceService.Save(loadedItems);
            }

            Artworks = new ObservableCollection<Artwork>(loadedItems);
        }

        public void Save()
        {
            _persistenceService.Save(Artworks);
        }
    }
}
