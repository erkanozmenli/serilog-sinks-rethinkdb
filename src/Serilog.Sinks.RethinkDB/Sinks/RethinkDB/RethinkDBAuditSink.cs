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
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Serilog.Sinks.RethinkDB
{
    /// <summary>
    /// Writes log events as rows in a table of MSSqlServer database using Audit logic, meaning that each row is synchronously committed
    /// and any errors that occur are propagated to the caller.
    /// </summary>
    public class RethinkDBAuditSink : ILogEventSink
    {
        private readonly string _hostName;
        private readonly int _port;
        private readonly string _databaseName;
        private readonly string _tableName;
        private readonly string _indexName;
        private readonly bool _useProps;
        private static readonly RethinkDb.Driver.RethinkDB R = RethinkDb.Driver.RethinkDB.R;


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
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <param name="indexName"></param>
        /// <param name="useProps"></param>
        public RethinkDBAuditSink(string hostname, int port, string databaseName, string tableName, string indexName, bool useProps)
        {
            _hostName = hostname;
            _port = port;
            _databaseName = databaseName;
            _tableName = tableName;
            _indexName = indexName;
            _useProps = useProps;
            EnsureDatabaseAndTable();
        }

        private void EnsureDatabaseAndTable()
        {
            using (var conn = R.Connection().Hostname(_hostName).Port(_port).Connect())
            {
                var dbExists = R.DbList().Contains(_databaseName).Run(conn);
                if (!dbExists)
                    R.DbCreate(_databaseName).Run(conn);

                var tableExists = R.Db(_databaseName).TableList().Contains(_tableName).Run(conn);
                if (!tableExists)
                    R.Db(_databaseName).TableCreate(_tableName).Run(conn);

                var indexExists = R.Db(_databaseName).Table(_tableName).IndexList().Contains(_indexName).Run(conn);
                if (!indexExists)
                {
                    R.Db(_databaseName).Table(_tableName).IndexCreate(_indexName, row => R.Array(row.G("Timestamp"), row.G("SequentialId"))).Run(conn);
                    R.Db(_databaseName).Table(_tableName).IndexWait(_indexName).Run(conn);
                }
            }
        }

        
        /// <summary>
        /// Emit the provided log event to the sink.
        /// </summary>
        /// <param name="logEvent"></param>
        public void Emit(LogEvent logEvent)
        {
            using (var conn = R.Connection().Hostname(_hostName).Port(_port).Connect()) 
            {
                var rethinkDbLogEvent = new RethinkDbLogEvent()
                {
                    Id = Guid.NewGuid(),
                    SequentialId = logEvent.Properties.ContainsKey("SequentialId") ? ((ScalarValue)logEvent.Properties["SequentialId"]).Value : -1,
                    Timestamp = logEvent.Timestamp,
                    Message = logEvent.RenderMessage(),
                    MessageTemplate = logEvent.MessageTemplate.Text,
                    Level = logEvent.Level,
                    Exception = logEvent?.Exception?.ToString(),
                    Props = _useProps == true ? SetProps(logEvent) : null,
                    ContextName = logEvent.GetScalarValueObject() != null ? logEvent.GetScalarValueObject().GetType().Name : null,
                    ContextData = logEvent.GetScalarValueObject()
                };

                R.Db(_databaseName).Table(_tableName).Insert(rethinkDbLogEvent).Run(conn);
            }
        }

        private Dictionary<string, object> SetProps(LogEvent logEvent)
        {
            return logEvent.Properties.ToDictionary<KeyValuePair<string, LogEventPropertyValue>, string, object>(k => k.Key, v => v.Value);
        }        
    }
}
