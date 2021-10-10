using Serilog.Configuration;

namespace Serilog.Sinks.RethinkDB.Configuration
{
    internal interface ISystemConfigurationSinkOptionsProvider
    {
        RethinkDBSinkOptions ConfigureSinkOptions(RethinkDBConfigurationSection config, RethinkDBSinkOptions sinkOptions);
    }
}
