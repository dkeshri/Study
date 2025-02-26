using ApiGateway.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiGateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiAuthentication(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetTokenSecret())),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    }
                );
            services.AddScoped<PrometheusMetricsMiddleware>();
        }
    }
}
