using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Lab2.Application.Middlewares;
using Lab2.Application.Services.AdminService;
using Lab2.Domain.Models;
using Lab2.Infrastructure;
using Lab2.Persistence;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//DBcontext
builder.Services.AddDbContext<UmsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

//My services
builder.Services.AddScoped<IFirebaseAuthService,FirebaseAuthService>();
builder.Services.AddTransient<IAdminService, AdminService>();

//Odata
static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Course>("Courses");
    return builder.GetEdmModel();
}
builder.Services.AddControllers().AddOData(options => options
    .AddRouteComponents("odata", GetEdmModel())
    .Select()
    .Filter()
    .OrderBy()
    .SetMaxTop(20)
    .Count()
    .Expand()
);


//Firebase
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("./firebase-config.json")
});

builder.Services.AddAuthorization();
//JWT handling
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

//Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});// UI for authorization in swagger

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//My middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();