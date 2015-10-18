using Rooijakkers.MeditationTimer.Model;

namespace Rooijakkers.MeditationTimer.ViewModel
{
    /// <summary>
    /// Class used to send display sit ready event or not to view
    /// </summary>
    public class DisplaySitReadyMessage
    {
        public DisplaySitReadyMessage(bool display)
        {
            Display = display;
        }

        public bool Display { get; set; }
    }
}
