using System;
using System.Diagnostics;
using System.Threading;

using JokesWeb.Clients;
using JokesWeb.Clients.Rest;

using JokesWebService;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Runtime;

namespace JokesWeb
{
    public class Program
    {
        private const string ENV_FABRIC_APPLICATIONNAME = "Fabric_ApplicationName";

        public static void Main(
            string[] args)
        {
            var host = BuildWebHost(args);

            if (IsServiceFabricEnvironment(host))
            {
                try
                {
                    ServiceRuntime.RegisterServiceAsync(
                            "JokesWebServiceType",
                            context => new JokesWebService.JokesWebService(context))
                       .GetAwaiter()
                       .GetResult();

                    ServiceEventSource.Current.ServiceTypeRegistered(
                        Process.GetCurrentProcess().Id,
                        typeof(JokesWebService.JokesWebService).Name);

                    Thread.Sleep(Timeout.Infinite);
                }
                catch (Exception e)
                {
                    ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                    throw;
                }
            }

            host.Run();
        }
        public static IWebHost BuildWebHost(
            string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .ConfigureServices(
                    services =>
                    {
                        services.AddTransient<IJokesApiClient, HttpJokesApiClient>();
                    })
               .Build();
        }

        private static bool IsServiceFabricEnvironment(
            IWebHost webHost)
        {
            return webHost.Services.GetService<IConfiguration>() is IConfiguration cfg && cfg[ENV_FABRIC_APPLICATIONNAME] != null;
        }
    }
}