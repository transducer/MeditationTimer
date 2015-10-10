using System.Threading.Tasks;
using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.ViewModel
{
    /// <summary>
    /// Class used to send new diary event to view
    /// </summary>
    public class DiaryMessage
    {
        public Task<MeditationDiary> CurrentMeditationDiary { get; set; }

        public DiaryMessage(Task<MeditationDiary> diary)
        {
            CurrentMeditationDiary = diary;
        }
    }
}
