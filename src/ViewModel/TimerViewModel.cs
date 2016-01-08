using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Rooijakkers.MeditationTimer.Data.Contracts;
using Rooijakkers.MeditationTimer.Messages;
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
    public class TimerViewModel : ViewModelBase
    {
        private static readonly TimeSpan OneSecond = new TimeSpan(0, 0, 1);

        // While debugging we want 10 seconds to last 5 times as short
#if DEBUG
        private static readonly TimeSpan TenSeconds = new TimeSpan(0, 0, 2);
#else
        private static readonly TimeSpan TenSeconds = new TimeSpan(0, 0, 10);
#endif
        private static readonly TimeSpan FiveMinutes = new TimeSpan(0, 5, 0);
        private static readonly TimeSpan TenMinutes = new TimeSpan(0, 10, 0);

        private readonly IMeditationDiaryRepository _repository;
        private readonly ISettings _settings;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public TimerViewModel(IMeditationDiaryRepository repository, ISettings settings)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _repository = repository;

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            _settings = settings;

            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);
            AddFiveMinutesCommand = new RelayCommand(AddFiveMinutes);
            ResetInitialTimeCommand = new RelayCommand(ResetInitialTime);

            InitializeDispatcherTimer();

            CountdownTimerValue = InitialMeditationTime;
        }

        private void InitializeDispatcherTimer()
        {
            DispatcherTimer = new DispatcherTimer
            {
                Interval = OneSecond
            };
            DispatcherTimer.Tick += TimerTick;
            DispatcherTimer.Tick += (s, e) =>
                RingBellOnMoment(InitialMeditationTime, TimeSpan.Zero);
            DispatcherTimer.Tick += (s, e) =>
                RingFiveMinutesLeftBellOnMoment(TimeSpan.Zero.Add(FiveMinutes));
            DispatcherTimer.Tick += StopTimerOnEnd;
            DispatcherTimer.Tick += DisplaySitReadyMessageAtBegin;
        }

        public ICommand StartTimerCommand { get; private set; }
        public ICommand StopTimerCommand { get; private set; }
        public ICommand AddFiveMinutesCommand { get; private set; }
        public ICommand ResetInitialTimeCommand { get; private set; }
        public DispatcherTimer DispatcherTimer { get; private set; }

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
            CountdownTimerValue = CountdownTimerValue += _settings.TimeToGetReady;
            DispatcherTimer.Start();

            Messenger.Default.Send(new StartTimerMessage());
        }

        private void StopTimer()
        {
            if (MeditatedForLongerThanTimeToSitReady)
            {
                AddMeditationEntry();
                Messenger.Default.Send(new UpdateDiaryMessage());
            }

            CountdownTimerValue = InitialMeditationTime;
            DispatcherTimer.Stop();

            Messenger.Default.Send(new StopTimerMessage());
            Messenger.Default.Send(new DisplaySitReadyMessage(false));
        }

        private void ResetInitialTime()
        {
            InitialMeditationTime = TenMinutes;
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

        private void DisplaySitReadyMessageAtBegin(object sender, object e)
        {
            Messenger.Default.Send(CountdownTimerValue > InitialMeditationTime
                ? new DisplaySitReadyMessage(true)
                : new DisplaySitReadyMessage(false));
        }

        private void RingBellOnMoment(params TimeSpan[] moments)
        {
            if (moments.Contains(CountdownTimerValue))
            {
                RingBell();
            }
        }

        private void RingFiveMinutesLeftBellOnMoment(params TimeSpan[] moments)
        {
            if (moments.Contains(CountdownTimerValue))
            {
                RingFiveMinutesLeftBell();
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
            Messenger.Default.Send(new PlayMessage(BellSound.Burmese));
        }

        private void RingFiveMinutesLeftBell()
        {
            Messenger.Default.Send(new PlayMessage(BellSound.Cymbals));
        }
    }
}
