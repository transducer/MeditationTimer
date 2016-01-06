using System;

namespace Rooijakkers.MeditationTimer.Data
{
    /// <summary>
    /// Stores settings data in local settings storage
    /// </summary>
    public class Settings
    {
        private const string TIME_TO_GET_READY_IN_SECONDS_STORAGE = "TimeToGetReadyStorage";

        public TimeSpan TimeToGetReady
        {
            get
            {
                var secondsToGetReady = 
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[TIME_TO_GET_READY_IN_SECONDS_STORAGE];

                if (secondsToGetReady == null)
                {
                    secondsToGetReady = Constants.DEFAULT_TIME_TO_GET_READY_IN_SECONDS;
                    SetTimeToGetReady(Convert.ToInt32(secondsToGetReady));
                }

                // Return stored time in seconds as a TimeSpan.
                return new TimeSpan(0, 0, Convert.ToInt32(secondsToGetReady));
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
