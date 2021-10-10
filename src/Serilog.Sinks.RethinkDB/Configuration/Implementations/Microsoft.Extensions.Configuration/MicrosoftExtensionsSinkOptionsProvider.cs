using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Serilog.Sinks.RethinkDB
{
    internal class MicrosoftExtensionsSinkOptionsProvider : IMicrosoftExtensionsSinkOptionsProvider
    {
        public RethinkDBSinkOptions ConfigureSinkOptions(RethinkDBSinkOptions sinkOptions, IConfigurationSection config)
        {
            if (config == null)
            {
                return sinkOptions;
            }

            ReadSinkOptions(config, sinkOptions);
            
            return sinkOptions;
        }

        private static void ReadSinkOptions(IConfigurationSection config, RethinkDBSinkOptions sinkOptions)
        {
            SetProperty.IfNotNull<string>(config["Host"], val => sinkOptions.Host = val);
            SetProperty.IfNotNull<int>(config["Port"], val => sinkOptions.Port = val);
            SetProperty.IfNotNull<string>(config["Database"], val => sinkOptions.Database = val);
            SetProperty.IfNotNull<string>(config["Table"], val => sinkOptions.Table = val);
            SetProperty.IfNotNull<string>(config["Index"], val => sinkOptions.Index = val);
            SetProperty.IfNotNull<int>(config["BatchSizeLimit"], val => sinkOptions.BatchSizeLimit = val);
            SetProperty.IfNotNull<string>(config["BatchPeriod"], val => sinkOptions.BatchPeriod = TimeSpan.FromSeconds(Convert.ToInt32(val)));
            SetProperty.IfNotNull<bool>(config["EagerlyEmitFirstEvent"], val => sinkOptions.EagerlyEmitFirstEvent = val);
            SetProperty.IfNotNull<int>(config["QueueLimit"], val => sinkOptions.QueueLimit = val);
            SetProperty.IfNotNull<bool>(config["UseProps"], val => sinkOptions.UseProps = val);
        }
    }
}
