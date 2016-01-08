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
        private const string RING_BELL_FIVE_MINUTES_BEFORE_END_STORAGE = "RingBellFiveMinutesBeforeEndStorage";

        public bool RingBellFiveMinutesBeforeEnd
        {
            get
            {
                bool? ringBellBeforeEnd =
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[RING_BELL_FIVE_MINUTES_BEFORE_END_STORAGE] as bool?;

                // If settings were not yet stored set to default value.
                if (ringBellBeforeEnd == null)
                {
                    ringBellBeforeEnd = Constants.DEFAULT_RING_BELL_FIVE_MINUTES_BEFORE_END;
                    SetRingBellBeforeEnd(ringBellBeforeEnd.Value);
                }

                return ringBellBeforeEnd.Value;
            }
            set
            {
                SetRingBellBeforeEnd(value);
            }
        }
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

        private void SetRingBellBeforeEnd(bool value)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[RING_BELL_FIVE_MINUTES_BEFORE_END_STORAGE] = value;
        }

        private void SetTimeToGetReady(int value)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[TIME_TO_GET_READY_IN_SECONDS_STORAGE] = value;
        }
    }
}
