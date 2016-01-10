using System;
using Rooijakkers.MeditationTimer.Data.Contracts;
using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.Data
{
    /// <summary>
    /// Stores settings data in local settings storage
    /// </summary>
    public class Settings : ISettings
    {
        private const string TIME_TO_GET_READY_IN_SECONDS_STORAGE = "TimeToGetReadyStorage";
        private const string RING_BELL_FIVE_MINUTES_BEFORE_END_STORAGE = "RingBellFiveMinutesBeforeEndStorage";
        private const string BELL_SOUND_STORAGE = "BellSoundStorage";
        private const string NOTIFICATION_SOUND_STORAGE = "NotificationSoundStorage";

        public bool NotificationBeforeEnd
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

        public BellSound BellSound
        {
            get
            {
                // Enum storage is not supported, so store as int
                int? bellSoundEnumIndex = 
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[BELL_SOUND_STORAGE] as int?;

                // If settings were not yet stored set to default value.
                if (bellSoundEnumIndex == null)
                {
                    bellSoundEnumIndex = (int)Constants.DEFAULT_BELL_SOUND;
                    SetBellSound((BellSound)bellSoundEnumIndex);
                }

                return (BellSound)bellSoundEnumIndex;
            }
            set
            {
                SetBellSound(value);
            }
        }

        public NotificationSound NotificationSound
        {
            get
            {
                // Enum storage is not supported, so store as int
                int? notificationSoundEnumIndex = 
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[NOTIFICATION_SOUND_STORAGE] as int?;

                // If settings were not yet stored set to default value.
                if (notificationSoundEnumIndex == null)
                {
                    notificationSoundEnumIndex = (int)Constants.DEFAULT_NOTIFICATION_SOUND;
                    SetNotificationSound((NotificationSound)notificationSoundEnumIndex);
                }

                return (NotificationSound)notificationSoundEnumIndex;
            }
            set
            {
                SetNotificationSound(value);
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

        private void SetBellSound(BellSound value)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[BELL_SOUND_STORAGE] = (int)value;
        }

        private void SetNotificationSound(NotificationSound value)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[NOTIFICATION_SOUND_STORAGE] = (int)value;
        }
    }
}
