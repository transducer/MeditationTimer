using System.Threading.Tasks;
using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.Data.Contracts
{
    public interface IMeditationDiaryRepository
    {
        Task AddEntryAsync(MeditationEntry entry);
        Task DeleteEntryAsync(int entryId);
        Task<MeditationDiary> GetAsync();
    }
}
