using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Lab3.API.Configuration;

public static class SwaggerAuthorizationUIConfiguration
{
    public static void AddSwaggerAuthorizationUIConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
    }
}