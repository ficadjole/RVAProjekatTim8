using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RVAProjekatTim8.Commands
{
    public class AddArtworkMonitoringCommand : IUndoableCommand
    {
        private readonly ObservableCollection<ArtworkMonitoring> _targetCollection;
        private readonly ReadOnlyObservableCollection<Artwork> _availableArtworks;
        private readonly ArtworkMonitoring _monitoringToAdd;

        public string Desciption { get; set; }

        public AddArtworkMonitoringCommand(
            ObservableCollection<ArtworkMonitoring> targetCollection,
            ReadOnlyObservableCollection<Artwork> availableArtworks,
            ArtworkMonitoring monitoringToAdd)
        {
            _targetCollection = targetCollection;
            _availableArtworks = availableArtworks;
            _monitoringToAdd = monitoringToAdd;

            Desciption = $"Dodato merenje za delo Id={_monitoringToAdd.ArtworkId} ({_monitoringToAdd.MonitoringTime:dd.MM.yyyy HH:mm})";
        }

        public void Execute()
        {
            var referencedArtworkExists = _availableArtworks
                .Any(artwork => artwork.Id == _monitoringToAdd.ArtworkId);

            if (!referencedArtworkExists)
            {
                throw new InvalidOperationException(
                    $"Merenje se ne može dodati: umetničko delo sa Id={_monitoringToAdd.ArtworkId} ne postoji.");
            }

            _targetCollection.Add(_monitoringToAdd);
        }

        public void Undo()
        {
            _targetCollection.Remove(_monitoringToAdd);
        }

        public string GetDescription()
        {
            return Desciption;
        }
    }
}
