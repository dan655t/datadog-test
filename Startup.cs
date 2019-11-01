using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DatadogTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }            

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // PrintVar("CORECLR_ENABLE_PROFILING");
            // PrintVar("CORECLR_PROFILER");
            // PrintVar("CORECLR_PROFILER_PATH");
            // PrintVar("DD_INTEGRATIONS");
            // PrintVar("DD_DOTNET_TRACER_HOME");
            // PrintVar("DD_TRACE_ANALYTICS_ENABLED");
            // PrintVar("TRACING_HOST");
            // PrintVar("TRACING_BATCH_SIZE");
            // PrintVar("TRACING_SYNC_THRESHOLD");

            // System.Console.WriteLine(File.Exists("/opt/datadog/Datadog.Trace.ClrProfiler.Native.so"));            
        }

        private void PrintVar(string variable)
            => Console.WriteLine($"{variable}: {Environment.GetEnvironmentVariable(variable)}");
    }
}
