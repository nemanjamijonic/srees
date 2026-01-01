using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Constants;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Customers;
using SREES.Common.Models.Dtos.Statistics;
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
                return new ResponsePackage<List<CustomerDataOut>>(customerList, "Kupci uspe�no preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri preuzimanju svih kupaca");
                return new ResponsePackage<List<CustomerDataOut>>(null, "Gre�ka pri preuzimanju kupaca");
            }
        }

        public async Task<ResponsePackage<PaginatedResponse<List<CustomerDataOut>>>> GetCustomersFiltered(CustomerFilterRequest filterRequest)
        {
            try
            {
                var (customers, totalCount) = await _uow.GetCustomerRepository().GetCustomersFilteredAsync(filterRequest);
                
                var customerList = _mapper.Map<List<CustomerDataOut>>(customers.ToList());
                var paginatedResponse = new PaginatedResponse<List<CustomerDataOut>>(
                    customerList, 
                    totalCount, 
                    filterRequest.PageNumber, 
                    filterRequest.PageSize);

                return new ResponsePackage<PaginatedResponse<List<CustomerDataOut>>>(
                    paginatedResponse, 
                    "Customers successfully retrieved");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving filtered customers");
                return new ResponsePackage<PaginatedResponse<List<CustomerDataOut>>>(null, "Error retrieving customers");
            }
        }

        public async Task<ResponsePackage<List<CustomerSelectDataOut>>> GetAllCustomersForSelect()
        {
            try
            {
                var customers = await _uow.GetCustomerRepository().GetAllAsync();
                var customerSelectList = _mapper.Map<List<CustomerSelectDataOut>>(customers.ToList());
                return new ResponsePackage<List<CustomerSelectDataOut>>(customerSelectList, "Kupci za select uspe�no preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri preuzimanju kupaca za select");
                return new ResponsePackage<List<CustomerSelectDataOut>>(null, "Gre�ka pri preuzimanju kupaca za select");
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
                return new ResponsePackage<CustomerDataOut?>(customerDataOut, "Kupac uspe�no preuzet");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri preuzimanju kupca sa ID-om {CustomerId}", id);
                return new ResponsePackage<CustomerDataOut?>(null, "Gre�ka pri preuzimanju kupca");
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
                return new ResponsePackage<CustomerDataOut?>(customerDataOut, "Kupac uspe�no kreiran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri kreiranju kupca");
                return new ResponsePackage<CustomerDataOut?>(null, "Gre�ka pri kreiranju kupca");
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
                return new ResponsePackage<CustomerDataOut?>(customerDataOut, "Kupac uspe�no a�uriran");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri a�uriranju kupca sa ID-om {CustomerId}", id);
                return new ResponsePackage<CustomerDataOut?>(null, "Gre�ka pri a�uriranju kupca");
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
                return new ResponsePackage<string>(null, "Kupac uspe�no obrisan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri brisanju kupca sa ID-om {CustomerId}", id);
                return new ResponsePackage<string>(null, "Gre�ka pri brisanju kupca");
            }
        }

        public async Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetCustomerStatistics()
        {
            try
            {
                var customerCountByType = await _uow.GetCustomerRepository().GetCustomerCountByTypeAsync();
                var statistics = customerCountByType
                    .Select(kvp => new EntityCountStatisticsDataOut
                    {
                        Name = kvp.Key.ToString(),
                        Count = kvp.Value,
                        Type = "CustomerType"
                    })
                    .ToList();

                return new ResponsePackage<List<EntityCountStatisticsDataOut>>(statistics, "Statistika kupaca uspe�no preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gre�ka pri preuzimanju statistike kupaca");
                return new ResponsePackage<List<EntityCountStatisticsDataOut>>(null, "Gre�ka pri preuzimanju statistike kupaca");
            }
        }
    }
}
