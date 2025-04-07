using Ecommerce.Base.Responses;
using MediatR;
using Product.Application.Responses.DeleteProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Features.Command.DeleteProduct
{
    public class DeleteProductCommand : IRequest<DeleteProductResponse> 
    {
        public Guid Id { get; set; }
    }
}
