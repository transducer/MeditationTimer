using System;

namespace Rooijakkers.MeditationTimer.Data.Contracts
{
    public interface ISettings
    {
        TimeSpan TimeToGetReady { get; set; }
    }
}