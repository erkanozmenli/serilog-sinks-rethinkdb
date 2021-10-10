using System;
using System.Globalization;

namespace Serilog.Sinks.RethinkDB
{
    public static partial class SetProperty
    {
        public delegate void PropertySetter<T>(T value);

        public static void IfNotNull<T>(string value, PropertySetter<T> setter)
        {
            if (value == null || setter == null) return;
            try
            {
                var setting = (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
                setter(setting);
            }
            // don't change the property if the conversion fails 
            catch (InvalidCastException) { }
            catch (OverflowException) { }
        }

        public static void IfNotNullOrEmpty<T>(string value, PropertySetter<T> setter)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            IfNotNull(value, setter);
        }
    }
}
