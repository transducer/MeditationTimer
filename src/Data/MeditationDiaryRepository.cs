using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Rooijakkers.MeditationTimer.Data.Contracts;
using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.Data
{
    public class MeditationDiaryRepository : IMeditationDiaryRepository
    {
        private const string JSON_FILENAME = "noNonsenseMeditationTimerData.json";

        public void AddEntryAsync(MeditationEntry entry)
        {
            AddEntryIntoJsonAsync(entry);
        }

        public async Task<MeditationDiary> GetAsync()
        {
            var meditationDiaryJson = await ReadJsonAsync();

            return ConvertToMeditationDiary(meditationDiaryJson);
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
            string content;

            var readStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSON_FILENAME);
            using (var reader = new StreamReader(readStream))
            {
                content = await reader.ReadToEndAsync();
            }

            var diary = ConvertToMeditationDiary(content);
            diary.Add(entry);

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

            return diary;
        }
    }
}
