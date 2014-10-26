using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Voodoo;

namespace StringReplicator.Core
{
    public static class Program
    {
        private static string url = "http://localhost:{0}";

        public static void Main(string[] args)
        {
            try
            {
                var options = new StartOptions();
                var port = ConfigurationManager.AppSettings["port"] ?? "9998";
                url = string.Format(url, port);
                options.Urls.Add(url);
                options.ServerFactory = "Microsoft.Owin.Host.HttpListener";
                options.AppStartup = "Hosting.Startup";


                using (WebApp.Start<Startup>(options))
                {
                    IoNic.ShellExecute(string.Format("{0}/index.html", url));
                    Console.WriteLine("Server running on {0}", url);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is HttpListenerException)
                {
                    Console.WriteLine(
                        "Failed to start web server.  You may wish to change the port in the configuration file: {0}",
                        ex.InnerException.Message);
                    Console.ReadLine();
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseFileServer(new FileServerOptions
            {
                FileSystem = new PhysicalFileSystem("Web"),
                EnableDefaultFiles = true,
                EnableDirectoryBrowsing = true
            });
            var apiConfig = new HttpConfiguration();
            apiConfig.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
            var jsonSettings = apiConfig.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            app.UseWebApi(apiConfig);
        }
    }
}