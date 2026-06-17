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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // --- 1. Persistencija i repozitorijumi (redosled je bitan:
            //        Artwork pre ArtworkMonitoring, zbog FK zavisnosti) ---

            var artworkRepository = new ArtworkRepository(
                new JsonPersistenceService<Artwork>("Data/artworks.json"),
                new ArtworkInitialDataProvider());
            artworkRepository.Load();

            var monitoringRepository = new ArtworkMonitoringRepository(
                new JsonPersistenceService<ArtworkMonitoring>("Data/monitorings.json"),
                () => new ArtworkMonitoringInitialDataProvider(artworkRepository.Artworks));
            monitoringRepository.Load();

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
                monitoringRepository,
                new ReadOnlyObservableCollection<Artwork>(artworkRepository.Artworks));

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
            // Mesto za eksplicitan Save() poziv na repozitorijume — vidi
            // otvoreno pitanje iz prethodnog koraka o auto-save vs eksplicitno.
            base.OnExit(e);
        }
    }
}

