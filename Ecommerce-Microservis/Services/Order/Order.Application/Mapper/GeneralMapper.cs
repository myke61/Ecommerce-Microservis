using AutoMapper;
using Order.Application.Features.Command.CreateOrder;
using Order.Application.Features.Queries.GetOrders;
using Order.Application.Requests.GetOrders;

namespace Order.Application.Mapper
{
    public class GeneralMapper : Profile
    {
        public GeneralMapper()
        {
            CreateMap<GetOrdersRequest, GetOrdersQuery>().ReverseMap();
        }
    }
}
