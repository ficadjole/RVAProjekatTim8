using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVAProjekatTim8.Commands
{
    public class EditArtworkCommand : IUndoableCommand
    {
        private readonly IList<Artwork> _targetCollection;
        private readonly Artwork _originalSnapshot;
        private readonly Artwork _updatedSnapshot;

        public string Description { get; set; }

        /// <param name="targetCollection">Kolekcija u kojoj se nalazi instanca.</param>
        /// <param name="originalSnapshot">Stanje PRE izmene (snimljeno u trenutku otvaranja forme za edit).</param>
        /// <param name="updatedSnapshot">Stanje POSLE izmene (iz ArtworkEditViewModel.ToModel()).</param>
        public EditArtworkCommand(IList<Artwork> targetCollection, Artwork originalSnapshot, Artwork updatedSnapshot)
        {
            _targetCollection = targetCollection;
            _originalSnapshot = originalSnapshot;
            _updatedSnapshot = updatedSnapshot;

            Description = $"Izmenjeno umetničko delo: \"{_originalSnapshot.Title}\" → \"{_updatedSnapshot.Title}\"";
        }

        public void Execute() => ReplaceInCollection(_updatedSnapshot);

        public void Undo() => ReplaceInCollection(_originalSnapshot);

        private void ReplaceInCollection(Artwork snapshotToApply)
        {
            var index = IndexOfById(_originalSnapshot.Id);

            if (index == -1)
            {
                throw new InvalidOperationException(
                    $"Umetničko delo sa Id={_originalSnapshot.Id} ne postoji u kolekciji — verovatno je u međuvremenu obrisano.");
            }

            _targetCollection[index] = snapshotToApply;
        }

        private int IndexOfById(Guid id)
        {
            for (var i = 0; i < _targetCollection.Count; i++)
            {
                if (_targetCollection[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public string GetDescription()
        {
            return Description;
        }

            
    }
}
