using RVAProjekatTim8.Model;
using RVAProjekatTim8.Validators;
using System;
using System.Collections.Generic;

namespace RVAProjekatTim8.Services
{
    public class ArtworkValidator : IValidator<Artwork>
    {
        private const int MinYear = 1000;

        public string ValidateProperty(Artwork instance, string propertyName)
        {
            return propertyName switch
            {
                nameof(Artwork.Title) => ValidateTitle(instance.Title),
                nameof(Artwork.Artist) => ValidateArtist(instance.Artist),
                nameof(Artwork.Medium) => ValidateMedium(instance.Medium),
                nameof(Artwork.YearCreated) => ValidateYearCreated(instance.YearCreated),
                nameof(Artwork.Style) => ValidateStyle(instance.Style),
                _ => string.Empty
            };
        }

        public IReadOnlyDictionary<string, string> ValidateAll(Artwork instance)
        {
            var errors = new Dictionary<string, string>();
            string[] propertiesToValidate =
            {
            nameof(Artwork.Title),
            nameof(Artwork.Artist),
            nameof(Artwork.Medium),
            nameof(Artwork.YearCreated),
            nameof(Artwork.Style)
        };

            foreach (var propertyName in propertiesToValidate)
            {
                var error = ValidateProperty(instance, propertyName);
                if (!string.IsNullOrEmpty(error))
                {
                    errors[propertyName] = error;
                }
            }

            return errors;
        }

        private static string ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return "Naziv dela je obavezan.";
            }

            if (title.Length > 200)
            {
                return "Naziv dela ne može biti duži od 200 karaktera.";
            }

            return string.Empty;
        }

        private static string ValidateArtist(string artist)
        {
            if (string.IsNullOrWhiteSpace(artist))
            {
                return "Autor dela je obavezan.";
            }

            if (artist.Length > 150)
            {
                return "Ime autora ne može biti duže od 150 karaktera.";
            }

            return string.Empty;
        }

        private static string ValidateMedium(string medium)
        {
            if (string.IsNullOrWhiteSpace(medium))
            {
                return "Tehnika izrade je obavezna.";
            }

            return string.Empty;
        }

        private static string ValidateYearCreated(int yearCreated)
        {
            var currentYear = DateTime.Now.Year;

            if (yearCreated < MinYear || yearCreated > currentYear)
            {
                return $"Godina nastanka mora biti između {MinYear} i {currentYear}.";
            }

            return string.Empty;
        }

        private static string ValidateStyle(string style)
        {
            if (string.IsNullOrWhiteSpace(style))
            {
                return "Umetnički stil je obavezan.";
            }

            return string.Empty;
        }
    }
}
