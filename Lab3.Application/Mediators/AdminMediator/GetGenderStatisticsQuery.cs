using Lab3.Domain.Models;
using MediatR;

namespace Lab3.Application.Mediators.AdminMediator;

public class GetGenderStatisticsQuery:IRequest<List<GenderStatistics>>
{
    
}

public class
    GetGenderStatisticsQueryHandler : IRequestHandler<GetGenderStatisticsQuery,
        List<GenderStatistics>>
{
    private readonly IAdminService _adminService;
    public GetGenderStatisticsQueryHandler(IAdminService adminService)
    {
        _adminService = adminService;
    }
    public async Task<List<GenderStatistics>> Handle(GetGenderStatisticsQuery request, CancellationToken cancellationToken)
    {
        return await _adminService.GetGenderStatistics();
    }
}