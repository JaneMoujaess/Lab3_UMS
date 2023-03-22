using MediatR;

namespace Lab2.Application.Odata;

public class GetOdataQuery:IRequest<List<object>>
{
    public Type Type { get; set; }
}