using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Sinks.RethinkDB
{
    internal class ApplyMicrosoftExtensionsConfiguration : IApplyMicrosoftExtensionsConfiguration
    {
        private readonly IMicrosoftExtensionsSinkOptionsProvider _sinkOptionsProvider;

        public ApplyMicrosoftExtensionsConfiguration() : this(
            new MicrosoftExtensionsSinkOptionsProvider())
        {
        }

        internal ApplyMicrosoftExtensionsConfiguration(
            IMicrosoftExtensionsSinkOptionsProvider sinkOptionsProvider)
        {
            _sinkOptionsProvider = sinkOptionsProvider;
        }

        public RethinkDBSinkOptions ConfigureSinkOptions(RethinkDBSinkOptions sinkOptions, IConfigurationSection config) =>
            _sinkOptionsProvider.ConfigureSinkOptions(sinkOptions, config);
    }
}
