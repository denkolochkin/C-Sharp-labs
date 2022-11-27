using PickyBrideProblem;
using PickyBrideProblem.Entity;
using PickyBrideProblem.Service;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Faker;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

public class MainClass
{

    public static void Main()
    {
        CreateHostBuilder().Build().Run();
    }

    public static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Princess>();

                services.AddScoped<Hall>();
                services.AddScoped<Friend>();
                services.AddScoped<ContenderGenerator>();
                services.AddScoped<DataBaseContext>();
                services.AddScoped<ContenderConsumer>();
            });
    }
}