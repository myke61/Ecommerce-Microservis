using MediatR;
using Product.Application.Responses.GetProductById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Features.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<GetProductByIdResponse>
    {
        public Guid Id { get; set; }

    }
}
