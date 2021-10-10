using System.Configuration;
using Serilog.Sinks.RethinkDB;

namespace Serilog.Configuration
{
    public class RethinkDBConfigurationSection : ConfigurationSection
    {
        public static RethinkDBConfigurationSection Settings
        { get; } = ConfigurationManager.GetSection(LoggerConfigurationRethinkDBExtensions.AppConfigSectionName) as RethinkDBConfigurationSection;

        public RethinkDBConfigurationSection()
        {

        }

        [ConfigurationProperty(nameof(Host))]
        public ValueConfigElement Host
        {
            get => (ValueConfigElement)base[nameof(Host)];
        }

        [ConfigurationProperty(nameof(Port))]
        public ValueConfigElement Port
        {
            get => (ValueConfigElement)base[nameof(Port)];
        }

        [ConfigurationProperty(nameof(Database))]
        public ValueConfigElement Database
        {
            get => (ValueConfigElement)base[nameof(Database)];
        }

        [ConfigurationProperty(nameof(Table))]
        public ValueConfigElement Table
        {
            get => (ValueConfigElement)base[nameof(Table)];
        }

        [ConfigurationProperty(nameof(Index))]
        public ValueConfigElement Index
        {
            get => (ValueConfigElement)base[nameof(Index)];
        }

        [ConfigurationProperty(nameof(BatchSizeLimit))]
        public ValueConfigElement BatchSizeLimit
        {
            get => (ValueConfigElement)base[nameof(BatchSizeLimit)];
        }

        [ConfigurationProperty(nameof(BatchPeriod))]
        public ValueConfigElement BatchPeriod
        {
            get => (ValueConfigElement)base[nameof(BatchPeriod)];
        }

        [ConfigurationProperty(nameof(EagerlyEmitFirstEvent))]
        public ValueConfigElement EagerlyEmitFirstEvent
        {
            get => (ValueConfigElement)base[nameof(EagerlyEmitFirstEvent)];
        }

        [ConfigurationProperty(nameof(QueueLimit))]
        public ValueConfigElement QueueLimit
        {
            get => (ValueConfigElement)base[nameof(QueueLimit)];
        }

        [ConfigurationProperty(nameof(UseProps))]
        public ValueConfigElement UseProps
        {
            get => (ValueConfigElement)base[nameof(UseProps)];
        }
    }
}

