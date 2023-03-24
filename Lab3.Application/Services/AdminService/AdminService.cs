﻿using Lab3.Application.Services.TenantProviderService;
using Lab3.Domain.Models;
using Lab3.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Application.Services.AdminService;

public class AdminService : IAdminService
{
    private readonly UmsDbContext _dbContext;

    private readonly ITenantProviderService _tenantProviderService;

    public AdminService(UmsDbContext dbContext,ITenantProviderService tenantProviderService)
    {
        _dbContext = dbContext;
        _tenantProviderService = tenantProviderService;
    }

    public async Task<List<Course>> CreateCourse(Course newCourse)
    {
        var tenantId = _tenantProviderService.GetTenantId();
        newCourse.BranchTenantId = tenantId;
        _dbContext.Courses.Add(newCourse);
        await _dbContext.SaveChangesAsync();
        return await _dbContext.Courses.ToListAsync();
    }

    public async Task<List<Course>> GetAllCourses()
    {
        var tenantId = _tenantProviderService.GetTenantId();
 
        return await _dbContext.Courses
            .Where(e => e.BranchTenantId == tenantId)
            .ToListAsync();
    }
}