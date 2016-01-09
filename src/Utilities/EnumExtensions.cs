using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Rooijakkers.MeditationTimer.Utilities
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        // http://www.minddriven.de/index.php/technology/dot-net/net-winrt-get-custom-attributes-from-enum-value
        public static string GetDescription(this Enum enumValue)
	    {
            return enumValue
                .GetType()
                .GetTypeInfo()
                .GetDeclaredField(enumValue.ToString())
                .GetCustomAttribute<DescriptionAttribute>()
                .Name;
	    }
    }
}
