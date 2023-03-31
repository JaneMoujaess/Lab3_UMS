using MediatR;

namespace Lab3.Application.Mediators.AdminMediator;

public class GetCommonStudentsQuery:IRequest<List<string>>
{
    public long FirstTeacherId { set; get; }
    public long SecondTeacherId { set; get; }
}

public class GetCommonStudentsQueryHandler : IRequestHandler<GetCommonStudentsQuery, List<string>>
{
    private readonly IAdminService _adminService;
    public GetCommonStudentsQueryHandler(IAdminService adminService)
    {
        _adminService = adminService;
    }
    public async Task<List<string>> Handle(GetCommonStudentsQuery request, CancellationToken cancellationToken)
    {
        return await _adminService.GetCommonStudents(request.FirstTeacherId,request.SecondTeacherId);
    }
}