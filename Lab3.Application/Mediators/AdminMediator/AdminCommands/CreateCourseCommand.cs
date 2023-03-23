using Lab3.Domain.Models;
using MediatR;

namespace Lab3.Application.Mediators.AdminMediator.AdminCommands;

public class CreateCourseCommand : IRequest<List<Course>>
{
    public Course newCourse { set; get; }
}