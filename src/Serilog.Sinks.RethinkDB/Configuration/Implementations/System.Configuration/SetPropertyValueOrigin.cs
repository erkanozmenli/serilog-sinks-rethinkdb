using System.Configuration;

namespace Serilog.Sinks.RethinkDB
{
    public static partial class SetProperty
    {
        public static void IfProvided<T>(ConfigurationElement element, string propertyName, PropertySetter<T> setter)
        {
            if (element == null)
            {
                return;
            }

            var property = element.ElementInformation.Properties[propertyName];
            if (property.ValueOrigin == PropertyValueOrigin.Default) return;
            IfNotNull((string)property.Value, setter);
        }

        public static void IfProvidedNotEmpty<T>(ConfigurationElement element, string propertyName, PropertySetter<T> setter)
        {
            if (element == null)
            {
                return;
            }

            var property = element.ElementInformation.Properties[propertyName];
            if (property.ValueOrigin == PropertyValueOrigin.Default) return;
            IfNotNullOrEmpty((string)property.Value, setter);
        }
    }
}
