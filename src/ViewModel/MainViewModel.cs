using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Rooijakkers.MeditationTimer.Data.Contracts;
using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private static readonly TimeSpan OneSecond = new TimeSpan(0, 0, 1);

        // While debugging we want 10 seconds to last 10 times as short
#if DEBUG
        private static readonly TimeSpan TenSeconds = new TimeSpan(0, 0, 1);
#else
        private static readonly TimeSpan TenSeconds = new TimeSpan(0, 0, 10);
#endif
        private static readonly TimeSpan FiveMinutes = new TimeSpan(0, 5, 0);
        private static readonly TimeSpan TenMinutes = new TimeSpan(0, 10, 0);
        private static readonly TimeSpan TimeToSitReady = TenSeconds;

        private readonly IMeditationDiaryRepository _repository;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IMeditationDiaryRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _repository = repository;

            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);
            AddFiveMinutesCommand = new RelayCommand(AddFiveMinutes);
            ResetInitialTimeCommand = new RelayCommand(ResetInitialTime);

            InitializeDispatcherTimer();

            CountdownTimerValue = InitialMeditationTime;

            // Initially update diary
            UpdateDiary();

            if (IsInDesignMode)
            {
                SeedDesignTimeData();
            }
        }

        private void SeedDesignTimeData()
        {
            for (var i = 0; i < 1000; i++)
            {
                MeditationDiary.Add(new MeditationEntry { TimeMeditated = TenMinutes, StartTime = DateTime.Now });
            }
        }

        private void InitializeDispatcherTimer()
        {
            DispatcherTimer = new DispatcherTimer
            {
                Interval = OneSecond
            };
            DispatcherTimer.Tick += TimerTick;
            DispatcherTimer.Tick += (s, e) =>
                RingBellMoments(InitialMeditationTime, TimeSpan.Zero.Add(FiveMinutes), TimeSpan.Zero);
            DispatcherTimer.Tick += StopTimerOnEnd;
        }

        public ICommand StartTimerCommand { get; private set; }
        public ICommand StopTimerCommand { get; private set; }
        public ICommand AddFiveMinutesCommand { get; private set; }
        public ICommand ResetInitialTimeCommand { get; private set; }
        public DispatcherTimer DispatcherTimer { get; private set; }

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

        private TimeSpan _initialMeditationTime;
        public TimeSpan InitialMeditationTime
        {
            get
            {
                if (_initialMeditationTime == default(TimeSpan))
                {
                    _initialMeditationTime = TenMinutes;
                }

                return _initialMeditationTime;
            }
            set
            {
                _initialMeditationTime = value;
            }
        }

        private TimeSpan _countdownTimerValue;
        public TimeSpan CountdownTimerValue
        {
            get
            {
                RaisePropertyChanged(nameof(TimerText));
                return _countdownTimerValue;
            }
            set
            {
                _countdownTimerValue = value;
                RaisePropertyChanged(nameof(TimerText));
            }
        }

        // Ensure TimeMedidated is not a negative value if there is meditated for less than the
        // time to sit ready
        public TimeSpan TimeMeditated => CountdownTimerValue < InitialMeditationTime
            ? InitialMeditationTime.Subtract(CountdownTimerValue)
            : TimeSpan.Zero;

        public bool MeditatedForLongerThanTimeToSitReady => TimeMeditated > TimeSpan.Zero;

        public string TimerText
        {
            get
            {
                var totalMinutes = (int)CountdownTimerValue.TotalMinutes;
                var seconds = CountdownTimerValue.Seconds % 60;

                var minutesText = totalMinutes < 10 ? "0" + totalMinutes : totalMinutes.ToString();
                var secondsText = seconds < 10 ? "0" + seconds : seconds.ToString();

                return minutesText + ":" + secondsText;
            }
        }

        private void StartTimer()
        {
            CountdownTimerValue = CountdownTimerValue += TimeToSitReady;
            DispatcherTimer.Start();
        }

        private void StopTimer()
        {
            if (MeditatedForLongerThanTimeToSitReady)
            {
                AddMeditationEntry();
                UpdateDiary();
            }

            CountdownTimerValue = InitialMeditationTime;
            DispatcherTimer.Stop();
        }

        private void ResetInitialTime()
        {
            CountdownTimerValue = TenMinutes;
        }

        private void AddMeditationEntry()
        {
            var meditationEntry = new MeditationEntry
            {
                StartTime = DateTime.Now.Subtract(TimeMeditated),
                TimeMeditated = TimeMeditated
            };

            var task = Task.Run(async () =>
            {
                await _repository.AddEntryAsync(meditationEntry);
            });

            task.Wait();
        }

        private void AddFiveMinutes()
        {
            InitialMeditationTime += FiveMinutes;
            CountdownTimerValue += FiveMinutes;
        }

        private void TimerTick(object sender, object e)
        {
            CountdownTimerValue = CountdownTimerValue.Subtract(OneSecond);
        }

        private void RingBellMoments(params TimeSpan[] moments)
        {
            if (moments.Contains(CountdownTimerValue))
            {
                RingBell();
            }
        }

        private void StopTimerOnEnd(object sender, object e)
        {
            if (CountdownTimerValue == TimeSpan.Zero)
            {
                StopTimer();
            }
        }

        private void RingBell()
        {
            Messenger.Default.Send(new PlayMessage());
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
