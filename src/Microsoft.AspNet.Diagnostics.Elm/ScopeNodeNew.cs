using System;
using System.Collections.Generic;

namespace Microsoft.AspNet.Diagnostics.Elm
{
    public class ScopeNodeNew : LogNode
    {
        public ScopeNodeNew Parent { get; set; }

        public List<LogNode> Children { get; set; } = new List<LogNode>();

        public object State { get; set; }

        public string Name { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }
    }
}