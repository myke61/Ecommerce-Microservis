using AutoMapper;
using Order.Application.Features.Command.CreateOrder;

namespace Order.Application.Mapper
{
    public class GeneralMapper : Profile
    {
        public GeneralMapper()
        {
            //CreateMap<CreateOrderCommand, CreateOrderCommand>().ReverseMap();
        }
    }
}
