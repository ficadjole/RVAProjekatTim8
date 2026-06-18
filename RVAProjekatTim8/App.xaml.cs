using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;
using RVAProjekatTim8.Repositories;
using RVAProjekatTim8.Services;
using RVAProjekatTim8.Validators;
using RVAProjekatTim8.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace RVAProjekatTim8
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ArtworkRepository artworkRepository;
        private ArtworkMonitoringRepository artworkMonitoringRepository;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // --- 1. Persistencija i repozitorijumi (redosled je bitan:
            //        Artwork pre ArtworkMonitoring, zbog FK zavisnosti) ---

            artworkRepository = new ArtworkRepository(
                new JsonPersistenceService<Artwork>("Data/artworks.json"),
                new ArtworkInitialDataProvider());
            artworkRepository.Load();

            artworkMonitoringRepository = new ArtworkMonitoringRepository(
                new JsonPersistenceService<ArtworkMonitoring>("Data/monitorings.json"),
                () => new ArtworkMonitoringInitialDataProvider(artworkRepository.Artworks));
            artworkMonitoringRepository.Load();

            // --- 2. Validatori (Strategy implementacije) ---

            IValidator<Artwork> artworkValidator = new ArtworkValidator();
            IValidator<ArtworkMonitoring> monitoringValidator = new ArtworkMonitoringValidator();

            // --- 3. Globalna CommandHistory + Observer logging ---

            var commandHistory = new CommandHistory();
            var loggingService = new FileLoggingService("Logs/activity-log.txt");
            var activityLogger = new ActivityLoggingService(loggingService);
            activityLogger.SubscribeTo(commandHistory);

            // --- 4. Dialog servis ---

            IDialogService dialogService = new DialogService();

            // --- 5. Pod-ViewModeli ---

            var artworkListViewModel = new ArtworkListViewModel(
                commandHistory,
                dialogService,
                artworkValidator,
                artworkRepository);

            var artworkMonitoringListViewModel = new ArtworkMonitoringListViewModel(
                commandHistory,
                artworkMonitoringRepository,
                new ReadOnlyObservableCollection<Artwork>(artworkRepository.Artworks),
                dialogService,
                monitoringValidator);

            // --- 6. Glavni ViewModel i prozor ---

            var mainWindowViewModel = new MainWindowViewModel(
                commandHistory,
                artworkListViewModel,
                artworkMonitoringListViewModel);

            var mainWindow = new MainWindow(mainWindowViewModel);
            MainWindow = mainWindow;
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            artworkRepository?.Save();
            artworkMonitoringRepository?.Save();
            base.OnExit(e);
        }
    }
}

