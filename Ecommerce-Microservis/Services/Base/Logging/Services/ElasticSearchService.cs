using Logging.Options;
using Logging.Services.Interface;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _options;

        public ElasticSearchService(IOptions<ElasticSearchOptions> options)
        {
            _options = options.Value;

            var settings = new ConnectionSettings(new Uri(_options.Uri))
                .DefaultIndex(_options.DefaultIndex)
                .EnableDebugMode();

            _elasticClient = new ElasticClient(settings);
        }

        public async Task IndexAsync<T>(T document, string? indexName = null) where T : class
        {
            var response = await _elasticClient.IndexAsync(document, i => i.Index(indexName ?? _options.DefaultIndex));

            if (!response.IsValid)
                throw new Exception($"Elasticsearch indexing failed: {response.OriginalException.Message}");
        }
    }
}
