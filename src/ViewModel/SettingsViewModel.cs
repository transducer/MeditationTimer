using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Rooijakkers.MeditationTimer.Data.Contracts;

namespace Rooijakkers.MeditationTimer.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettings _settings;

        public SettingsViewModel(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            _settings = settings;

            SaveSettingsCommand = new RelayCommand(SaveSettings);

            SetValuesToSettings();
        }

        public void SetValuesToSettings()
        {
            TimeToGetReadySliderValue = _settings.TimeToGetReady.Seconds;
        }

        public ICommand SaveSettingsCommand { get; private set; }

        /// <summary>
        /// Saves the current settings on the view model  
        /// </summary>
        private void SaveSettings()
        {
            SetTimeToGetReady();
            SetRingBellFiveMinutesBeforeEnd();
        }

        private void SetTimeToGetReady()
        {
            _settings.TimeToGetReady = TimeSpan.FromSeconds(TimeToGetReadySliderValue);
        }

        private void SetRingBellFiveMinutesBeforeEnd()
        {
            _settings.RingBellFiveMinutesBeforeEnd = RingBellFiveMinutesBeforeEndCheckBoxValue;
        }

        private double _timeToGetReadySliderValue; 
        public double TimeToGetReadySliderValue // Value needs to be a double for easy binding to slider.
        {
            get
            {
                RaisePropertyChanged(nameof(TimeToGetReadySliderValue));
                return _timeToGetReadySliderValue;
            }
            set
            {
                _timeToGetReadySliderValue = value;
                RaisePropertyChanged(nameof(TimeToGetReadySliderValue));
            }
        }

        private bool _ringBellFiveMinutesBeforeEndCheckBoxValue;
        public bool RingBellFiveMinutesBeforeEndCheckBoxValue 
        {
            get
            {
                RaisePropertyChanged(nameof(RingBellFiveMinutesBeforeEndCheckBoxValue));
                return _ringBellFiveMinutesBeforeEndCheckBoxValue;
            }
            set
            {
                _ringBellFiveMinutesBeforeEndCheckBoxValue = value;
                RaisePropertyChanged(nameof(RingBellFiveMinutesBeforeEndCheckBoxValue));
            }
        }
    }
}
