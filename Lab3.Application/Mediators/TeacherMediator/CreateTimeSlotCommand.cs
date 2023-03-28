using Lab3.Application.Services.TeacherService;
using MediatR;

namespace Lab3.Application.Mediators.TeacherMediator;

public class CreateTimeSlotCommand:IRequest<string>
{
    public DateTime startSession;
    public DateTime endSession;
}

public class CreateTimeSlotCommandHandler : IRequestHandler<CreateTimeSlotCommand, string>
{
    private readonly ITeacherService _teacherService;

    public CreateTimeSlotCommandHandler(ITeacherService teacherService)
    {
        _teacherService = teacherService;
        
    }
    public async Task<string> Handle(CreateTimeSlotCommand request, CancellationToken cancellationToken)
    {
        return await _teacherService.CreateTimeSlot(request.startSession,request.endSession);
    }
}