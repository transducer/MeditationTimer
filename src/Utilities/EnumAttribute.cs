using System;

namespace Rooijakkers.MeditationTimer.Utilities
{
    /// <remarks>
    /// Windows Phone 8.1 does not reference System.ComponentModel.DataAnnotations, so we have to create our own.
    /// Source: www.stackoverflow.com/questions/25887725/windows-phone-8-1-how-to-add-data-annotations-to-enum-value-types-to-get-the-a#25961073
    /// </remarks>

    [AttributeUsage(AttributeTargets.All)]
    public class DescriptionAttribute : Attribute
    {
        public string Name { get; private set; }

        public DescriptionAttribute(string name)
        {
            this.Name = name;
        }
    }
}
