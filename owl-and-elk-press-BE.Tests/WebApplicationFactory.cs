using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using owl_and_elk_press_BE.Endpoints;

namespace owl_and_elk_press_BE.Tests;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.SetBasePath(AppContext.BaseDirectory);
        });
        
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IStripeService));
            if (descriptor != null)
            {
                services.Remove(descriptor); 
            }

            services.AddScoped<IStripeService, FakeStripeService>();
        });
    }
}
