using Common.Enums;
using Common.Models;
using RVAProjekatTim8.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace RVAProjekatTim8.Services
{
    /// <summary>
    /// Simulira automatski prolazak ArtworkMonitoring instance kroz sva
    /// stanja iz ArtworkCondition enuma, redom, na fiksnom intervalu, dok ne
    /// dostigne terminalno stanje (Critical).
    /// </summary>
    public class ArtworkConditionSimulator : IArtworkConditionSimulator
    {
        private static readonly ArtworkCondition[] StateSequence = (ArtworkCondition[])Enum.GetValues(typeof(ArtworkCondition));
        private readonly TimeSpan _interval;
        private readonly Dictionary<Guid, DispatcherTimer> _activeTimers = new Dictionary<Guid, DispatcherTimer>();

        public ArtworkConditionSimulator(TimeSpan interval)
        {
            _interval = interval;
        }

        public void StartSimulating(ArtworkMonitoring monitoring)
        {
            StopSimulating(monitoring.Id);

            var currentIndex = Array.IndexOf(StateSequence, monitoring.State);

            if(currentIndex < 0 || currentIndex >= StateSequence.Length - 1)
            {
                return;
            }

            var timer = new DispatcherTimer { Interval = _interval };

            timer.Tick += (_, _) =>
            {
                var index = Array.IndexOf(StateSequence, monitoring.State);
                var nextIndex = index + 1;

                if (nextIndex >= StateSequence.Length)
                {
                    StopSimulating(monitoring.Id);
                    return;
                }

                monitoring.State = StateSequence[nextIndex];

                if (nextIndex == StateSequence.Length - 1)
                {
                    StopSimulating(monitoring.Id);
                }
            };

            _activeTimers[monitoring.Id] = timer;
            timer.Start();
        }

        public void StopSimulating(Guid monitoringId)
        {
            if(_activeTimers.TryGetValue(monitoringId, out var timer))
            {
                timer.Stop();
                _activeTimers.Remove(monitoringId);
            }
        }
    }
}
