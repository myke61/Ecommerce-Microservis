using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Services.Interface
{
    public interface IElasticSearchService
    {
        Task IndexAsync<T>(T document, string? indexName = null) where T : class;
    }
}
