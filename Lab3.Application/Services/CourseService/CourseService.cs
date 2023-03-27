using AutoMapper;
using Lab3.Application.DTOs;
using Lab3.Application.Services.UserIdentifierService;
using Lab3.Domain.Models;
using Lab3.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Application.Services.CourseService;

public class CourseService:ICourseService
{
    
    private readonly UmsDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserIdentifierService _userIdentifierService;

    public CourseService(UmsDbContext dbContext,IUserIdentifierService userIdentifierService)
    {
        _dbContext = dbContext;
        _userIdentifierService = userIdentifierService;
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        });
        _mapper = new Mapper(config);
    }
    
    public async Task<List<CourseDTOResponse>> GetAllCourses()
    {
        var tenantId = await _userIdentifierService.GetTenantId();
 
        var courses=await _dbContext.Courses
            .Where(e => e.BranchTenantId == tenantId)
            .ToListAsync();
        return courses.Select(course => _mapper.Map<CourseDTOResponse>(course)).ToList();
    }
}