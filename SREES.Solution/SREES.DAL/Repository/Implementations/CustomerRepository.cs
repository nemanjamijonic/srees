using Microsoft.EntityFrameworkCore;
using SREES.Common.Constants;
using SREES.Common.Models.Dtos.Customers;
using SREES.Common.Repositories.Implementations;
using SREES.DAL.Context;
using SREES.DAL.Models;
using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.Repository.Implementations
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public SreesContext Context 
        {
            get { return _context is SreesContext ? (SreesContext)_context : throw new TypeLoadException("_context is not type of SreesContext"); }
        }

        public CustomerRepository(SreesContext context) : base(context)
        {
        }

        public async Task<Dictionary<CustomerType, int>> GetCustomerCountByTypeAsync()
        {
            return await Context.Customers
                .Where(c => !c.IsDeleted)
                .GroupBy(c => c.CustomerType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type, x => x.Count);
        }

        public async Task<(IEnumerable<Customer> Customers, int TotalCount)> GetCustomersFilteredAsync(CustomerFilterRequest filterRequest)
        {
            var query = Context.Customers.Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(filterRequest.SearchTerm))
            {
                var searchTerm = filterRequest.SearchTerm.ToLower();
                query = query.Where(c =>
                    c.FirstName.ToLower().Contains(searchTerm) ||
                    c.LastName.ToLower().Contains(searchTerm) ||
                    (c.Address != null && c.Address.ToLower().Contains(searchTerm)));
            }

            if (filterRequest.CustomerType.HasValue)
            {
                query = query.Where(c => c.CustomerType == (CustomerType)filterRequest.CustomerType.Value);
            }

            if (filterRequest.DateFrom.HasValue)
            {
                query = query.Where(c => c.CreatedAt >= filterRequest.DateFrom.Value);
            }

            if (filterRequest.DateTo.HasValue)
            {
                var dateTo = filterRequest.DateTo.Value.AddDays(1);
                query = query.Where(c => c.CreatedAt < dateTo);
            }

            var totalCount = await query.CountAsync();

            var customers = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((filterRequest.PageNumber - 1) * filterRequest.PageSize)
                .Take(filterRequest.PageSize)
                .ToListAsync();

            return (customers, totalCount);
        }
    }
}
