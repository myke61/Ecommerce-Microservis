using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infastructure.MassTransit
{
    public class RabbitMQSettings
    {
        public RabbitMQHostSettings Primary { get; set; } = new RabbitMQHostSettings()
        {
            Host = "localhost",
            Username = "guest",
            Password = "guest"
        };
    }

    public class RabbitMQHostSettings
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
