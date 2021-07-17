using HotelListing.Controllers.Data;
using HotelListing.DTOModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using AspNetCoreRateLimit;

namespace HotelListing
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<APIUser>(q => q.User.RequireUniqueEmail = true);

            builder = new Microsoft.AspNetCore.Identity.IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)  //IConfiguration gives access to the appsettings configuration, setting this here so it is not inside appsettings due to sensitive purposes
        {
            var jwtSettings = Configuration.GetSection("JWT");
            var key = Environment.GetEnvironmentVariable("Justinskey");

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });
        }
        
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)  //built for generic global error handling
        {
            app.UseExceptionHandler(error => {
               error.Run(async context => {
                   context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;
                   context.Response.ContentType = "application/json";
                   var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                   if(contextFeature != null)
                   {
                       Log.Error($"Something Went Wrong in the {contextFeature.Error}");

                       await context.Response.WriteAsync(new Error
                       {
                           StatusCode = context.Response.StatusCode,
                           Message = "INternal Server Error.  Please Try Again Later."
                       }.ToString());
                   }
               });
           });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;  //reports in the header what version api
                opt.AssumeDefaultVersionWhenUnspecified = true;  //uses default APO defined below if none specified
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                //opt.ApiVersionReader = new HeaderApiVersionReader("api-version");  //used as another way to add api versioning to the header
            });
        }

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddResponseCaching();  //sets the caching here globally instead of running these two in startup
            services.AddHttpCacheHeaders(
                (expirationOpt) =>
                {
                    expirationOpt.MaxAge = 70;
                    expirationOpt.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
                },
                (validationOpt) =>
                {
                    validationOpt.MustRevalidate = true;
                }
            );
        }

        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 1,
                    Period = "1s"
                }
            };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}
