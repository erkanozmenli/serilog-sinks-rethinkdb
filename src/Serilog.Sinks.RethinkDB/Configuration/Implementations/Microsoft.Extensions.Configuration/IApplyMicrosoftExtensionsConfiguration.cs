using Microsoft.Extensions.Configuration;

namespace Serilog.Sinks.RethinkDB
{
    internal interface IApplyMicrosoftExtensionsConfiguration
    {
       RethinkDBSinkOptions ConfigureSinkOptions(RethinkDBSinkOptions sinkOptions, IConfigurationSection config);
    }
}
