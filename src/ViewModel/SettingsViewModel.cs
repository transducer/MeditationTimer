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

            SetTimeToGetReadyCommand = new RelayCommand(SetTimeToGetReady);
            TimeToGetReadySliderValue = settings.TimeToGetReady.Seconds;
        }

        public ICommand SetTimeToGetReadyCommand { get; private set; }

        private void SetTimeToGetReady()
        {
            _settings.TimeToGetReady = TimeSpan.FromSeconds(TimeToGetReadySliderValue);
        }

        public double TimeToGetReadySliderValue { get; set; }
    }
}
