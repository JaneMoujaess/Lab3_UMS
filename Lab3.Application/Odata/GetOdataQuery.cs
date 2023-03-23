using MediatR;

namespace Lab3.Application.Odata;

public class GetOdataQuery : IRequest<List<object>>
{
    public Type Type { get; set; }
}