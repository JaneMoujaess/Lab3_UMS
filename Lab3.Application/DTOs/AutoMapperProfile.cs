using AutoMapper;
using Lab3.Domain.Models;
using NpgsqlTypes;

namespace Lab3.Application.DTOs;

public class AutoMapperProfile:Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Course, CourseDTO>();
        CreateMap<CourseDTO, Course>().ForMember(dest=>dest.EnrolmentDateRange,
            opt=>opt.MapFrom(src=>
                new NpgsqlRange<DateOnly>(new DateOnly(src.StartEnrollment.Year,src.StartEnrollment.Month,src.StartEnrollment.Day),
                    new DateOnly(src.EndEnrollment.Year,src.EndEnrollment.Month,src.EndEnrollment.Day))));
    }
}