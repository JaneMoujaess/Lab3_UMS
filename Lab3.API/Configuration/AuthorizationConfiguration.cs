using Microsoft.AspNetCore.Authorization;

namespace Lab3.API.Configuration;

public static class AuthorizationConfiguration
{
    public static void AddAuthorizationConfiguration(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPermission", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c =>
                        c.Type == "roleId" && c.Value == "1"
                    )
                )
            );
            options.AddPolicy("TeacherPermission", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c =>
                        c.Type == "roleId" && c.Value == "2"
                    )
                )
            );
            options.AddPolicy("StudentPermission", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c =>
                        c.Type == "roleId" && c.Value == "3"
                    )
                )
            );
        });
    }
}