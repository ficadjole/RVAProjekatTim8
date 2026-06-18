using RVAProjekatTim8.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVAProjekatTim8.Commands
{
    public class EditArtworkMonitoringCommand : IUndoableCommand
    {
        private readonly IList<ArtworkMonitoring> _targetCollection;
        private readonly IReadOnlyList<Artwork> _availableArtworks;
        private readonly ArtworkMonitoring _originalSnapshot;
        private readonly ArtworkMonitoring _updatedSnapshot;
        public string Description { get; set; }
        public EditArtworkMonitoringCommand(
            IList<ArtworkMonitoring> targetCollection,
            IReadOnlyList<Artwork> availableArtworks,
            ArtworkMonitoring originalSnapshot,
            ArtworkMonitoring updatedSnapshot)
        {
            _targetCollection = targetCollection;
            _availableArtworks = availableArtworks;
            _originalSnapshot = originalSnapshot;
            _updatedSnapshot = updatedSnapshot;

            Description = $"Izmenjeno merenje Id={_originalSnapshot.Id}: stanje {_originalSnapshot.State} → {_updatedSnapshot.State}";
        }

        public void Execute()
        {
            ValidateForeignKey(_updatedSnapshot.ArtworkId);
            ReplaceInCollection(_updatedSnapshot);
        }

        public void Undo()
        {
            // Undo vraća na originalno stanje koje je po definiciji već bilo
            // validno (postojalo je u kolekciji pre izmene) — FK provera nije
            // potrebna ovde, jer ne uvodimo novu referencu, samo se vraćamo.
            ReplaceInCollection(_originalSnapshot);
        }

        private void ValidateForeignKey(Guid artworkId)
        {
            var referencedArtworkExists = _availableArtworks.Any(artwork => artwork.Id == artworkId);

            if (!referencedArtworkExists)
            {
                throw new InvalidOperationException(
                    $"Merenje se ne može izmeniti: umetničko delo sa Id={artworkId} ne postoji.");
            }
        }

        private void ReplaceInCollection(ArtworkMonitoring snapshotToApply)
        {
            var index = IndexOfById(_originalSnapshot.Id);

            if (index == -1)
            {
                throw new InvalidOperationException(
                    $"Merenje sa Id={_originalSnapshot.Id} ne postoji u kolekciji — verovatno je u međuvremenu obrisano.");
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
