using Common.Models;
using Statistics.Clients;
using Statistics.Helpers;
using Statistics.Interfaces;
using Statistics.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Statistics.ViewModels
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        private readonly ArtworkServiceClient _serviceClient;

        private Dictionary<string, List<ArtworkMonitoring>> _monitoringsByKey
            = new Dictionary<string, List<ArtworkMonitoring>>();

        private IStatisticsStrategy _selectedStrategy;
        public IStatisticsStrategy SelectedStrategy
        {
            get => _selectedStrategy;
            set
            {
                _selectedStrategy = value;
                OnPropertyChanged(nameof(SelectedStrategy));
                CalculateStatistics(); 
            }
        }

        private List<Artwork> _artworks = new List<Artwork>();
        public List<Artwork> Artworks
        {
            get => _artworks;
            set { _artworks = value; OnPropertyChanged(nameof(Artworks)); }
        }

        private Artwork _selectedArtwork;
        public Artwork SelectedArtwork
        {
            get => _selectedArtwork;
            set { 
                _selectedArtwork = value; 
            
                OnPropertyChanged(nameof(SelectedArtwork));
                StatisticsResult = string.Empty;
                SelectedStrategy = null;

                MonitoringDisplay.Clear();
            }
        }

        private int _selectedMonth = DateTime.Now.Month;
        public int SelectedMonth
        {
            get => _selectedMonth;
            set { _selectedMonth = value; OnPropertyChanged(nameof(SelectedMonth)); }
        }

        private int _selectedYear = DateTime.Now.Year;
        public int SelectedYear
        {
            get => _selectedYear;
            set { _selectedYear = value; OnPropertyChanged(nameof(SelectedYear)); }
        }

        public List<int> Months { get; } = Enumerable.Range(1, 12).ToList();
        public List<int> Years { get; } = Enumerable.Range(2020, 10).ToList();

        private ObservableCollection<string> _monitoringDisplay = new ObservableCollection<string>();
        public ObservableCollection<string> MonitoringDisplay
        {
            get => _monitoringDisplay;
            set { _monitoringDisplay = value; OnPropertyChanged(nameof(MonitoringDisplay)); }
        }

        public ICommand RefreshArtworksCommand { get; }

        public List<IStatisticsStrategy> StatisticsMethods { get; } = new List<IStatisticsStrategy>
{
            new AverageLightStrategy(),
            new MaxAirPollutionStrategy(),
            new CriticalStateStrategy()
        };

        private string _statisticsResult;
        public string StatisticsResult
        {
            get => _statisticsResult;
            set { _statisticsResult = value; OnPropertyChanged(nameof(StatisticsResult)); }
        }

        public ICommand FetchDataCommand { get; }
        public ICommand ExportCsvCommand { get; }

        public StatisticsViewModel()
        {
            _serviceClient = new ArtworkServiceClient();
            FetchDataCommand = new RelayCommand(_ => FetchData());
            ExportCsvCommand = new RelayCommand(_ => ExportToCsv());
            RefreshArtworksCommand = new RelayCommand(_ => RefreshArtworks());
            LoadArtworks();
        }

        private void LoadArtworks()
        {
            try
            {
                Artworks = _serviceClient.GetAllArtworks();
            }
            catch
            {
                Artworks = new List<Artwork>();
            }
        }

        private void FetchData()
        {
            if (SelectedArtwork == null) return;

            var monitorings = _serviceClient.GetMonitoringsByArtworkAndMonth(
                SelectedArtwork.Id, SelectedMonth, SelectedYear);

            if (monitorings == null || monitorings.Count == 0)
            {
                StatisticsResult = $"Nema pronađenih merenja za delo \"{SelectedArtwork.Title}\" u izabranom periodu.";
                return;
            }

            var key = $"{SelectedArtwork.Id}-{SelectedMonth:D2}{SelectedYear}";
            _monitoringsByKey[key] = monitorings;

            UpdateDisplay();
            CalculateStatistics();
        }

        private void UpdateDisplay()
        {
            MonitoringDisplay.Clear();

            foreach (var kvp in _monitoringsByKey)
            {
                int dashIdx = kvp.Key.LastIndexOf('-');
                string artworkId = kvp.Key.Substring(0, dashIdx);
                string monthYear = kvp.Key.Substring(dashIdx + 1); 
                string period = $"{monthYear.Substring(0, 2)}/{monthYear.Substring(2)}"; 

                var entries = string.Join(", ", kvp.Value.Select(m =>
                    $"{{{m.Id}, {m.LightExposure}, {m.AirPollution}}}"));

                MonitoringDisplay.Add($"[{artworkId}, {period}]: {entries}");
            }
        }

        private void CalculateStatistics()
        {
            if (SelectedArtwork == null || SelectedStrategy == null) return;

            var key = $"{SelectedArtwork.Id}-{SelectedMonth:D2}{SelectedYear}";

            if (!_monitoringsByKey.ContainsKey(key) || _monitoringsByKey[key].Count == 0)
            {
                StatisticsResult = "Statistika ne može biti izračunata jer nema merenja za izabrani period.";
                return;
            }

            List<ArtworkMonitoring> trenutnaMerenja = _monitoringsByKey[key];

            string rezultatStrategije = SelectedStrategy.Calculate(trenutnaMerenja);

            StatisticsResult = $"Metoda: {SelectedStrategy.GetName()}\nRezultat: {rezultatStrategije}";
        }

        private void ExportToCsv()
        {
            if (_monitoringsByKey.Count == 0) return;

            var lines = new List<string> { "Key,MonitoringId,LightExposure,AirPollution,State" };

            foreach (var kvp in _monitoringsByKey)
            {
                foreach (var m in kvp.Value)
                {
                    lines.Add($"{kvp.Key},{m.Id},{m.LightExposure},{m.AirPollution},{m.State}");
                }
            }

            File.WriteAllLines("statistics_export.csv", lines);
            StatisticsResult = "Exportovano u statistics_export.csv!";
        }

        private void RefreshArtworks()
        {
            Guid? prethodniId = SelectedArtwork?.Id;

            LoadArtworks();

            if (prethodniId.HasValue)
            {
                SelectedArtwork = Artworks.FirstOrDefault(a => a.Id == prethodniId.Value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}