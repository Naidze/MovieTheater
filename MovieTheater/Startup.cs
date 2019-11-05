using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieTheater.Helpers;
using MovieTheater.Models;
using MovieTheater.Repositories;
using MovieTheater.Services;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheater
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddIdentity<User, IdentityRole>(options =>
                    {
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.User.RequireUniqueEmail = true;
                    })
            .AddEntityFrameworkStores<MovieContext>()
            .AddDefaultTokenProviders();

            var symmetricSecuriytKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "naidzinavicius.com",
                        ValidAudience = "naidzinavicius.com",
                        IssuerSigningKey = symmetricSecuriytKey
                    };
                });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            ConfigureDbContext(services);
            AddScopes(services);
        }

        public void AddScopes(IServiceCollection services)
        {
            // Services
            services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<ICinemasService, CinemasService>();
            //services.AddScoped<IMoviesService, MoviesService>();
            //services.AddScoped<IQuotesService, QuotesService>();
            //services.AddScoped<IReviewsService, ReviewsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

            // Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            //services.AddScoped<ICinemasRepository, CinemasRepository>();
            //services.AddScoped<IMoviesRepository, InjuryRepository>();
            //services.AddScoped<IQuotesRepository, LeaderboardRepository>();
            //services.AddScoped<IReviewsRepository, LineupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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
            app.UseAuthentication();

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

            CreateRoles(serviceProvider).Wait();
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

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { UserRoleDefaults.Admin, UserRoleDefaults.User, UserRoleDefaults.Guest };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Here you could create a super user who will maintain the web app
            var poweruser = new User
            {
                UserName = Configuration["Admin:UserName"],
                Email = Configuration["Admin:UserEmail"],
            };
            //Ensure you have these values in your appsettings.json file
            string userPWD = Configuration["Admin:UserPassword"];
            var _user = await UserManager.FindByEmailAsync(Configuration["Admin:UserEmail"]);

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, UserRoleDefaults.Admin);
                }
            }
        }
    }
}
