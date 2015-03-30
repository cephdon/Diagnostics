using System;

namespace Microsoft.AspNet.Diagnostics.Elm
{
    public class ActivityContextNew
    {
        public Guid Id { get; set; }

        public HttpInfo HttpInfo { get; set; }

        public LogNode Root { get; set; }

        public DateTimeOffset Time { get; set; }

        public bool IsCollapsed { get; set; }
    }
}