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


using Newtonsoft.Json;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Serilog.Sinks.RethinkDB
{
    /// <summary>
    /// A log event.
    /// </summary>
    [DataContract]
    public class RethinkDbLogEvent
    {
        /// <summary>
        /// The Id of the event.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid Id;

        /// <summary>
        /// The time at which the event occurred.
        /// </summary>
        [JsonProperty]
        public DateTimeOffset Timestamp;

        /// <summary>
        /// The level of the event.
        /// </summary>
        [JsonProperty]
        public LogEventLevel Level;

        /// <summary>
        /// The message describing the event
        /// </summary>
        [JsonProperty]
        public string Message;

        /// <summary>
        /// The message template describing the event.
        /// </summary>
        [JsonProperty]
        public string MessageTemplate;

        /// <summary>
        /// Properties associated with the event, including those presented in <see cref="LogEvent.MessageTemplate"/>.
        /// </summary>
        [JsonProperty]
        public Dictionary<string, object> Props;

        /// <summary>
        /// An exception associated with the event, or null.
        /// </summary>
        [JsonProperty]
        public string Exception;

        /// <summary>
        /// Name of attached context object.
        /// </summary>
        [JsonProperty]
        public string ContextName;

        /// <summary>
        /// Content of attached context object.
        /// </summary>
        [JsonProperty]
        public Object ContextData;

        /// <summary>
        /// A helper for accurate sorting (Use auto-created index (idx) for ordering).
        /// </summary>
        [JsonProperty]
        public object SequentialId { get; set; }
    }
}
