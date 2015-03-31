using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.WebEncoders;

namespace ElmPageSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddElm();

            services.ConfigureElm(elmOptions =>
            {
                elmOptions.Filter = (loggerName, loglevel) => true;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseElmPage();

            app.UseElmCapture();

            app.UseMiddleware<TestMiddleware>();
        }
    }
}
