using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

using Rooijakkers.MeditationTimer.Data.Contracts;
using Rooijakkers.MeditationTimer.Messages;
using Rooijakkers.MeditationTimer.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Rooijakkers.MeditationTimer.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly IMeditationDiaryRepository _repository;
        private static readonly TimeSpan TenMinutes = new TimeSpan(0, 10, 0);

        public StatisticsViewModel(IMeditationDiaryRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _repository = repository;

            // Initially update diary
            UpdateDiary();

            Messenger.Default.Register<UpdateDiaryMessage>(this, ReceiveUpdateDiaryMessage);
        }

        private void SeedDesignTimeData()
        {
            for (var i = 0; i < 1000; i++)
            {
                MeditationDiary.Add(new MeditationEntry { TimeMeditated = TenMinutes, StartTime = DateTime.Now });
            }
        }

        private IEnumerable<HoursMeditatedPerWeek> _hoursMeditatedPerWeeks;
        public IEnumerable<HoursMeditatedPerWeek> HoursMeditatedPerWeeks
        {
            get
            {
                RaisePropertyChanged(nameof(HoursMeditatedPerWeeks));
                return _hoursMeditatedPerWeeks ?? (_hoursMeditatedPerWeeks = new Collection<HoursMeditatedPerWeek>());
            }
            set
            {
                _hoursMeditatedPerWeeks = value;
                RaisePropertyChanged(nameof(HoursMeditatedPerWeeks));
            }
        }

        private IEnumerable<HoursMeditatedPerWeek> CalculateHoursMeditatedPerWeeks()
        {
            // TODO: Sort by years. If the app is used for longer than a year this statistic fails.
            Func<MeditationEntry, int> weekProjector = d =>
                CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d.StartTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            var _hoursMeditatedPerWeeks = MeditationDiary
                .GroupBy(weekProjector)
                .Select(g => new HoursMeditatedPerWeek
                {
                    WeekNumber = g.Key,
                    HoursMeditated = g.ToList().Sum(m => m.TimeMeditated.Minutes / 60)
                });

            return _hoursMeditatedPerWeeks;
        }

        private MeditationDiary _meditationDiary;
        public MeditationDiary MeditationDiary
        {
            get
            {
                return _meditationDiary ?? (_meditationDiary = new MeditationDiary());
            }
            set
            {
                _meditationDiary = value;
            }
        }

        /// <summary>
        /// Updates the diary to the latest version.
        /// </summary>
        /// <param name="msg"></param>
        private void ReceiveUpdateDiaryMessage(UpdateDiaryMessage msg)
        {
            UpdateDiary();

            // Also recalculate hours meditated per week
            HoursMeditatedPerWeeks = CalculateHoursMeditatedPerWeeks();
        }

        private async void UpdateDiary()
        {
            var latestDiary = await _repository.GetAsync();

            MeditationDiary.Clear();

            foreach (var entry in latestDiary)
            {
                MeditationDiary.Add(entry);
            }
        }
    }
}
