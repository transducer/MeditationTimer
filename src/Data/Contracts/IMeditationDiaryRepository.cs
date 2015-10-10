using System.Threading.Tasks;
using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.Data.Contracts
{
    public interface IMeditationDiaryRepository
    {
        void AddEntryAsync(MeditationEntry entry);
        Task<MeditationDiary> GetAsync();
    }
}
