using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.Messages
{
    /// <summary>
    /// Class used to send play notificaion event to view
    /// </summary>
    public class PlayNotificationMessage
    {
        public PlayNotificationMessage(NotificationSound sound)
        {
            NotificationSound = sound;
        }

        public NotificationSound NotificationSound { get; set; }
    }
}
