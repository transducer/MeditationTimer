using System;
using Rooijakkers.MeditationTimer.Data.Contracts;

namespace Rooijakkers.MeditationTimer.Data
{
    /// <summary>
    /// Stores settings data in local settings storage
    /// </summary>
    public class Settings : ISettings
    {
        private const string TIME_TO_GET_READY_IN_SECONDS_STORAGE = "TimeToGetReadyStorage";

        public TimeSpan TimeToGetReady
        {
            get
            {
                int? secondsToGetReady = 
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[TIME_TO_GET_READY_IN_SECONDS_STORAGE] as int?;

                // If settings were not yet stored set to default value.
                if (secondsToGetReady == null)
                {
                    secondsToGetReady = Constants.DEFAULT_TIME_TO_GET_READY_IN_SECONDS;
                    SetTimeToGetReady(secondsToGetReady.Value);
                }

                // Return stored time in seconds as a TimeSpan.
                return TimeSpan.FromSeconds(secondsToGetReady.Value);
            }
            set
            {
                // Store time in seconds obtained from TimeSpan.
                SetTimeToGetReady(value.Seconds);
            }
        }

        private void SetTimeToGetReady(int value)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[TIME_TO_GET_READY_IN_SECONDS_STORAGE] = value;
        }
    }
}
