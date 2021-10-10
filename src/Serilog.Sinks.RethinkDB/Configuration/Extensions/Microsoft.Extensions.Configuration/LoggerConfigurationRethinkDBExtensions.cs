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


using Microsoft.Extensions.Configuration;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Sinks.RethinkDB;
using System;


namespace Serilog
{
    public static partial class LoggerConfigurationRethinkDBExtensions
    {
        /// <summary>
        /// The configuration settings file.
        /// </summary>
        public const string AppSettingsJsonFileName = "appsettings.json";

        /// <summary>
        /// The configuration section name for settings file.
        /// </summary>
        public const string AppConfigSectionName = "Serilog:SinkOptions";


        public static LoggerConfiguration RethinkDB(
        this LoggerSinkConfiguration loggerConfiguration,
        string appSettingsJsonFileName = AppSettingsJsonFileName,
        string appConfigSectionName = AppConfigSectionName,
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
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(appSettingsJsonFileName, optional: false, reloadOnChange: true)
                .Build();
            var sinkOptionsSection = configuration.GetSection(appConfigSectionName);

            ReadConfiguration(ref sinkOptions, sinkOptionsSection);

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
            string appSettingsJsonFileName = AppSettingsJsonFileName,
            string appConfigSectionName = AppConfigSectionName,
            RethinkDBSinkOptions sinkOptions = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string host = null,
            int? port = null,
            string database = null,
            string table = null,
            string index = null,
            bool? useProps = null)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(appSettingsJsonFileName, optional: false, reloadOnChange: true)
               .Build();
            var sinkOptionsSection = configuration.GetSection(appConfigSectionName);

            ReadConfiguration(ref sinkOptions, sinkOptionsSection);

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
            ref RethinkDBSinkOptions sinkOptions,
            IConfigurationSection sinkOptionsSection)
        {
            sinkOptions = sinkOptions ?? new RethinkDBSinkOptions();
            IApplyMicrosoftExtensionsConfiguration microsoftExtensionsConfiguration = new ApplyMicrosoftExtensionsConfiguration();
            sinkOptions = microsoftExtensionsConfiguration.ConfigureSinkOptions(sinkOptions, sinkOptionsSection);
        }
    }
}
