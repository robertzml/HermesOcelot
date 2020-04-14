using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System.IO;
using System.Text;

namespace HermesOcelot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new OcelotPipelineConfiguration
            {
                PreErrorResponderMiddleware = async (ctx, next) =>
                {
                    var req = ctx.HttpContext.Request;

                    if (req.Headers["Authorization"][0] == "1")
                    {
                        var response = ctx.HttpContext.Response;
                        response.ContentType = "application/json";
                        response.StatusCode = 403;

                        string strtxt = "some miss";
                        byte[] bytetxt = Encoding.UTF8.GetBytes(strtxt);
                        Stream memstream = new MemoryStream();
                        memstream.Write(bytetxt, 0, bytetxt.Length);

                        response.Body = memstream;
                        response.Headers["Answer"] = "miss";
                        // await response.CompleteAsync();

                        var error = new UnauthenticatedError(
                            $"Request for authenticated route {ctx.HttpContext.Request.Path} by {ctx.HttpContext.User.Identity.Name} was unauthenticated");

                        ctx.Errors.Add(error);
                        //await next.Invoke();
                    }
                    else
                        await next.Invoke();
                }
            };

            new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                       .AddJsonFile("ocelot.json")
                       .AddEnvironmentVariables();
               })
               .ConfigureServices(s =>
               {
                   s.AddOcelot();//.AddConsul();
               })
               .ConfigureLogging((hostingContext, logging) =>
               {
                   //add your logging
               })
               .UseIISIntegration()
               .Configure(app =>
               {
                   app.UseOcelot(configuration).Wait();
               })
               .Build()
               .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
