using Microsoft.Extensions.Configuration;

namespace Serilog.Sinks.RethinkDB
{
    public interface IMicrosoftExtensionsSinkOptionsProvider
    {
        RethinkDBSinkOptions ConfigureSinkOptions(RethinkDBSinkOptions sinkOptions, IConfigurationSection config);
    }
}
