using System;

namespace Rooijakkers.MeditationTimer.Model
{
    public class MeditationEntry
    {
        public DateTime StartTime { get; set; }
        public TimeSpan TimeMeditated { get; set; }
    }
}
