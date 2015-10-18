using System;

namespace Rooijakkers.MeditationTimer.Model
{
    public class MeditationEntry
    {
        public int EntryId { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan TimeMeditated { get; set; }
    }
}
