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

    public async Task<List<string>> GetCommonStudents(long firstTeacherId, long secondTeacherId)
    {
        var tenantId = await _userIdentifierService.GetTenantId();

        List<string> commonStudents = await
            (from teacherPerCourse1 in _dbContext.TeacherPerCourses
            join classEnrollments1 in _dbContext.ClassEnrollments on teacherPerCourse1.Id equals classEnrollments1.ClassId
            join users1 in _dbContext.Users on classEnrollments1.StudentId equals users1.Id
            join teacherPerCourse2 in _dbContext.TeacherPerCourses on teacherPerCourse1.CourseId equals teacherPerCourse2.CourseId
            join classEnrollments2 in _dbContext.ClassEnrollments on teacherPerCourse2.Id equals classEnrollments2.ClassId
            join users2 in _dbContext.Users on classEnrollments2.StudentId equals users2.Id
            where users1.BranchTenantId==tenantId
            where users2.BranchTenantId==tenantId
            where teacherPerCourse1.TeacherId == firstTeacherId && teacherPerCourse2.TeacherId == secondTeacherId
            select users1.Name)
            .Distinct()
            .ToListAsync();

        return commonStudents;
    }

    // todo: implement the distribution of courses by gender functionality
    public async Task<List<GenderStatistics>> GetGenderStatistics()
    {
        throw new NotImplementedException();
    }
}