using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCosmosDB.Data
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync(string query);
        Task<Employee> GetEmployeeAsync(string id, string city);
        Task AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(string id, Employee employee);
        Task DeleteEmployeeAsync(string id, string city);
    }
}
