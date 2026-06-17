using RVAProjekatTim8.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RVAProjekatTim8.Services
{
    public class JsonPersistenceService<T> : IPersistenceService<T>
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _serializerOptions;

        public JsonPersistenceService(string filePath)
        {
            _filePath = filePath;
            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };

            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public List<T> Load()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            var json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>();
            }

            var items = JsonSerializer.Deserialize<List<T>>(json, _serializerOptions);
            return items ?? new List<T>();
        }

        public void Save(IEnumerable<T> items)
        {
            var json = JsonSerializer.Serialize(items, _serializerOptions);
            File.WriteAllText(_filePath, json);
        }
    }

}
