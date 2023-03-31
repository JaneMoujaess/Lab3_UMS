using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Lab3.API.Configuration;

public static class JWTAuthenticationConfiguration
{
    public static void AddJWTAuthenticationConfiguration(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
        
                options.Authority = "https://securetoken.google.com/lab2-7a5dd";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/lab2-7a5dd",
                    ValidateAudience = true,
                    ValidAudience = "lab2-7a5dd",
                    ValidateLifetime = true
                };
            });
    }
}