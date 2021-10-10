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

using RethinkDb.Driver.Net;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Sinks.RethinkDB
{
    /// <summary>
    /// Writes log events as json nodes in a collection of RethinkDB database.
    /// </summary>
    public class RethinkDBSink : IBatchedLogEventSink
    {
        private readonly string _hostName;
        private readonly int _port;
        private readonly string _databaseName;
        private readonly string _tableName;
        private readonly string _indexName;
        private readonly bool _useProps;
        private static readonly RethinkDb.Driver.RethinkDB R = RethinkDb.Driver.RethinkDB.R;
        private readonly Connection _connection;

        /// <summary>
        /// A reasonable default for the number of events posted in each batch.
        /// </summary>
        public const int DefaultBatchPostingLimit = 50;

        /// <summary>
        /// A reasonable default time to wait between checking for event batches.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2);

        /// <summary>
        /// 
        /// </summary>
        public static readonly bool DefaultEagerlyEmitFirstEvent = true;

        /// <summary>
        /// Default queue limit for batch posting.
        /// </summary>
        public static readonly int? DefaultQueueLimit = null;

        /// <summary>
        /// The default hostname for the RethinkDb connecton.
        /// </summary>
        public static readonly string DefaultHostName = "localhost";

        /// <summary>
        /// Default port number for the RethinkDB connection.
        /// </summary>
        public static readonly int DefaultPort = 28015;

        /// <summary>
        /// The default name for the logging database.
        /// </summary>
        public static readonly string DefaultDbName = "logging";

        /// <summary>
        /// The default name for the log table.
        /// </summary>
        public static readonly string DefaultTableName = "log";

        /// <summary>
        /// The default name for the index.
        /// </summary>
        public static readonly string DefaultIndexName = "idx";

        /// <summary>
        /// Set as true if you would like to insert Props dictionary.
        /// </summary>
        public static readonly bool DefaultUseProps = true;


        /// <summary>
        /// Construct a sink posting to the specified database.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <param name="batchPostingLimit"></param>
        /// <param name="period"></param>
        /// <param name="hostname"></param>
        public RethinkDBSink(string hostname, int port, string databaseName, string tableName, string indexName, bool useProps)
        {
            _hostName = hostname;
            _port = port;
            _databaseName = databaseName;
            _tableName = tableName;
            _indexName = indexName;
            _useProps = useProps;
            _connection = EnsureConnection(_connection);
            EnsureDatabaseAndTable();
        }

        private void EnsureDatabaseAndTable()
        {
            var dbExists = R.DbList().Contains(_databaseName).Run(_connection);
            if (!dbExists)
                R.DbCreate(_databaseName).Run(_connection);

            var tableExists = R.Db(_databaseName).TableList().Contains(_tableName).Run(_connection);
            if (!tableExists)
                R.Db(_databaseName).TableCreate(_tableName).Run(_connection);

            var indexExists = R.Db(_databaseName).Table(_tableName).IndexList().Contains(_indexName).Run(_connection);
            if (!indexExists)
            {
                R.Db(_databaseName).Table(_tableName).IndexCreate(_indexName, row => R.Array(row.G("Timestamp"), row.G("SequentialId"))).Run(_connection);
                R.Db(_databaseName).Table(_tableName).IndexWait(_indexName).Run(_connection);
            }
        }

        private Connection EnsureConnection(Connection conn)
        {
            if (conn == null)
                conn = R.Connection().Hostname(_hostName).Port(_port).Connect();

            if (!conn.Open)
                conn.Reconnect();

            return conn;
        }

        /// <summary>
        /// Emit a batch of log events, running asynchronously.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        public async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            EnsureConnection(_connection);

            var rethinkDbLogEvents = events.Select(x => new RethinkDbLogEvent()
            {
                Id = Guid.NewGuid(),
                SequentialId = x.Properties.ContainsKey("SequentialId") ? ((ScalarValue)x.Properties["SequentialId"]).Value : -1,
                Timestamp = x.Timestamp,
                Message = x.RenderMessage(),
                MessageTemplate = x.MessageTemplate.Text,
                Level = x.Level,
                Exception = x?.Exception?.ToString(),
                Props = _useProps == true ? SetProps(x) : null,
                ContextName = x.GetScalarValueObject() != null ? x.GetScalarValueObject().GetType().Name : null,
                ContextData = x.GetScalarValueObject()
            });

            await R.Db(_databaseName).Table(_tableName).Insert(rethinkDbLogEvents).RunAsync(_connection);
        }

        /// <summary>
        /// Called upon batchperiod if no data is in batch. Not used by this sink.
        /// </summary>
        /// <returns></returns>
        public Task OnEmptyBatchAsync() =>
        #if NET452
             Task.FromResult(false);
        #else
            Task.CompletedTask;
        #endif

        private Dictionary<string, object> SetProps(LogEvent logEvent)
        {
            return logEvent.Properties.ToDictionary<KeyValuePair<string, LogEventPropertyValue>, string, object>(k => k.Key, v => v.Value);
        }
    }
}
