using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MovieTheater
{
    public class Startup
    {
        private MovieContext _context;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            ConfigureDbContext(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var excHandler = context.Features.Get<IExceptionHandlerFeature>();
                    if (context.Request.GetTypedHeaders().Accept.Any(header => header.MediaType == "application/json"))
                    {
                        var jsonString = string.Format("{{\"error\":\"{0}\"}}", excHandler.Error.Message);
                        context.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
                        await context.Response.WriteAsync(jsonString, Encoding.UTF8);
                    }
                    else
                    {
                        //I haven't figured out a better way of signally ExceptionHandlerMiddleware that we can't handle the exception
                        //But this will do the trick of letting the other error handlers to intervene
                        //as the ExceptionHandlerMiddleware class will swallow this exception and rethrow the original one
                        throw excHandler.Error;
                    }
                });
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<MovieContext>();
            // 'scoped' in ASP.NET means "per HTTP request"
            services.AddScoped<MovieContext>();

            services.AddMvc()
             .AddJsonOptions(
                   options => options.SerializerSettings.ReferenceLoopHandling
                       = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }
    }
}
