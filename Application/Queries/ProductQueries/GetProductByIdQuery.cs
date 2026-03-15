using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries.ProductQueries
{
    public class GetProductByIdQuery : IRequest<ProductDetailsDto>
    {
        public int Id { get; set; }
    }
}
