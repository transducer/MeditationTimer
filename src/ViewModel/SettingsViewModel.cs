using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Rooijakkers.MeditationTimer.Data;

namespace Rooijakkers.MeditationTimer.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            SetTimeToGetReadyCommand = new RelayCommand(SetTimeToGetReady);
            TimeToGetReadySliderValue = Settings.TimeToGetReady.Seconds;
        }

        public ICommand SetTimeToGetReadyCommand { get; private set; }

        private void SetTimeToGetReady()
        {
            Settings.TimeToGetReady = TimeSpan.FromSeconds(TimeToGetReadySliderValue);
        }

        public double TimeToGetReadySliderValue { get; set; }
    }
}
