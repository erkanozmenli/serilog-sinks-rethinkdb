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


using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Sinks.RethinkDB
{
    public class RethinkDBSinkOptions
    {
        public RethinkDBSinkOptions()
        {
            Host = RethinkDBSink.DefaultHostName;
            Port = RethinkDBSink.DefaultPort;
            Database = RethinkDBSink.DefaultDbName;
            Table = RethinkDBSink.DefaultTableName;
            Index = RethinkDBSink.DefaultIndexName;
            BatchSizeLimit = RethinkDBSink.DefaultBatchPostingLimit;
            BatchPeriod = RethinkDBSink.DefaultPeriod;
            EagerlyEmitFirstEvent = RethinkDBSink.DefaultEagerlyEmitFirstEvent;
            QueueLimit = RethinkDBSink.DefaultQueueLimit;
            UseProps = RethinkDBSink.DefaultUseProps;
        }

        internal RethinkDBSinkOptions(
            string host,
            int? port,
            string database,
            string table,
            string index,
            int? batchSizeLimit,
            TimeSpan? period,
            bool? eagerlyEmitFirstEvent,
            int? queueLimit,
            bool? useProps) : this()
        {
            Host = host ?? Host;
            Port = port ?? Port;
            Database = database ?? Database;
            Table = table ?? Table;
            Index = index ?? Index;
            BatchSizeLimit = batchSizeLimit ?? BatchSizeLimit;
            BatchPeriod = period ?? BatchPeriod;
            EagerlyEmitFirstEvent = eagerlyEmitFirstEvent ?? EagerlyEmitFirstEvent;
            QueueLimit = queueLimit ?? QueueLimit;
            UseProps = useProps ?? UseProps;
        }


        public string Host { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Table { get; set; }
        public string Index { get; set; }
        public int BatchSizeLimit { get; set; }
        public TimeSpan BatchPeriod { get; set; }
        public bool EagerlyEmitFirstEvent { get; set; }
        public int? QueueLimit { get; set; }
        public bool UseProps { get; set; }
    }
}
