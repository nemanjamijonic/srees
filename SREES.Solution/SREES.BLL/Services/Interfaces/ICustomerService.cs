using SREES.Common.Models;
using SREES.Common.Models.Dtos.Customers;
using SREES.Common.Models.Dtos.Statistics;

namespace SREES.BLL.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ResponsePackage<List<CustomerDataOut>>> GetAllCustomers();
        Task<ResponsePackage<PaginatedResponse<List<CustomerDataOut>>>> GetCustomersFiltered(CustomerFilterRequest filterRequest);
        Task<ResponsePackage<List<CustomerSelectDataOut>>> GetAllCustomersForSelect();
        Task<ResponsePackage<CustomerDataOut?>> GetCustomerById(int id);
        Task<ResponsePackage<CustomerDataOut?>> CreateCustomer(CustomerDataIn customerDataIn);
        Task<ResponsePackage<CustomerDataOut?>> UpdateCustomer(int id, CustomerDataIn customerDataIn);
        Task<ResponsePackage<string>> DeleteCustomer(int id);
        Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetCustomerStatistics();
    }
}
