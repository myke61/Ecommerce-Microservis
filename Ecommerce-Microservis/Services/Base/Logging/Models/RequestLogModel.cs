using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Models
{
    public class RequestLogModel
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Query { get; set; }
        public string? Body { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
