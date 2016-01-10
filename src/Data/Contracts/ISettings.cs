using Rooijakkers.MeditationTimer.Model;
using System;

namespace Rooijakkers.MeditationTimer.Data.Contracts
{
    public interface ISettings
    {
        TimeSpan TimeToGetReady { get; set; }
        bool RingBellFiveMinutesBeforeEnd { get; set; }
        BellSound BeginSound { get; set; }
        BellSound EndSound { get; set; }
        NotificationSound NotificationSound { get; set; }
    }
}