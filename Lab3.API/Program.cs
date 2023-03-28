using System.Security.Claims;
using dotenv.net;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Lab3.Application.Middlewares;
using Lab3.Application.Services.AdminService;
using Lab3.Application.Services.CourseService;
using Lab3.Application.Services.StudentService;
using Lab3.Application.Services.TeacherService;
using Lab3.Application.Services.UserIdentifierService;
using Lab3.Domain.Models;
using Lab3.Infrastructure;
using Lab3.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

//Loading environmental variables
//DotEnv.Load();

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);


//DBcontext
builder.Services.AddDbContext<UmsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

//My services
builder.Services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<ITeacherService, TeacherService>();
builder.Services.AddScoped<IUserIdentifierService, UserIdentifierService>();
builder.Services.AddScoped<ICourseService, CourseService>();
//builder.Services.AddScoped<IMessageProducer, StudentService>();

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
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("./firebase-config.json")
});

builder.Services.AddAuthorization(options =>
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
}); // UI for authorization in swagger

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//My middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<UserIdentificationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();