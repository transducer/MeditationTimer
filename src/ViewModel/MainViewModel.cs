using System;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Rooijakkers.MeditationTimer.Data;
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
        private static readonly TimeSpan TenSeconds = new TimeSpan(0, 0, 10);
        private static readonly TimeSpan FiveMinutes = new TimeSpan(0, 5, 0);
        private static readonly TimeSpan TenMinutes = new TimeSpan(0, 10, 0);

        private readonly IMeditationDiaryRepository _repository;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);
            AddFiveMinutesCommand = new RelayCommand(AddFiveMinutes);

            DispatcherTimer = new DispatcherTimer
            {
                Interval = OneSecond
            };
            DispatcherTimer.Tick += TimerTick;
            DispatcherTimer.Tick += (s, e) => RingBellMoments(InitialMeditationTime, TimeSpan.Zero.Add(FiveMinutes), TimeSpan.Zero);
            DispatcherTimer.Tick += StopTimerOnEnd;

            CountdownTimerValue = InitialMeditationTime;

            _repository = new MeditationDiaryRepository();
        }

        public ICommand StartTimerCommand { get; private set; }
        public ICommand StopTimerCommand { get; private set; }
        public ICommand AddFiveMinutesCommand { get; private set; }
        public DispatcherTimer DispatcherTimer { get; }

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
                CountdownTimerValue = InitialMeditationTime;
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

        public string TimerText => CountdownTimerValue.ToString(@"mm\:ss");

        private void StartTimer()
        {
            CountdownTimerValue = CountdownTimerValue += TenSeconds;
            DispatcherTimer.Start();
        }

        private void StopTimer()
        {
            AddMeditationEntry();
            UpdateDiary();

            CountdownTimerValue = InitialMeditationTime;
            DispatcherTimer.Stop();
        }

        private void AddMeditationEntry()
        {
            var timeMeditated = InitialMeditationTime.Subtract(CountdownTimerValue);
            var meditationEntry = new MeditationEntry
            {
                StartTime = DateTime.Now.Subtract(timeMeditated),
                TimeMeditated = timeMeditated
            };

            _repository.AddEntryAsync(meditationEntry);
        }

        private void AddFiveMinutes()
        {
            InitialMeditationTime += FiveMinutes;
        }

        private void TimerTick(object sender, object e)
        {
            var oneSecond = new TimeSpan(0, 0, 1);
            CountdownTimerValue = CountdownTimerValue.Subtract(oneSecond);
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

        private void UpdateDiary()
        {
            var latestDiary = _repository.GetAsync();
            Messenger.Default.Send(new DiaryMessage(latestDiary));
        }
    }
}
