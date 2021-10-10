// Copyright 2021 Serilog Contributors 
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using Serilog.Configuration;
using Serilog.Events;
using System;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Sinks.RethinkDB;


namespace Serilog
{
    public static partial class LoggerConfigurationRethinkDBExtensions
    {
        /// <summary>
        /// The configuration section name for app.config or web.config configuration files.
        /// </summary>
        public const string AppConfigSectionName = "RethinkDBSettingsSection";

        public static LoggerConfiguration RethinkDB(
            this LoggerSinkConfiguration loggerConfiguration,
            string configSectionName = AppConfigSectionName,
            RethinkDBSinkOptions sinkOptions = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string host = null,
            int? port = null,
            string database = null,
            string table = null,
            string index = null,
            int? batchSizeLimit = null,
            TimeSpan? batchPeriod = null,
            bool? eagerlyEmitFirstEvent = null,
            int? queueLimit = null,
            bool? useProps = null)
        {
            ReadConfiguration(configSectionName, ref sinkOptions, new ApplySystemConfiguration());

            var batchingOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = batchSizeLimit ?? sinkOptions.BatchSizeLimit,
                Period = batchPeriod ?? sinkOptions.BatchPeriod,
                EagerlyEmitFirstEvent = eagerlyEmitFirstEvent ?? sinkOptions.EagerlyEmitFirstEvent,
                QueueLimit = queueLimit ?? sinkOptions.QueueLimit
            };

            var rethinkDbSink = new RethinkDBSink(
                    host ?? sinkOptions.Host,
                    port ?? sinkOptions.Port,
                    database ?? sinkOptions.Database,
                    table ?? sinkOptions.Table,
                    index ?? sinkOptions.Index,
                    useProps ?? sinkOptions.UseProps
            );

            var batchingSink = new PeriodicBatchingSink(rethinkDbSink, batchingOptions);
            return loggerConfiguration.Sink(batchingSink, restrictedToMinimumLevel);
        }

        public static LoggerConfiguration RethinkDB(
            this LoggerAuditSinkConfiguration loggerConfiguration,
            string configSectionName = AppConfigSectionName,
            RethinkDBSinkOptions sinkOptions = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string host = null,
            int? port = null,
            string database = null,
            string table = null,
            string index = null,
            bool? useProps = null)
        {
            ReadConfiguration(configSectionName, ref sinkOptions, new ApplySystemConfiguration());

            var rethinkDbSink = new RethinkDBAuditSink(
                    host ?? sinkOptions.Host,
                    port ?? sinkOptions.Port,
                    database ?? sinkOptions.Database,
                    table ?? sinkOptions.Table,
                    index ?? sinkOptions.Index,
                    useProps ?? sinkOptions.UseProps
            );

            return loggerConfiguration.Sink(rethinkDbSink, restrictedToMinimumLevel);
        }

        private static void ReadConfiguration(
            string configSectionName,
            ref RethinkDBSinkOptions sinkOptions,
            IApplySystemConfiguration applySystemConfiguration)
        {
            sinkOptions = sinkOptions ?? new RethinkDBSinkOptions();
            var serviceConfigSection = applySystemConfiguration.GetSinkConfigurationSection(configSectionName);
            if (serviceConfigSection != null)
                sinkOptions = applySystemConfiguration.ConfigureSinkOptions(serviceConfigSection, sinkOptions);
        }
    }
}
