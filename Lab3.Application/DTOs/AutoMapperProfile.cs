using AutoMapper;
using Lab3.Domain.Models;

namespace Lab3.Application.DTOs;

public class AutoMapperProfile:Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Course, CourseDTO>();
        CreateMap<CourseDTO, Course>();
    }
}