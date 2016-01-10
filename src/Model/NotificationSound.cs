using Rooijakkers.MeditationTimer.Utilities;

namespace Rooijakkers.MeditationTimer.Model
{
    public enum NotificationSound
    {
        [Description("Thai Buddhists chanting - by Kyles")]
        ThaiBuddhist,
        [Description("Jong Sou Temple chanting - by RTB45")]
        JongSou,
        [Description("Wat Phra Si Ratana Mahathat chanting - by RTB45")]
        RatanaMahathat,
        [Description("Ohm sound - by Jagadamba")]
        Ohm,
        [Description("Oahu temple bells - by Bumpy")]
        Oahu,
        [Description("Hare Krishna chanting - by Jagadamba")]
        HareKrishna,

        // Notification sounds also contain bell sounds (copy pasted since we cannot inherit from another enum)
        [Description("Burmese gong")]
        Burmese,
        [Description("Cymbals Duisberg - by the Very Real Horst")]
        Cymbals,
        [Description("Perfect gong - by Qudobup")]
        Perfect,
        [Description("Bell with flute - by Nocpr")]
        Flute
    }
}
