using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Customers;
using SREES.Services.Interfaces;

namespace SREES.Services.Implementations
{
    public class CustomerApplicationService : ICustomerApplicationService
    {
        private readonly ICustomerService _customerService;

        public CustomerApplicationService(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ResponsePackage<List<CustomerDataOut>>> GetAllCustomers()
        {
            return await _customerService.GetAllCustomers();
        }

        public async Task<ResponsePackage<List<CustomerSelectDataOut>>> GetAllCustomersForSelect()
        {
            return await _customerService.GetAllCustomersForSelect();
        }

        public async Task<ResponsePackage<CustomerDataOut?>> GetCustomerById(int id)
        {
            return await _customerService.GetCustomerById(id);
        }

        public async Task<ResponsePackage<CustomerDataOut?>> CreateCustomer(CustomerDataIn customerDataIn)
        {
            return await _customerService.CreateCustomer(customerDataIn);
        }

        public async Task<ResponsePackage<CustomerDataOut?>> UpdateCustomer(int id, CustomerDataIn customerDataIn)
        {
            return await _customerService.UpdateCustomer(id, customerDataIn);
        }

        public async Task<ResponsePackage<string>> DeleteCustomer(int id)
        {
            return await _customerService.DeleteCustomer(id);
        }
    }
}
