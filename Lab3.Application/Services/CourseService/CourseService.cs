using AutoMapper;
using Lab3.Application.DTOs;
using Lab3.Application.Services.TenantProviderService;
using Lab3.Domain.Models;
using Lab3.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Application.Services.CourseService;

public class CourseService:ICourseService
{
    
    private readonly UmsDbContext _dbContext;
    private readonly ITenantProviderService _tenantProviderService;
    private readonly IMapper _mapper;
    
    public CourseService(UmsDbContext dbContext,ITenantProviderService tenantProviderService)
    {
        _dbContext = dbContext;
        _tenantProviderService = tenantProviderService;
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        });
        _mapper = new Mapper(config);
    }
    
    public async Task<List<Course>> GetAllCourses()
    {
        var tenantId = _tenantProviderService.GetTenantId();
 
        return await _dbContext.Courses
            .Where(e => e.BranchTenantId == tenantId)
            .ToListAsync();
    }
}