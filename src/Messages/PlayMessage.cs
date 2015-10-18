using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.Messages
{
    /// <summary>
    /// Class used to send play event to view
    /// </summary>
    public class PlayMessage
    {
        public PlayMessage(BellSound sound)
        {
            BellSound = sound;
        }

        public BellSound BellSound { get; set; }
    }
}
