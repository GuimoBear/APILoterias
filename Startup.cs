using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AngleSharp.Parser.Html;
using System.Net;
using MegaResult.Repositories;
using MegaResult.Services;

namespace MegaResult
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<CookieContainer>();
            services.AddScoped(prov => 
            {
                var container = new CookieAwareWebClient(prov.GetService<CookieContainer>());
                container.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                container.Headers[HttpRequestHeader.KeepAlive] = "300";
                //container.Proxy = new WebProxy("10.2.25.3", 3128);
                return container;
            });
            services.AddSingleton<HtmlParser>();
            services.AddSingleton<ConcursoRepository>();
            services.AddSingleton<JogoService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            appLifetime.ApplicationStopping.Register(OnStop);
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Resultado}/{action=Index}/{id?}");
            });
        }

        private void OnStop()
        {
            ConcursoRepository.Clear();
        }
    }
}
