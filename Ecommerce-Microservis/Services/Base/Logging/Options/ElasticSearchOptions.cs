using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Options
{
    public class ElasticSearchOptions
    {
        public string Uri { get; set; } = "http://localhost:9200";
        public string DefaultIndex { get; set; } = "request-logs";
    }
}
