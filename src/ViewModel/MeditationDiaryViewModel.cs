using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Rooijakkers.MeditationTimer.Data;
using Rooijakkers.MeditationTimer.Data.Contracts;

namespace Rooijakkers.MeditationTimer.ViewModel
{
    public class MeditationDiaryViewModel : ViewModelBase
    {
        private readonly IMeditationDiaryRepository _repository;

        public MeditationDiaryViewModel()
        {
            _repository = new MeditationDiaryRepository();
            DisplayLatestDiary();
        }

        private void DisplayLatestDiary()
        {
            var latestDiary = _repository.GetAsync();
            Messenger.Default.Send(new DiaryMessage(latestDiary));
        }
    }
}
