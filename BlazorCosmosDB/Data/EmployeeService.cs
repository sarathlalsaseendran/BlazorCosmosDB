using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCosmosDB.Data
{
    public class EmployeeService : IEmployeeService
    {
        private readonly Container _container;

        public EmployeeService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            employee.Id = Guid.NewGuid().ToString();
            await _container.CreateItemAsync(employee, new PartitionKey(employee.City));
        }

        public async Task DeleteEmployeeAsync(string id, string city)
        {
            await this._container.DeleteItemAsync<Employee>(id, new PartitionKey(city));
        }

        public async Task<Employee> GetEmployeeAsync(string id, string city)
        {
            try
            {
                ItemResponse<Employee> response = await _container.ReadItemAsync<Employee>(id, new PartitionKey(city));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Employee>(new QueryDefinition(queryString));
            List<Employee> results = new List<Employee>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateEmployeeAsync(string id, Employee employee)
        {
            await _container.UpsertItemAsync(employee, new PartitionKey(employee.City));
        }
    }
}
