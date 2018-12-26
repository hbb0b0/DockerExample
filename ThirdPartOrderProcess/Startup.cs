using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using ThirdPartOrderProcess.Common;
using ThirdPartOrderProcess.Job;
using ThirdPartOrderProcess.DB;
namespace ThirdPartOrderProcess
{
    public class Startup
    {
        private Timer m_stateTimer;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            //此方法获取不到配置 Configuration.GetConnectionString("ConnectionStrings:DefaultConnection")
            string str= Configuration.GetSection("ConnectionStrings").GetValue<string>("DefaultConnection");
            DBHelper.ConnectionString = str;
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            
            loggerFactory.AddDebug();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });
             
             var orderNotice = new XiaMenQBiNotice();
              m_stateTimer = new Timer(orderNotice.Notice, 
                                   null, 1000, 2000);

            
        }
    }
}