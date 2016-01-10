using Rooijakkers.MeditationTimer.Utilities;

namespace Rooijakkers.MeditationTimer.Model
{
    public enum NotificationSound
    {
        [Description("Thai Buddhists chanting - by Kyles")]
        ThaiBuddhist = 0,
        [Description("Jong Sou Temple chanting - by RTB45")]
        JongSou = 1,
        [Description("Wat Phra Si Ratana Mahathat chanting - by RTB45")]
        RatanaMahathat = 2,
        [Description("Ohm sound - by Jagadamba")]
        Ohm = 3,
        [Description("Oahu temple bells - by Bumpy")]
        Oahu = 4,
        [Description("Hare Krishna chanting - by Jagadamba")]
        HareKrishna = 5,

        // Notification sounds also contain bell sounds (copy pasted since we cannot inherit from another enum)
        [Description("Burmese gong")]
        Burmese = 6,
        [Description("Cymbals Duisberg - by the Very Real Horst")]
        Cymbals = 7,
        [Description("Perfect gong - by Qudobup")]
        Perfect = 8,
        [Description("Bell with flute - by Nocpr")]
        Flute = 9
    }
}
