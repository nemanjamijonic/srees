using AutoMapper;
using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Constants;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Customers;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Common.Services.Interfaces;
using SREES.DAL.Models;
using SREES.DAL.UOW.Interafaces;

namespace SREES.BLL.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICachingService _cachingService;

        // Cache keys constants
        private const string EntityPrefix = "Customers";
        private const string AllCustomersCacheKey = $"{EntityPrefix}|All";
        private const string CustomerSelectCacheKey = $"{EntityPrefix}|Select";
        private const string CustomerStatisticsCacheKey = $"{EntityPrefix}|Statistics";
        private const string CustomerFilteredPrefix = $"{EntityPrefix}|Filtered";
        private const string CustomerByIdPrefix = $"{EntityPrefix}|ById";
        private const int CacheExpirationMinutes = 30;

        public CustomerService(ILogger<CustomerService> logger, IUnitOfWork uow, IMapper mapper, ICachingService cachingService)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
            _cachingService = cachingService;
        }

        public async Task<ResponsePackage<List<CustomerDataOut>>> GetAllCustomers()
        {
            try
            {
                // Check cache first
                if (await _cachingService.ObjectsCached<List<CustomerDataOut>>(AllCustomersCacheKey))
                {
                    var cachedCustomers = await _cachingService.GetObjectsFromCache<List<CustomerDataOut>>(AllCustomersCacheKey);
                    if (cachedCustomers != null)
                    {
                        _logger.LogInformation("Customers retrieved from cache");
                        return new ResponsePackage<List<CustomerDataOut>>(cachedCustomers, "Kupci uspešno preuzeti iz keša");
                    }
                }

                // If not in cache, get from database
                var customers = await _uow.GetCustomerRepository().GetAllAsync();
                var customerList = _mapper.Map<List<CustomerDataOut>>(customers.ToList());

                // Store in cache
                await _cachingService.SetObjectsInCache(AllCustomersCacheKey, customerList, CacheExpirationMinutes);
                _logger.LogInformation("Customers stored in cache");

                return new ResponsePackage<List<CustomerDataOut>>(customerList, "Kupci uspešno preuzeti");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju svih kupaca");
                return new ResponsePackage<List<CustomerDataOut>>(null, "Greška pri preuzimanju kupaca");
            }
        }

        public async Task<ResponsePackage<PaginatedResponse<List<CustomerDataOut>>>> GetCustomersFiltered(CustomerFilterRequest filterRequest)
        {
            try
            {
                // Generate cache key based on filter parameters
                var cacheKey = GenerateFilteredCacheKey(filterRequest);

                // Check cache first
                if (await _cachingService.ObjectsCached<PaginatedResponse<List<CustomerDataOut>>>(cacheKey))
                {
                    var cachedResult = await _cachingService.GetObjectsFromCache<PaginatedResponse<List<CustomerDataOut>>>(cacheKey);
                    if (cachedResult != null)
                    {
                        _logger.LogInformation("Filtered customers retrieved from cache with key: {CacheKey}", cacheKey);
                        return new ResponsePackage<PaginatedResponse<List<CustomerDataOut>>>(
                            cachedResult,
                            "Customers successfully retrieved from cache");
                    }
                }

                // If not in cache, get from database
                var (customers, totalCount) = await _uow.GetCustomerRepository().GetCustomersFilteredAsync(filterRequest);
                
                var customerList = _mapper.Map<List<CustomerDataOut>>(customers.ToList());
                var paginatedResponse = new PaginatedResponse<List<CustomerDataOut>>(
                    customerList, 
                    totalCount, 
                    filterRequest.PageNumber, 
                    filterRequest.PageSize);

                // Store in cache
                await _cachingService.SetObjectsInCache(cacheKey, paginatedResponse, CacheExpirationMinutes);
                _logger.LogInformation("Filtered customers stored in cache with key: {CacheKey}", cacheKey);

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
                // Check cache first
                if (await _cachingService.ObjectsCached<List<CustomerSelectDataOut>>(CustomerSelectCacheKey))
                {
                    var cachedCustomers = await _cachingService.GetObjectsFromCache<List<CustomerSelectDataOut>>(CustomerSelectCacheKey);
                    if (cachedCustomers != null)
                    {
                        _logger.LogInformation("Customers for select retrieved from cache");
                        return new ResponsePackage<List<CustomerSelectDataOut>>(cachedCustomers, "Kupci za select uspešno preuzeti iz keša");
                    }
                }

                // If not in cache, get from database
                var customers = await _uow.GetCustomerRepository().GetAllAsync();
                var customerSelectList = _mapper.Map<List<CustomerSelectDataOut>>(customers.ToList());

                // Store in cache
                await _cachingService.SetObjectsInCache(CustomerSelectCacheKey, customerSelectList, CacheExpirationMinutes);
                _logger.LogInformation("Customers for select stored in cache");

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
                var cacheKey = $"{CustomerByIdPrefix}:{id}";

                // Check cache first
                if (await _cachingService.ObjectsCached<CustomerDataOut>(cacheKey))
                {
                    var cachedCustomer = await _cachingService.GetObjectsFromCache<CustomerDataOut>(cacheKey);
                    if (cachedCustomer != null)
                    {
                        _logger.LogInformation("Customer {CustomerId} retrieved from cache", id);
                        return new ResponsePackage<CustomerDataOut?>(cachedCustomer, "Kupac uspešno preuzet iz keša");
                    }
                }

                // If not in cache, get from database
                var customer = await _uow.GetCustomerRepository().GetByIdAsync(id);
                if (customer == null)
                    return new ResponsePackage<CustomerDataOut?>(null, "Kupac nije pronađen");

                var customerDataOut = _mapper.Map<CustomerDataOut>(customer);

                // Store in cache
                await _cachingService.SetObjectsInCache(cacheKey, customerDataOut, CacheExpirationMinutes);
                _logger.LogInformation("Customer {CustomerId} stored in cache", id);

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
                        return new ResponsePackage<CustomerDataOut?>(null, "Zgrada nije pronađena");
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

                // Invalidate all relevant caches after creating a customer
                await InvalidateCustomerCaches();
                _logger.LogInformation("Customer caches invalidated after creating customer {CustomerId}", customer.Id);

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
                    return new ResponsePackage<CustomerDataOut?>(null, "Kupac nije pronađen");

                // Provera da li zgrada postoji ako je poslata
                if (customerDataIn.BuildingId.HasValue)
                {
                    var building = await _uow.GetBuildingRepository().GetByIdAsync(customerDataIn.BuildingId.Value);
                    if (building == null)
                        return new ResponsePackage<CustomerDataOut?>(null, "Zgrada nije pronađena");
                }

                customer.FirstName = customerDataIn.FirstName;
                customer.LastName = customerDataIn.LastName;
                customer.Address = customerDataIn.Address;
                customer.BuildingId = customerDataIn.BuildingId;
                customer.IsActive = customerDataIn.IsActive;
                customer.CustomerType = customerDataIn.CustomerType;
                customer.LastUpdateTime = DateTime.Now;

                await _uow.CompleteAsync();

                // Invalidate all relevant caches after updating a customer
                await InvalidateCustomerCaches();
                await _cachingService.RemoveObjectsFromCache($"{CustomerByIdPrefix}:{id}");
                _logger.LogInformation("Customer caches invalidated after updating customer {CustomerId}", id);

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
                    return new ResponsePackage<string>(null, "Kupac nije pronađen");

                _uow.GetCustomerRepository().RemoveEntity(customer);
                await _uow.CompleteAsync();

                // Invalidate all relevant caches after deleting a customer
                await InvalidateCustomerCaches();
                await _cachingService.RemoveObjectsFromCache($"{CustomerByIdPrefix}:{id}");
                _logger.LogInformation("Customer caches invalidated after deleting customer {CustomerId}", id);

                return new ResponsePackage<string>(null, "Kupac uspešno obrisan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri brisanju kupca sa ID-om {CustomerId}", id);
                return new ResponsePackage<string>(null, "Greška pri brisanju kupca");
            }
        }

        public async Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetCustomerStatistics()
        {
            try
            {
                // Check cache first
                if (await _cachingService.ObjectsCached<List<EntityCountStatisticsDataOut>>(CustomerStatisticsCacheKey))
                {
                    var cachedStatistics = await _cachingService.GetObjectsFromCache<List<EntityCountStatisticsDataOut>>(CustomerStatisticsCacheKey);
                    if (cachedStatistics != null)
                    {
                        _logger.LogInformation("Customer statistics retrieved from cache");
                        return new ResponsePackage<List<EntityCountStatisticsDataOut>>(cachedStatistics, "Statistika kupaca uspešno preuzeta iz keša");
                    }
                }

                // If not in cache, get from database
                var customerCountByType = await _uow.GetCustomerRepository().GetCustomerCountByTypeAsync();
                var statistics = customerCountByType
                    .Select(kvp => new EntityCountStatisticsDataOut
                    {
                        Name = kvp.Key.ToString(),
                        Count = kvp.Value,
                        Type = "CustomerType"
                    })
                    .ToList();

                // Store in cache
                await _cachingService.SetObjectsInCache(CustomerStatisticsCacheKey, statistics, CacheExpirationMinutes);
                _logger.LogInformation("Customer statistics stored in cache");

                return new ResponsePackage<List<EntityCountStatisticsDataOut>>(statistics, "Statistika kupaca uspešno preuzeta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri preuzimanju statistike kupaca");
                return new ResponsePackage<List<EntityCountStatisticsDataOut>>(null, "Greška pri preuzimanju statistike kupaca");
            }
        }

        #region Private Helper Methods

        /// <summary>
        /// Generates a cache key for filtered customer queries based on filter parameters
        /// </summary>
        private string GenerateFilteredCacheKey(CustomerFilterRequest filterRequest)
        {
            var keyParts = new List<string> { CustomerFilteredPrefix };

            keyParts.Add($"Page_{filterRequest.PageNumber}");
            keyParts.Add($"Size_{filterRequest.PageSize}");

            if (!string.IsNullOrWhiteSpace(filterRequest.SearchTerm))
                keyParts.Add($"Search_{filterRequest.SearchTerm.ToLower().Replace(" ", "_")}");

            if (filterRequest.CustomerType.HasValue)
                keyParts.Add($"Type_{filterRequest.CustomerType.Value}");

            if (filterRequest.DateFrom.HasValue)
                keyParts.Add($"From_{filterRequest.DateFrom.Value:yyyyMMdd}");

            if (filterRequest.DateTo.HasValue)
                keyParts.Add($"To_{filterRequest.DateTo.Value:yyyyMMdd}");

            return string.Join("|", keyParts);
        }

        /// <summary>
        /// Invalidates all customer-related caches
        /// Called after Create, Update, or Delete operations
        /// </summary>
        private async Task InvalidateCustomerCaches()
        {
            try
            {
                // Invalidate specific cache keys
                await _cachingService.RemoveObjectsFromCache(AllCustomersCacheKey);
                await _cachingService.RemoveObjectsFromCache(CustomerSelectCacheKey);
                await _cachingService.RemoveObjectsFromCache(CustomerStatisticsCacheKey);
                
                // Invalidate ALL filtered caches using pattern matching
                await _cachingService.RemoveObjectsFromCacheByPattern(CustomerFilteredPrefix);
                
                _logger.LogInformation("All customer caches invalidated including filtered caches");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error invalidating customer caches, but continuing operation");
            }
        }

        #endregion
    }
}
