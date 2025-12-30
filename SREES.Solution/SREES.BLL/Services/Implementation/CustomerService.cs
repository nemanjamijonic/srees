using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Customers;
using SREES.DAL.Models;
using SREES.DAL.UOW.Interafaces;

namespace SREES.BLL.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CustomerService(ILogger<CustomerService> logger, IUnitOfWork uow, IMapper mapper)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponsePackage<List<CustomerDataOut>>> GetAllCustomers()
        {
            try
            {
                var customers = await _uow.GetCustomerRepository().GetAllAsync();
                var customerList = _mapper.Map<List<CustomerDataOut>>(customers.ToList());
                return new ResponsePackage<List<CustomerDataOut>>(customerList, "Kupci uspešno preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju svih kupaca");
                return new ResponsePackage<List<CustomerDataOut>>(null, "Greška pri preuzimanju kupaca");
            }
        }

        public async Task<ResponsePackage<List<CustomerSelectDataOut>>> GetAllCustomersForSelect()
        {
            try
            {
                var customers = await _uow.GetCustomerRepository().GetAllAsync();
                var customerSelectList = _mapper.Map<List<CustomerSelectDataOut>>(customers.ToList());
                return new ResponsePackage<List<CustomerSelectDataOut>>(customerSelectList, "Kupci za select uspešno preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju kupaca za select");
                return new ResponsePackage<List<CustomerSelectDataOut>>(null, "Greška pri preuzimanju kupaca za select");
            }
        }

        public async Task<ResponsePackage<CustomerDataOut?>> GetCustomerById(int id)
        {
            try
            {
                var customer = await _uow.GetCustomerRepository().GetByIdAsync(id);
                if (customer == null)
                    return new ResponsePackage<CustomerDataOut?>(null, "Kupac nije prona?en");

                var customerDataOut = _mapper.Map<CustomerDataOut>(customer);
                return new ResponsePackage<CustomerDataOut?>(customerDataOut, "Kupac uspešno preuzet");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju kupca sa ID-om {CustomerId}", id);
                return new ResponsePackage<CustomerDataOut?>(null, "Greška pri preuzimanju kupca");
            }
        }

        public async Task<ResponsePackage<CustomerDataOut?>> CreateCustomer(CustomerDataIn customerDataIn)
        {
            try
            {
                // Provera da li zgrada postoji ako je poslata
                if (customerDataIn.BuildingId.HasValue)
                {
                    var building = await _uow.GetBuildingRepository().GetByIdAsync(customerDataIn.BuildingId.Value);
                    if (building == null)
                        return new ResponsePackage<CustomerDataOut?>(null, "Zgrada nije prona?ena");
                }

                var customer = new Customer
                {
                    FirstName = customerDataIn.FirstName,
                    LastName = customerDataIn.LastName,
                    Address = customerDataIn.Address,
                    BuildingId = customerDataIn.BuildingId,
                    IsActive = customerDataIn.IsActive,
                    CustomerType = customerDataIn.CustomerType
                };

                await _uow.GetCustomerRepository().AddAsync(customer);
                await _uow.CompleteAsync();

                var customerDataOut = _mapper.Map<CustomerDataOut>(customer);
                return new ResponsePackage<CustomerDataOut?>(customerDataOut, "Kupac uspešno kreiran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju kupca");
                return new ResponsePackage<CustomerDataOut?>(null, "Greška pri kreiranju kupca");
            }
        }

        public async Task<ResponsePackage<CustomerDataOut?>> UpdateCustomer(int id, CustomerDataIn customerDataIn)
        {
            try
            {
                var customer = await _uow.GetCustomerRepository().GetByIdAsync(id);
                if (customer == null)
                    return new ResponsePackage<CustomerDataOut?>(null, "Kupac nije prona?en");

                // Provera da li zgrada postoji ako je poslata
                if (customerDataIn.BuildingId.HasValue)
                {
                    var building = await _uow.GetBuildingRepository().GetByIdAsync(customerDataIn.BuildingId.Value);
                    if (building == null)
                        return new ResponsePackage<CustomerDataOut?>(null, "Zgrada nije prona?ena");
                }

                customer.FirstName = customerDataIn.FirstName;
                customer.LastName = customerDataIn.LastName;
                customer.Address = customerDataIn.Address;
                customer.BuildingId = customerDataIn.BuildingId;
                customer.IsActive = customerDataIn.IsActive;
                customer.CustomerType = customerDataIn.CustomerType;
                customer.LastUpdateTime = DateTime.Now;

                await _uow.CompleteAsync();

                var customerDataOut = _mapper.Map<CustomerDataOut>(customer);
                return new ResponsePackage<CustomerDataOut?>(customerDataOut, "Kupac uspešno ažuriran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ažuriranju kupca sa ID-om {CustomerId}", id);
                return new ResponsePackage<CustomerDataOut?>(null, "Greška pri ažuriranju kupca");
            }
        }

        public async Task<ResponsePackage<string>> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _uow.GetCustomerRepository().GetByIdAsync(id);
                if (customer == null)
                    return new ResponsePackage<string>(null, "Kupac nije prona?en");

                _uow.GetCustomerRepository().RemoveEntity(customer);
                await _uow.CompleteAsync();
                return new ResponsePackage<string>(null, "Kupac uspešno obrisan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri brisanju kupca sa ID-om {CustomerId}", id);
                return new ResponsePackage<string>(null, "Greška pri brisanju kupca");
            }
        }
    }
}
