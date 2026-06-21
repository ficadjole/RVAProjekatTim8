using RVAProjekatTim8.Interfaces;
using Common.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace RVAProjekatTim8.Commands
{
    public class DeleteArtworkCommand : IUndoableCommand
    {
        private readonly ObservableCollection<Artwork> _targetCollection;
        private readonly ObservableCollection<ArtworkMonitoring> _monitorings;
        private readonly Artwork _artworkToDelete;
        private int _originalIndex;
        private Dictionary<ArtworkMonitoring, int> _deletedMonitorings = new Dictionary<ArtworkMonitoring, int>();
        public string Description { get; private set; }
        public DeleteArtworkCommand(ObservableCollection<Artwork> targetCollection,ObservableCollection<ArtworkMonitoring> monitorings,  Artwork artworkToDelete)
        {
            _targetCollection = targetCollection;
            _monitorings = monitorings;
            _artworkToDelete = artworkToDelete;
            Description = $"Obrisano umetničko delo: \"{_artworkToDelete.Title}\" ({_artworkToDelete.Artist})";
        }

        public void Execute()
        {
            _originalIndex = _targetCollection.IndexOf(_artworkToDelete);
            _deletedMonitorings = _monitorings.Select((m, i) => (Monitoring: m, Index: i))
                                              .Where(x => x.Monitoring.ArtworkId == _artworkToDelete.Id)
                                              .ToDictionary(x => x.Monitoring, x => x.Index);


            foreach (var kvp in _deletedMonitorings.OrderByDescending(x => x.Value))
            {
                _monitorings.RemoveAt(kvp.Value);
            }

            _targetCollection.Remove(_artworkToDelete);
        }

        public void Undo()
        {
            // Ako je originalni indeks i dalje validan, vraćamo na tačnu poziciju.
            // Inače (npr. lista je u međuvremenu skraćena drugim operacijama),
            // dodajemo na kraj kao bezbedan fallback.
            if (_originalIndex >= 0 && _originalIndex <= _targetCollection.Count)
                _targetCollection.Insert(_originalIndex, _artworkToDelete);
            else
                _targetCollection.Add(_artworkToDelete);

            foreach (var kvp in _deletedMonitorings.OrderBy(x => x.Value))
            {
                if (kvp.Value >= 0 && kvp.Value <= _monitorings.Count)
                    _monitorings.Insert(kvp.Value, kvp.Key);
                else
                    _monitorings.Add(kvp.Key);
            }
        }

        public string GetDescription()
        {
            return Description;
        }
    }

}
