using SREES.Common.Constants;
using SREES.Common.Models.Dtos.Customers;
using SREES.Common.Repositories.Interfaces;
using SREES.DAL.Models;

namespace SREES.DAL.Repository.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Dictionary<CustomerType, int>> GetCustomerCountByTypeAsync();
        Task<(IEnumerable<Customer> Customers, int TotalCount)> GetCustomersFilteredAsync(CustomerFilterRequest filterRequest);
    }
}
