using System.Configuration;
using Serilog.Configuration;
using Serilog.Sinks.RethinkDB.Configuration;

namespace Serilog.Sinks.RethinkDB
{
    internal class ApplySystemConfiguration : IApplySystemConfiguration
    {
        private readonly ISystemConfigurationSinkOptionsProvider _sinkOptionsProvider;

        public ApplySystemConfiguration() : this(
            new SystemConfigurationSinkOptionsProvider())
        {
        }

        public ApplySystemConfiguration(
            ISystemConfigurationSinkOptionsProvider sinkOptionsProvider)
        {
            _sinkOptionsProvider = sinkOptionsProvider;
        }

        public RethinkDBConfigurationSection GetSinkConfigurationSection(string configurationSectionName)
        {
            return ConfigurationManager.GetSection(configurationSectionName) as RethinkDBConfigurationSection;
        }

        public RethinkDBSinkOptions ConfigureSinkOptions(RethinkDBConfigurationSection config, RethinkDBSinkOptions sinkOptions) =>
            _sinkOptionsProvider.ConfigureSinkOptions(config, sinkOptions);
    }
}
