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

        private IEnumerable<TimeMeditatedPerWeekPerYear> _timeMeditatedOverview;
        public IEnumerable<TimeMeditatedPerWeekPerYear> TimeMeditatedOverview
        {
            get
            {
                RaisePropertyChanged(nameof(TimeMeditatedOverview));
                return _timeMeditatedOverview ?? (_timeMeditatedOverview = new Collection<TimeMeditatedPerWeekPerYear>());
            }
            set
            {
                _timeMeditatedOverview = value;
                RaisePropertyChanged(nameof(TimeMeditatedOverview));
            }
        }

        private IEnumerable<TimeMeditatedPerWeekPerYear> CalculateTimeMeditatedOverview()
        {
            const int MINUTES_PER_HOUR = 60;
            const DayOfWeek START_OF_WEEK = DayOfWeek.Monday;

            Func<MeditationEntry, string> weekWithYearProjector = 
                h => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(h.StartTime, CalendarWeekRule.FirstDay, START_OF_WEEK) 
                    /* Since we are grouping by the week, month and year we have to get the month belonging to the day where the week started */
                    + " (" + CalculateMonthWhereinWeekStarted(h.StartTime, START_OF_WEEK) + " " + h.StartTime.Year + ")";

            var hoursMeditatedPerWeeks = MeditationDiary
                .GroupBy(weekWithYearProjector)
                .Select(g => {
                    var minutesMeditated = g.ToList().Sum(m => m.TimeMeditated.Minutes);
                    return new TimeMeditatedPerWeekPerYear
                    {
                        WeekAndYear = g.Key,
                        TimeMeditated = minutesMeditated / MINUTES_PER_HOUR + "h " + minutesMeditated % MINUTES_PER_HOUR + "m"
                    };
                });

            return hoursMeditatedPerWeeks;
        }

        private string CalculateMonthWhereinWeekStarted(DateTime dateTime, DayOfWeek startOfWeek)
        {
            var dayOfWeek = (int)dateTime.DayOfWeek;
            var startDayOffset = (int)startOfWeek;
            var dateTimeWhereWeekStarted = dateTime.AddDays(-dayOfWeek + startDayOffset);

            return dateTimeWhereWeekStarted.ToString("MMM");
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
            TimeMeditatedOverview = CalculateTimeMeditatedOverview();
        }

        private void SendDisplayDiaryMessage()
        {
            Messenger.Default.Send(new DisplayDiaryMessage(display: MeditationDiary.Count > 0));
        }
    }
}
