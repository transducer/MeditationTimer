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
        private const string BEGIN_SOUND_STORAGE = "BeginSoundStorage";
        private const string END_SOUND_STORAGE = "EndSoundStorage";
        private const string NOTIFICATION_SOUND_STORAGE = "NotificationSoundStorage";

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

        public BellSound BeginSound
        {
            get
            {
                BellSound? beginSound = 
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[BEGIN_SOUND_STORAGE] as BellSound?;

                // If settings were not yet stored set to default value.
                if (beginSound == null)
                {
                    beginSound = Constants.DEFAULT_BEGIN_SOUND;
                    SetBeginSound(beginSound.Value);
                }

                return beginSound.Value;
            }
            set
            {
                SetBeginSound(value);
            }
        }

        public BellSound EndSound
        {
            get
            {
                BellSound? endSound = 
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[END_SOUND_STORAGE] as BellSound?;

                // If settings were not yet stored set to default value.
                if (endSound == null)
                {
                    endSound = Constants.DEFAULT_END_SOUND;
                    SetEndSound(endSound.Value);
                }

                return endSound.Value;
            }
            set
            {
                SetEndSound(value);
            }
        }

        public NotificationSound NotificationSound
        {
            get
            {
                NotificationSound? notificationSound = 
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[NOTIFICATION_SOUND_STORAGE] as NotificationSound?;

                // If settings were not yet stored set to default value.
                if (notificationSound == null)
                {
                    notificationSound = Constants.DEFAULT_NOTIFICATION_SOUND;
                    SetNotificationSound(notificationSound.Value);
                }

                return notificationSound.Value;
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

        private void SetBeginSound(BellSound value)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[BEGIN_SOUND_STORAGE] = value;
        }

        private void SetEndSound(BellSound value)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[END_SOUND_STORAGE] = value;
        }

        private void SetNotificationSound(NotificationSound value)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[NOTIFICATION_SOUND_STORAGE] = value;
        }
    }
}
