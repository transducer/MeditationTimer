using System.Threading.Tasks;
using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.Data.Contracts
{
    public interface IMeditationDiaryRepository
    {
        Task AddEntryAsync(MeditationEntry entry);
        Task<MeditationDiary> GetAsync();
    }
}
