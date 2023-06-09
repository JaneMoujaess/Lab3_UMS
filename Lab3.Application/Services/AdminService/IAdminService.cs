﻿using Lab3.Application.DTOs;
using Lab3.Domain.Models;



public interface IAdminService
{
    public Task<List<Course>> CreateCourse(CourseDtoRequest newCourse);
    public Task<List<string>> GetCommonStudents(long firstTeacherId, long secondTeacherId);
    public Task<List<GenderStatistics>> GetGenderStatistics();
}