using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infastructure.MassTransit
{
    public interface IMessageConsumer
    {
    }
    public interface IPrimaryBus : IBus { }
}
