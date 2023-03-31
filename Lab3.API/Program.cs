using System.Security.Claims;
using dotenv.net;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Lab3.API.Configuration;
using Lab3.API.Middlewares;
using Lab3.Application.Middlewares;
using Lab3.Application.Services.AdminService;
using Lab3.Application.Services.CourseService;
using Lab3.Application.Services.PublisherService;
using Lab3.Application.Services.StudentService;
using Lab3.Application.Services.TeacherService;
using Lab3.Application.Services.UserIdentifierService;
using Lab3.Infrastructure;
using Lab3.Persistence;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSingleton<IPublisherService, PublisherService>();


//Odata configuration
builder.Services.AddOdataConfiguration();


//Firebase
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile("./firebase-config.json")
});

builder.Services.AddAuthorizationConfiguration();

//JWT authentication configuration
builder.Services.AddJWTAuthenticationConfiguration();
    

//Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuthorizationUIConfiguration(); // UI for authorization in swagger

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