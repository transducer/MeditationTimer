using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Rooijakkers.MeditationTimer.Data.Contracts;
using Rooijakkers.MeditationTimer.Messages;
using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.ViewModel
{
    public class DiaryViewModel : ViewModelBase
    {
        private readonly IMeditationDiaryRepository _repository;
        private static readonly TimeSpan TenMinutes = new TimeSpan(0, 10, 0);

        public DiaryViewModel(IMeditationDiaryRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            _repository = repository;

            // Initially update diary
            UpdateDiary();

            Messenger.Default.Register<UpdateDiaryMessage>(this, ReceiveUpdateDiaryMessage);

            DeleteMeditationEntryCommand = new RelayCommand<int>(DeleteMeditationEntry);

            if (IsInDesignMode)
            {
                SeedDesignTimeData();
            }
        }

        private void SeedDesignTimeData()
        {
            for (var i = 0; i < 1000; i++)
            {
                MeditationDiary.Add(new MeditationEntry { TimeMeditated = TenMinutes, StartTime = DateTime.Now });
            }
        }

        private MeditationDiary _meditationDiary;
        public MeditationDiary MeditationDiary
        {
            get
            {
                return _meditationDiary ?? (_meditationDiary = new MeditationDiary());
            }
            set
            {
                _meditationDiary = value;
            }
        }

        public ICommand DeleteMeditationEntryCommand { get; private set; }

        /// <summary>
        /// Updates the diary to the latest version.
        /// </summary>
        /// <param name="msg"></param>
        private void ReceiveUpdateDiaryMessage(UpdateDiaryMessage msg)
        {
            UpdateDiary();
        }

        private async void UpdateDiary()
        {
            var latestDiary = await _repository.GetAsync();

            MeditationDiary.Clear();

            foreach (var entry in latestDiary)
            {
                MeditationDiary.Add(entry);
            }
        }

        private void DeleteMeditationEntry(int entryId)
        {
            var task = Task.Run(async () =>
            {
                await _repository.DeleteEntryAsync(entryId);
            });

            task.Wait();

            UpdateDiary();
        }
    }
}
