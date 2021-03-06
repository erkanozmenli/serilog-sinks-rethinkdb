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

using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Serilog.Sinks.RethinkDB
{
    public class SequentialIdEnricher : ILogEventEnricher
    {
        private long _currentId = 0;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null) throw new ArgumentNullException("logEvent");

            var incrementedId = Interlocked.Increment(ref _currentId);
            logEvent.AddPropertyIfAbsent(new LogEventProperty("SequentialId", new ScalarValue(incrementedId)));
        }
    }
}
