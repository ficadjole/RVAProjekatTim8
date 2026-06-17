using RVAProjekatTim8.Interfaces;
using RVAProjekatTim8.Model;
using System.Collections.Generic;

namespace RVAProjekatTim8.Services
{
    public class ArtworkInitialDataProvider : IInitialDataProvider<Artwork>
    {
        public List<Artwork> GetInitialData()
        {
            return new List<Artwork>
        {
            new()
            {
                Title = "Starry Night",
                Artist = "Vincent van Gogh",
                Medium = "Ulje na platnu",
                YearCreated = 1889,
                Style = "Postimpresionizam"
            },
            new()
            {
                Title = "Guernica",
                Artist = "Pablo Picasso",
                Medium = "Ulje na platnu",
                YearCreated = 1937,
                Style = "Kubizam"
            },
            new()
            {
                Title = "Mona Lisa",
                Artist = "Leonardo da Vinci",
                Medium = "Ulje na drvenoj tabli",
                YearCreated = 1503,
                Style = "Renesansa"
            }
        };
        }
    }
}
