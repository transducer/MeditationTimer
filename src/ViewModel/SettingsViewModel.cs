using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Rooijakkers.MeditationTimer.Data.Contracts;
using System.ComponentModel;

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

            SetTimeToGetReadyCommand = new RelayCommand(SetTimeToGetReady);

            SetTimeToGetReadySliderValueToValueInSettings();
        }

        public void SetTimeToGetReadySliderValueToValueInSettings()
        {
            TimeToGetReadySliderValue = _settings.TimeToGetReady.Seconds;
        }

        public ICommand SetTimeToGetReadyCommand { get; private set; }

        private void SetTimeToGetReady()
        {
            _settings.TimeToGetReady = TimeSpan.FromSeconds(TimeToGetReadySliderValue);
        }

        private double _timeToGetReadySliderValue;
        public double TimeToGetReadySliderValue
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
    }
}
