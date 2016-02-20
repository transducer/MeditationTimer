namespace Rooijakkers.MeditationTimer.Messages
{
    /// <summary>
    /// Class used to notify that the diary needs to be updated
    /// </summary>
    public class DisplayDiaryMessage
    {
        public DisplayDiaryMessage(bool display)
        {
            Display = display;
        }

        public bool Display { get; set; }
    }
}
