using System;
using Windows.Storage;
using Rooijakkers.MeditationTimer.Data.Contracts;

namespace Rooijakkers.MeditationTimer.Data
{
    public class SettingsRepository : ISettingsRepository
    {
        public TimeSpan GetTimeToSitReady()
        {
            throw new NotImplementedException();
        }

        public void SetTimeToSitReady(int seconds)
        {
            throw new NotImplementedException();
        }
    }
}
