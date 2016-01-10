using Rooijakkers.MeditationTimer.Model;
using System;

namespace Rooijakkers.MeditationTimer.Data.Contracts
{
    public interface ISettings
    {
        TimeSpan TimeToGetReady { get; set; }
        bool NotificationBeforeEnd { get; set; }
        BellSound BellSound { get; set; }
        NotificationSound NotificationSound { get; set; }
    }
}