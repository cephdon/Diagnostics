using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;

namespace WelcomePageSample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiagnostics();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRequestServices();

            app.UseWelcomePage();
        }
    }
}
