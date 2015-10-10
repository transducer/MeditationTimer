using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Rooijakkers.MeditationTimer.Data.Contracts;
using Rooijakkers.MeditationTimer.Model;
using Rooijakkers.MeditationTimer.Utilities;

namespace Rooijakkers.MeditationTimer.Data
{
    public class MeditationDiaryRepository : IMeditationDiaryRepository
    {
        private const string JSON_FILENAME = "noNonsenseMeditationTimerData.json";

        public void AddEntryAsync(MeditationEntry entry)
        {
            EnsureJsonFileExists();
            AddEntryIntoJsonAsync(entry);
        }

        public async Task<MeditationDiary> GetAsync()
        {
            EnsureJsonFileExists();

            var meditationDiaryJson = await ReadJsonAsync();

            return ConvertToMeditationDiary(meditationDiaryJson);
        }

        private void EnsureJsonFileExists()
        {
            var task = Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(JSON_FILENAME) as StorageFile;
                if (file == null)
                {
                    await ApplicationData.Current.LocalFolder.CreateFileAsync(JSON_FILENAME);
                }
            });

            task.Wait();
        }

        /// <summary>
        /// Writes to the JSON string stored in the JSON_FILENAME.
        /// </summary>
        /// <returns></returns>
        private async void WriteJsonAsync(MeditationDiary diary)
        {
            var serializer = new DataContractJsonSerializer(typeof(MeditationDiary));

            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(JSON_FILENAME,
                CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, diary);
            }
        }

        /// <summary>
        /// Reads the JSON string stored in the JSON_FILENAME.
        /// </summary>
        /// <returns></returns>
        private async Task<string> ReadJsonAsync()
        {
            string content;

            var jsonStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSON_FILENAME);
            using (var reader = new StreamReader(jsonStream))
            {
                content = await reader.ReadToEndAsync();
            }

            return content;
        }

        private async void AddEntryIntoJsonAsync(MeditationEntry entry)
        {
            var content = await ReadJsonAsync();
            var diary = ConvertToMeditationDiary(content);

            diary.Insert(0, entry);

            WriteJsonAsync(diary);
        }

        private MeditationDiary ConvertToMeditationDiary(string json)
        {
            MeditationDiary diary;

            var serializer = new DataContractJsonSerializer(typeof(MeditationDiary));
            using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                diary = serializer.ReadObject(memoryStream) as MeditationDiary;
            }

            return diary ?? new MeditationDiary();
        }
    }
}
