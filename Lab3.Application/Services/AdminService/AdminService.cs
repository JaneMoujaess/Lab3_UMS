using System.Security.Claims;
using AutoMapper;
using Lab3.Application.DTOs;
using Lab3.Application.Services.UserIdentifierService;
using Lab3.Domain.Models;
using Lab3.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;

namespace Lab3.Application.Services.AdminService;

public class AdminService : IAdminService
{
    private readonly UmsDbContext _dbContext;
    private readonly IUserIdentifierService _userIdentifierService;
    private readonly IMapper _mapper;
    private readonly ILogger<AdminService> _logger;

    public AdminService(UmsDbContext dbContext,IUserIdentifierService userIdentifierService,ILogger<AdminService> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userIdentifierService = userIdentifierService;
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        });
        _mapper = new Mapper(config);
    }
    
    //Used CourseDTO instead of Course model entity to avoid circular references and hide superfluous information
    public async Task<List<Course>> CreateCourse(CourseDtoRequest newCourse)
    {
        _logger.LogInformation("test");
        var tenantId = await _userIdentifierService.GetTenantId();
        _logger.LogInformation("tenantId="+tenantId);
        var course = _mapper.Map<Course>(newCourse);
        course.BranchTenantId = tenantId;
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();
        return await _dbContext.Courses.ToListAsync();
    }
}