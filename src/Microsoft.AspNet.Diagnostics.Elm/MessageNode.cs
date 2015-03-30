using Microsoft.Framework.Logging;
using System;

namespace Microsoft.AspNet.Diagnostics.Elm
{
    public class MessageNode : LogNode
    {
        public ActivityContext ActivityContext { get; set; }

        public string Name { get; set; }

        public object State { get; set; }

        public Exception Exception { get; set; }

        public string Message { get; set; }

        public LogLevel Severity { get; set; }

        public int EventID { get; set; }

        public DateTimeOffset Time { get; set; }
    }
}