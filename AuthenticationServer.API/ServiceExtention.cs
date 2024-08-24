using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthenticationServer.API
{
    public static class ServiceExtention
    {
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWT");
            //var key = Environment.GetEnvironmentVariable("KEY");
            var key = "MYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEYMYKEY";
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
        }

        public static void ConfigureExeptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(e =>
            {
                e.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Log.Error($"Something went wrong in the {contextFeature.Error}");
                        await context.Response.WriteAsync(new Core.Models.Error
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = "Internal server error. Please try again later."
                        }.ToString());
                    }
                });
            });
        }
    }
}
