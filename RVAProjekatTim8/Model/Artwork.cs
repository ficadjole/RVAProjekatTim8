using System;

namespace RVAProjekatTim8.Model
{
    public class Artwork
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Medium { get; set; } = string.Empty;
        public int YearCreated { get; set; }
        public string Style { get; set; } = string.Empty;
    }
}
