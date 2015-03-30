using System.Collections.Generic;
using Microsoft.AspNet.Http;

namespace Microsoft.AspNet.Diagnostics.Elm.Views
{
    public class LogPageModelNew
    {
        public IEnumerable<ActivityContextNew> Activities { get; set; }

        public ViewOptions Options { get; set; }

        public PathString Path { get; set; }
    }
}