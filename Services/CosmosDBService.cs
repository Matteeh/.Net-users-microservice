namespace UserApi.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UserApi.Models;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Fluent;
    using Microsoft.Extensions.Configuration;

    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddUserAsync(ApplicationUser user)
        {
            await this._container.CreateItemAsync<ApplicationUser>(user, new PartitionKey(user.Id));
        }

        public async Task DeleteUserAsync(string id)
        {
            await this._container.DeleteItemAsync<ApplicationUser>(id, new PartitionKey(id));
        }

        public async Task<ApplicationUser> GetUserAsync(string id)
        {
            try
            {
                ItemResponse<ApplicationUser> response = await this._container.ReadItemAsync<ApplicationUser>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<ApplicationUser>(new QueryDefinition(queryString));
            List<ApplicationUser> results = new List<ApplicationUser>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateUserAsync(string id, ApplicationUser user)
        {
            await this._container.UpsertItemAsync<ApplicationUser>(user, new PartitionKey(id));
        }
    }
}