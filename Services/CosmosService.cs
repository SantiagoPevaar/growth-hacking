using Microsoft.Azure.Cosmos;

namespace CustomEmailSender.Services
{
    public class CosmosService : ICosmosService
    {
        private readonly Container _container;

        public CosmosService(CosmosClient cosmosClient, IConfiguration config)
        {
            var databaseName = config["CosmosDb:DatabaseName"];
            var containerName = config["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }
        public async Task AddItemAsync<T>(T item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.GetType().GetProperty("Email").GetValue(item, null).ToString()));
        }

        public async Task<T> GetItemAsync<T>(string id, string email)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(email));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(string queryString)
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
    }
}
