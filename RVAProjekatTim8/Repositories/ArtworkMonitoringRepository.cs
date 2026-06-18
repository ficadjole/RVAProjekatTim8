using Common.Interfaces;
using RVAProjekatTim8.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVAProjekatTim8.Repositories
{
    public class ArtworkMonitoringRepository
    {
        private readonly IPersistenceService<ArtworkMonitoring> _persistenceService;
        private readonly Func<IInitialDataProvider<ArtworkMonitoring>> _initialDataProviderFactory;

        public ObservableCollection<ArtworkMonitoring> Monitorings { get; private set; } = new();

        /// <summary>
        /// Prima Func (fabrička metoda) umesto direktne IInitialDataProvider
        /// zavisnosti, jer ArtworkMonitoringInitialDataProvider zahteva listu
        /// VEĆ UČITANIH Artwork instanci u svom konstruktoru — ta lista ne
        /// postoji u trenutku kompozicije objekata (DI), samo u trenutku
        /// Load() poziva, nakon što je ArtworkRepository.Load() već izvršen.
        /// Func odlaže kreiranje providera do trenutka kada su podaci dostupni.
        /// </summary>
        public ArtworkMonitoringRepository(
            IPersistenceService<ArtworkMonitoring> persistenceService,
            Func<IInitialDataProvider<ArtworkMonitoring>> initialDataProviderFactory)
        {
            _persistenceService = persistenceService;
            _initialDataProviderFactory = initialDataProviderFactory;
        }

        public void Load()
        {
            var loadedItems = _persistenceService.Load();

            if (loadedItems.Count == 0)
            {
                var initialDataProvider = _initialDataProviderFactory();
                loadedItems = initialDataProvider.GetInitialData();
                _persistenceService.Save(loadedItems);
            }

            Monitorings = new ObservableCollection<ArtworkMonitoring>(loadedItems);
        }

        public void Save()
        {
            _persistenceService.Save(Monitorings);
        }
    }
}
