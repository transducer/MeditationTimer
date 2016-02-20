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

            UpdateDiaryAndStatistics();

            Messenger.Default.Register<UpdateDiaryMessage>(this, ReceiveUpdateDiaryMessage);
        }

        private void SeedDesignTimeData()
        {
            for (var i = 0; i < 1000; i++)
            {
                MeditationDiary.Add(new MeditationEntry { TimeMeditated = TenMinutes, StartTime = DateTime.Now });
            }
        }

        private IEnumerable<HoursMeditatedPerWeekPerYear> _hoursMeditatedOverview;
        public IEnumerable<HoursMeditatedPerWeekPerYear> HoursMeditatedOverview
        {
            get
            {
                RaisePropertyChanged(nameof(HoursMeditatedOverview));
                return _hoursMeditatedOverview ?? (_hoursMeditatedOverview = new Collection<HoursMeditatedPerWeekPerYear>());
            }
            set
            {
                _hoursMeditatedOverview = value;
                RaisePropertyChanged(nameof(HoursMeditatedOverview));
            }
        }

        private IEnumerable<HoursMeditatedPerWeekPerYear> CalculateHoursMeditatedOverview()
        {
            const int MINUTES_PER_HOUR = 60;

            Func<MeditationEntry, string> weekWithYearProjector = 
                h => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(h.StartTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday) 
                    + " (" + h.StartTime.ToString("MMM") + " " + h.StartTime.Year + ")";

            var hoursMeditatedPerWeeks = MeditationDiary
                .GroupBy(weekWithYearProjector)
                .Select(g => new HoursMeditatedPerWeekPerYear
                {
                    WeekAndYear = g.Key,
                    HoursMeditated = g.ToList().Sum(m => m.TimeMeditated.Minutes) / MINUTES_PER_HOUR
                });

            return hoursMeditatedPerWeeks;
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
            UpdateDiaryAndStatistics();
        }

        private async void UpdateDiaryAndStatistics()
        {
            var latestDiary = await _repository.GetAsync();

            MeditationDiary.Clear();

            foreach (var entry in latestDiary)
            {
                MeditationDiary.Add(entry);
            }
            SendDisplayDiaryMessage();

            // Update statistics after diary is updated
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            // Recalculate hours meditated per week
            HoursMeditatedOverview = CalculateHoursMeditatedOverview();
        }

        private void SendDisplayDiaryMessage()
        {
            Messenger.Default.Send(new DisplayDiaryMessage(display: MeditationDiary.Count > 0));
        }
    }
}
