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


using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Sinks.RethinkDB
{
    public static class Extensions
    {
        public static ScalarValueEnricher[] Log(this object input)
        {
            var arrScalarObjectEnricker = new ScalarValueEnricher[1];
            arrScalarObjectEnricker[0] = new ScalarValueEnricher("Foo", input);
            return arrScalarObjectEnricker;
        }

        public static object GetScalarValueObject(this LogEvent logEvent)
        {
            if (logEvent.Properties.TryGetValue("Foo", out LogEventPropertyValue logEventPropertyValue))
            {
                if (logEventPropertyValue is ScalarValue scalarValue)
                    return scalarValue.Value;
            }
            return null;
        }
    }
}
