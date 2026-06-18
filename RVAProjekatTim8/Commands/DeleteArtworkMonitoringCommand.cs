using RVAProjekatTim8.Interfaces;
using Common.Models;
using System.Collections.ObjectModel;

namespace RVAProjekatTim8.Commands
{
    public class DeleteArtworkMonitoringCommand : IUndoableCommand
    {
        private readonly ObservableCollection<ArtworkMonitoring> _targetCollection;
        private readonly ArtworkMonitoring _monitoringToDelete;
        private int _originalIndex;

        public string Decription { get; set; }

        public DeleteArtworkMonitoringCommand(
            ObservableCollection<ArtworkMonitoring> targetCollection,
            ArtworkMonitoring monitoringToDelete)
        {
            _targetCollection = targetCollection;
            _monitoringToDelete = monitoringToDelete;

            Decription = $"Obrisano merenje za delo Id={_monitoringToDelete.ArtworkId} ({_monitoringToDelete.MonitoringTime:dd.MM.yyyy HH:mm})";
        }

        public void Execute()
        {
            _originalIndex = _targetCollection.IndexOf(_monitoringToDelete);
            _targetCollection.Remove(_monitoringToDelete);
        }

        public void Undo()
        {
            if (_originalIndex >= 0 && _originalIndex <= _targetCollection.Count)
            {
                _targetCollection.Insert(_originalIndex, _monitoringToDelete);
            }
            else
            {
                _targetCollection.Add(_monitoringToDelete);
            }
        }

        public string GetDescription()
        {
            return Decription;
        }


    }
}
