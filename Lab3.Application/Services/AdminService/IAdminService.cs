﻿using Lab3.Domain.Models;



public interface IAdminService
{
    public Task<List<Course>> GetAllCourses();
    public Task<List<Course>> CreateCourse(Course newCourse);
}