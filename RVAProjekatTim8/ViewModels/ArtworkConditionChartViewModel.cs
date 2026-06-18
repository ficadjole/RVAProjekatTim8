using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Common.Enums;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace RVAProjekatTim8.ViewModels
{
    /// <summary>
    /// Prati Monitorings kolekciju (i strukturne promene i promene State
    /// svojstva na pojedinačnim instancama) i u realnom vremenu ažurira
    /// brojeve po stanju za LiveCharts2 Pie chart.
    /// </summary>

    public class ArtworkConditionChartViewModel
    {
        private readonly ObservableCollection<ArtworkMonitoring> _monitorings;
        private readonly Dictionary<ArtworkCondition, ObservableValue> _counters = new Dictionary<ArtworkCondition, ObservableValue>();

        public ISeries[] Series { get;  }

        public ArtworkConditionChartViewModel(ObservableCollection<ArtworkMonitoring> monitorings)
        {
            _monitorings = monitorings;

            foreach(ArtworkCondition condition in Enum.GetValues(typeof(ArtworkCondition)))
            {
                _counters[condition] = new ObservableValue(0);
            }

            Series = _counters
            .Select(kvp => (ISeries)new PieSeries<ObservableValue>
            {
                Name = kvp.Key.ToString(),
                Values = new[] { kvp.Value },
                Fill = new SolidColorPaint(GetColorFor(kvp.Key))
            })
            .ToArray();

            foreach (var monitoring in _monitorings)
            {
                monitoring.PropertyChanged += OnMonitoringPropertyChanged;
            }

            Recalculate();

            _monitorings.CollectionChanged += OnMonitoringsChanged;
        }

        private void OnMonitoringsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems is not null)
            {
                foreach(ArtworkMonitoring removed in e.OldItems)
                {
                    removed.PropertyChanged -= OnMonitoringPropertyChanged;
                }
            }

            if(e.NewItems is not null)
            {
                foreach (ArtworkMonitoring added in e.NewItems)
                {
                    added.PropertyChanged += OnMonitoringPropertyChanged;
                }
            }

            Recalculate();
        }

        private void OnMonitoringPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ArtworkMonitoring.State))
            {
                Recalculate();
            }
        }

        private void Recalculate()
        {
            var counts = _monitorings
                .GroupBy(m => m.State)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var pair in _counters)
            {
                pair.Value.Value = counts.TryGetValue(pair.Key, out var count) ? count : 0;
            }
        }

        private static SKColor GetColorFor(ArtworkCondition condition) => condition switch
        {
            ArtworkCondition.Pristine => new SKColor(76, 175, 80),
            ArtworkCondition.MinorWear => new SKColor(255, 193, 7),
            ArtworkCondition.Deteriorating => new SKColor(255, 152, 0),
            ArtworkCondition.Critical => new SKColor(244, 67, 54),
            _ => SKColors.Gray
        };
    }
}
