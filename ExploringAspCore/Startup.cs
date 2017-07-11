using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace ExploringAspCore
{
    public class Startup
    {
        private readonly IConfigurationRoot configuration;// bring in root 
        public Startup(IHostingEnvironment env)
        {
            
            //custom configuration
                 configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(env.ContentRootPath + "/config.json")
                .AddJsonFile(env.ContentRootPath + "/config.development.json",true)//optional 
                .Build();
        }
       
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {
                EnableDeveloperExeptions = configuration.GetValue<bool>("FeatureToggles:EnableDevelopmentExeption")
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, FeatureToggles features)
        {
           
            loggerFactory.AddConsole();

            app.UseExceptionHandler("/error.html");

            //if (configuration.GetValue<bool>("FeatureToggles:EnableDevelopmentExeption"))
            if(features.EnableDeveloperExeptions)
            {
                app.UseDeveloperExceptionPage();
            }
            //middle ware takes Use takes two args next() calls other app.run expression
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("invalid"))//starts with fuction exmple
                
                    throw new Exception("ERROR");

                    await next();
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseFileServer();//serves static files from wwwroot
        }
    }
}
