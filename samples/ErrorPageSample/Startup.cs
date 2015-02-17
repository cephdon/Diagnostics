using System;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;

namespace ErrorPageSample
{
    public class Startup
    {
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddWebEncoders();
        //}

        public void Configure(IApplicationBuilder app)
        {
           // app.UseRequestServices();

            app.UseErrorPage();
            app.Run(context =>
            {
                throw new Exception(string.Concat(
                    "Demonstration exception. The list:", "\r\n", 
                    "New Line 1", "\n", 
                    "New Line 2", Environment.NewLine, 
                    "New Line 3"));
            });
        }
    }
}