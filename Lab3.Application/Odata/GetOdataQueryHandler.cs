using Lab3.Persistence;
using MediatR;

namespace Lab3.Application.Odata;

public class GetOdataQueryHandler : IRequestHandler<GetOdataQuery, List<object>>
{
    private readonly UmsDbContext _dbContext;

    public GetOdataQueryHandler(UmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<object>> Handle(GetOdataQuery request, CancellationToken cancellationToken)
    {
        var data =(List<object>)_dbContext.GetType().GetMethod("Set")?.MakeGenericMethod(request.Type)
            .Invoke(_dbContext, null);

        return data;
    }
}