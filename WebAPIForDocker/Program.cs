using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebAPIForDocker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            //var host = new WebHostBuilder()
            //   .UseKestrel()
            //   .UseUrls("http://*:5000")
            //   //7030 用于215模拟供货商接口
            //   //.UseUrls("http://*:7030")
            //   .UseContentRoot(Directory.GetCurrentDirectory())
            //   .UseIISIntegration()
            //   .UseStartup<Startup>()
            //   .Build();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
               .UseUrls("http://*:5001")
             .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();
    }
}
