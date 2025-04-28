using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.Interface
{
    public interface IEventStoreHandler
    {
        void AppendToStreamAsync<T>(string streamName, string type, T data, CancellationToken cancellationToken = default) where T : class;
    }
}
