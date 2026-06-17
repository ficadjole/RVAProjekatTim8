using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;
using System.Collections.ObjectModel;

namespace RVAProjekatTim8.Commands
{
    public class AddArtworkCommand : IUndoableCommand
    {
        private readonly ObservableCollection<Artwork> _targetCollection;
        private readonly Artwork _artworkToAdd;
        public string Description { get; private set; }
        public AddArtworkCommand(ObservableCollection<Artwork> targetCollection, Artwork artworkToAdd)
        {
            _targetCollection = targetCollection;
            _artworkToAdd = artworkToAdd;
            Description = $"Dodato umetničko delo: \"{_artworkToAdd.Title}\" ({_artworkToAdd.Artist})";
        }

        public void Execute()
        {
            _targetCollection.Add(_artworkToAdd);
        }

        public void Undo()
        {
            _targetCollection.Remove(_artworkToAdd);
        }

        public string GetDescription()
        {
            return Description;
        }
    }
}
