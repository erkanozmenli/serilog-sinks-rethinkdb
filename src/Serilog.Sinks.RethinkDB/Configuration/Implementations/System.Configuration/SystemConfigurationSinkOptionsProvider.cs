using Serilog.Configuration;
using System;
using System.Globalization;

namespace Serilog.Sinks.RethinkDB.Configuration
{
    internal class SystemConfigurationSinkOptionsProvider : ISystemConfigurationSinkOptionsProvider
    {
        public RethinkDBSinkOptions ConfigureSinkOptions(RethinkDBConfigurationSection config, RethinkDBSinkOptions sinkOptions)
        {
            ReadSettings(config, sinkOptions);
            return sinkOptions;
        }

        private static void ReadSettings(RethinkDBConfigurationSection config, RethinkDBSinkOptions sinkOptions)
        {
            SetProperty.IfProvided<string>(config.Host, nameof(config.Host.Value), value => sinkOptions.Host = value);
            SetProperty.IfProvided<int>(config.Port, nameof(config.Port.Value), value => sinkOptions.Port = value);
            SetProperty.IfProvided<string>(config.Database, nameof(config.Database.Value), value => sinkOptions.Database = value);
            SetProperty.IfProvided<string>(config.Table, nameof(config.Table.Value), value => sinkOptions.Table = value);
            SetProperty.IfProvided<string>(config.Index, nameof(config.Index.Value), value => sinkOptions.Index = value);
            SetProperty.IfProvided<int>(config.BatchSizeLimit, nameof(config.BatchSizeLimit.Value), value => sinkOptions.BatchSizeLimit = value);
            SetProperty.IfProvided<string>(config.BatchPeriod, nameof(config.BatchPeriod.Value), value => sinkOptions.BatchPeriod = TimeSpan.FromSeconds(Convert.ToInt32(value)));
            SetProperty.IfProvided<bool>(config.EagerlyEmitFirstEvent, nameof(config.EagerlyEmitFirstEvent.Value), value => sinkOptions.EagerlyEmitFirstEvent = value);
            SetProperty.IfProvided<int>(config.QueueLimit, nameof(config.QueueLimit.Value), value => sinkOptions.QueueLimit = value);
            SetProperty.IfProvided<bool>(config.UseProps, nameof(config.UseProps.Value), value => sinkOptions.UseProps = value);
        }
    }
}
