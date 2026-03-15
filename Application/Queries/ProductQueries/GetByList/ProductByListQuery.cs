using MediatR;

namespace Application.Queries.ProductQueries.GetByList
{
    public class ProductByListQuery : IRequest<List<ProductDetailsDto>>
    {
    }
}
