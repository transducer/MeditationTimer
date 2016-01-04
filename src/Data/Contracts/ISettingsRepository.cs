using System.Threading.Tasks;
using Rooijakkers.MeditationTimer.Model;
using System;

namespace Rooijakkers.MeditationTimer.Data.Contracts
{
    public interface ISettingsRepository
    {
        void SetTimeToSitReady(int seconds);
        TimeSpan GetTimeToSitReady();
    }
}
