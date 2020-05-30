using FluentValidation.Results;
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
                   app.UseOcelot(new OcelotPipelineConfiguration
                   {
                       PreErrorResponderMiddleware = IdentityAuth.AuthIdToken
                   })
                   .Wait();
               })
               .Build()
               .Run();
        }

        private static OcelotPipelineConfiguration SetConfig()
        {
            var configuration = new OcelotPipelineConfiguration
            {
                PreErrorResponderMiddleware = async (ctx, next) =>
                {
                    var req = ctx.Request;

                    if (req.Headers["Authorization"][0] == "1")
                    {
                        var response = ctx.Response;
                        response.ContentType = "application/json";
                        response.StatusCode = 403;
                        response.Headers["Answer"] = "miss";

                        var strResult = "some miss";
                        await ctx.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(strResult));
                        
                        //var error = new UnauthenticatedError("Request for authenticated route was unauthenticated");
                        //ctx.Errors.Add(error);
                    }
                    else
                        await next.Invoke();
                }
            };

            return configuration;
        }

    }
}
