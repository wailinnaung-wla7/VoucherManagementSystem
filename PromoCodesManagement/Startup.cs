using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodesManagement.Helpers;
using PromoCodesManagement.Jobs;
using PromoCodesManagement.PromoContext;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodesManagement
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<storedbContext>(options =>
            options.UseMySQL(Configuration.GetConnectionString("storedb")));

            services.AddQuartz(q =>
            {

                q.UseMicrosoftDependencyInjectionJobFactory();

                var appSettingsSection = Configuration.GetSection("Quartz");
                services.Configure<QuartzSetting>(appSettingsSection);

                var appSettings = appSettingsSection.Get<QuartzSetting>();
                var cron = appSettings.PromoCodeGenerateJob;


                var jobKey = new JobKey("PromoCodeGenerateJob");


                q.AddJob<PromoCodeGenerateJob>(opts => opts.WithIdentity(jobKey));


                q.AddTrigger(opts => opts
                    .ForJob(jobKey) 
                    .WithIdentity("PromoCodeGenerateJob-trigger") 
                    .WithCronSchedule(cron)); 
            });

            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
