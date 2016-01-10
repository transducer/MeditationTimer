namespace Rooijakkers.MeditationTimer.Messages
{
    /// <summary>
    /// Class used as message to notify that notificiation sound picker settings should be displayed or not
    /// </summary>
    public class DisplayNotificationSoundPickerMessage
    {
        public bool Display { get; set; }
        public DisplayNotificationSoundPickerMessage(bool display)
        {
            Display = display;
        }
    }
}
