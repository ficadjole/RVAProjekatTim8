using RVAProjekatTim8.Enums;
using RVAProjekatTim8.Interfaces;

namespace RVAProjekatTim8.Services
{
    public class ActivityLoggingService
    {
        private readonly ILoggingService _loggingService;

        public ActivityLoggingService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        /// <summary>
        /// Pretplaćuje logger na dati CommandHistory. Pozvati jednom,
        /// po pravilu prilikom kompozicije objekata (npr. u App.xaml.cs
        /// ili kompozicionom korenu DI kontejnera).
        /// </summary>
        public void SubscribeTo(CommandHistory commandHistory)
        {
            commandHistory.CommandStateChanged += OnCommandStateChanged;
        }

        private void OnCommandStateChanged(object? sender, CommandHistoryChangedEventArgs e)
        {
            var actionLabel = e.Action switch
            {
                CommandAction.Executed => "Izvršeno",
                CommandAction.Undone => "Poništeno",
                CommandAction.Redone => "Ponovljeno",
                _ => "Nepoznata akcija"
            };

            _loggingService.LogActivity($"{actionLabel}: {e.Command.GetDescription()}");
        }
    }
}
