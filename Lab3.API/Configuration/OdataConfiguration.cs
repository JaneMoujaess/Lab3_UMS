using Lab3.Domain.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Lab3.API.Configuration;

public static class OdataConfiguration
{
    static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EntitySet<Course>("Courses");
        return builder.GetEdmModel();
    }
    public static void AddOdataConfiguration(this IServiceCollection services)
    {
        services.AddControllers().AddOData(options => options
            .AddRouteComponents("odata", GetEdmModel())
            .Select()
            .Filter()
            .OrderBy()
            .SetMaxTop(20)
            .Count()
            .Expand()
        );
    }
}