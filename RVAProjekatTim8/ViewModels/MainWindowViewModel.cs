using RVAProjekatTim8.Helpers;
using RVAProjekatTim8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RVAProjekatTim8.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly CommandHistory _commandHistory;

        public ArtworkListViewModel ArtworkListViewModel { get; }
        public ArtworkMonitoringListViewModel ArtworkMonitoringListViewModel { get; }

        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public MainWindowViewModel(
            CommandHistory commandHistory,
            ArtworkListViewModel artworkListViewModel,
            ArtworkMonitoringListViewModel artworkMonitoringListViewModel)
        {
            _commandHistory = commandHistory;
            ArtworkListViewModel = artworkListViewModel;
            ArtworkMonitoringListViewModel = artworkMonitoringListViewModel;

            UndoCommand = new RelayCommand(_ => _commandHistory.Undo(), _ => _commandHistory.CanUndo);
            RedoCommand = new RelayCommand(_ => _commandHistory.Redo(), _ => _commandHistory.CanRedo);

            _commandHistory.HistoryChanged += (_, _) => RaiseUndoRedoCanExecuteChanged();
        }

        private void RaiseUndoRedoCanExecuteChanged()
        {
            (UndoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
