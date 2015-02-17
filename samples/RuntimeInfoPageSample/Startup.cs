using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;

namespace RuntimeInfoPageSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebEncoders();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRequestServices();

            app.UseRuntimeInfoPage();

            app.Run(context =>
            {
                context.Response.StatusCode = 302;
                context.Response.Headers["Location"] = "/runtimeinfo";

                return Task.FromResult(0);
            });
        }
    }
}
