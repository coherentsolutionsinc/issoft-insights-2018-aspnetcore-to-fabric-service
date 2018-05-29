using System;
using System.Diagnostics;
using System.Threading;

using JokesApi.Impl;
using JokesApi.Impl.Services.EntityFramework;

using JokesApiService;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Runtime;

namespace JokesApi
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
                            "JokesApiServiceType",
                            context => new JokesApiService(context))
                       .GetAwaiter()
                       .GetResult();

                    ServiceEventSource.Current.ServiceTypeRegistered(
                        Process.GetCurrentProcess().Id,
                        typeof(JokesApiService).Name);

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
                        services.AddDbContext<DbJokesContext>(
                            options =>
                            {
                                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Jokes-Db;Trusted_Connection=True;");
                            });

                        services.AddTransient<IJokesService, DbJokesService>();
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