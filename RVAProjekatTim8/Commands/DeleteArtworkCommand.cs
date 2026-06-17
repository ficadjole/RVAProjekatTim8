using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;
using System.Collections.ObjectModel;

namespace RVAProjekatTim8.Commands
{
    public class DeleteArtworkCommand : IUndoableCommand
    {
        private readonly ObservableCollection<Artwork> _targetCollection;
        private readonly Artwork _artworkToDelete;
        private int _originalIndex;
        public string Description { get; private set; }
        public DeleteArtworkCommand(ObservableCollection<Artwork> targetCollection, Artwork artworkToDelete)
        {
            _targetCollection = targetCollection;
            _artworkToDelete = artworkToDelete;
            Description = $"Obrisano umetničko delo: \"{_artworkToDelete.Title}\" ({_artworkToDelete.Artist})";
        }

        public void Execute()
        {
            _originalIndex = _targetCollection.IndexOf(_artworkToDelete);
            _targetCollection.Remove(_artworkToDelete);
        }

        public void Undo()
        {
            // Ako je originalni indeks i dalje validan, vraćamo na tačnu poziciju.
            // Inače (npr. lista je u međuvremenu skraćena drugim operacijama),
            // dodajemo na kraj kao bezbedan fallback.
            if (_originalIndex >= 0 && _originalIndex <= _targetCollection.Count)
            {
                _targetCollection.Insert(_originalIndex, _artworkToDelete);
            }
            else
            {
                _targetCollection.Add(_artworkToDelete);
            }
        }

        public string GetDescription()
        {
            return Description;
        }
    }

}
