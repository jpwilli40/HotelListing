using AspNetCoreRateLimit;
using HotelListing.Configurations;
using HotelListing.Controllers.Data;
using HotelListing.IRepository;
using HotelListing.Repository;
using HotelListing.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing
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
            services.AddDbContext<DatabaseContext>(options =>           //Initiates the sql server connection defined in app.config
                options.UseSqlServer(Configuration.GetConnectionString("sqlConnection"))
            );

            services.AddMemoryCache();  //For Throttling
            services.ConfigureRateLimiting(); //--
            services.AddHttpContextAccessor(); // --

            services.AddResponseCaching();  //adds caching capabilities, moving this to ServiceExtensions called in method below (ConfigureHttpCacheHeaders  //use this if you want the age counter
            //services.ConfigureHttpCacheHeaders();   //use this for the extra caching headers (expiry, etc...)
            services.AddAuthentication();
            services.ConfigureIdentity();  //can be used to handle all of the commands (like below) instead of baking this file bigger
            services.ConfigureJWT(Configuration); //same as above, used to simplify this file from a bunch of configurations

            services.AddCors(o => {  //allows certian people to access your API
                o.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddAutoMapper(typeof(MapperInitializer));
            services.AddTransient<IUnitOfWork, UnitOfWork>(); //Transient - every time a new instance created, SCope - one for entire session, Singleton - just one instance
            //services.AddScoped<IAuthManager, AuthManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotellisting", Version = "v1" });
            });

            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile  //this adds global caching for data if desired.  If not just leave AddController param blank
                {
                    Duration = 120
                });
            }).AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.ConfigureVersioning();

            services.AddRazorPages();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotellisting v1"));

            app.ConfigureExceptionHandler();  //added for global error handling

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");  //calls CORS policy from above

            app.UseStaticFiles();
            app.UseResponseCaching();  //adds caching capability
            //app.UseHttpCacheHeaders();  //add back if you use the ConfigureHttpCacheHeaders in ServiceExtensions

            //app.UseIpRateLimiting();  //throttling stuff

            app.UseRouting();

            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
