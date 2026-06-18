using Common.Models;
using RVAProjekatTim8.Validators;
using System;
using System.Collections.Generic;

namespace RVAProjekatTim8.Services
{
    public class ArtworkMonitoringValidator : IValidator<ArtworkMonitoring>
    {
        public string ValidateProperty(ArtworkMonitoring instance, string propertyName)
        {
            return propertyName switch
            {
                nameof(ArtworkMonitoring.ArtworkId) => ValidateArtworkId(instance.ArtworkId),
                nameof(ArtworkMonitoring.MonitoringTime) => ValidateMonitoringTime(instance.MonitoringTime),
                nameof(ArtworkMonitoring.LightExposure) => ValidateLightExposure(instance.LightExposure),
                nameof(ArtworkMonitoring.AirPollution) => ValidateAirPollution(instance.AirPollution),
                _ => string.Empty
            };
        }

        public IReadOnlyDictionary<string, string> ValidateAll(ArtworkMonitoring instance)
        {
            var errors = new Dictionary<string, string>();
            string[] propertiesToValidate =
            {
            nameof(ArtworkMonitoring.ArtworkId),
            nameof(ArtworkMonitoring.MonitoringTime),
            nameof(ArtworkMonitoring.LightExposure),
            nameof(ArtworkMonitoring.AirPollution)
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

        private static string ValidateArtworkId(Guid artworkId)
        {
            if (artworkId == Guid.Empty)
            {
                return "Merenje mora biti vezano za postojeće umetničko delo.";
            }

            return string.Empty;
        }

        private static string ValidateMonitoringTime(DateTime monitoringTime)
        {
            if (monitoringTime > DateTime.Now)
            {
                return "Vreme merenja ne može biti u budućnosti.";
            }

            return string.Empty;
        }

        private static string ValidateLightExposure(double lightExposure)
        {
            // Realan opseg osvetljenja u muzejskim prostorima (u luksima).
            if (lightExposure < 0 || lightExposure > 5000)
            {
                return "Izloženost svetlosti mora biti između 0 i 5000 lux.";
            }

            return string.Empty;
        }

        private static string ValidateAirPollution(double airPollution)
        {
            if (airPollution < 0 || airPollution > 1000)
            {
                return "Zagađenost vazduha mora biti između 0 i 1000 μg/m³.";
            }

            return string.Empty;
        }
    }
}
