using Serilog.Configuration;

namespace Serilog.Sinks.RethinkDB
{
    internal interface IApplySystemConfiguration
    {
        RethinkDBConfigurationSection GetSinkConfigurationSection(string configurationSectionName);
        RethinkDBSinkOptions ConfigureSinkOptions(RethinkDBConfigurationSection config, RethinkDBSinkOptions sinkOptions);
    }
}
