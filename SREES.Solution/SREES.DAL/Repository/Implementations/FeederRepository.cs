using Microsoft.EntityFrameworkCore;
using SREES.Common.Constants;
using SREES.Common.Models.Dtos.Feeders;
using SREES.Common.Repositories.Implementations;
using SREES.DAL.Context;
using SREES.DAL.Models;
using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.Repository.Implementations
{
    public class FeederRepository : Repository<Feeder>, IFeederRepository
    {
        public SreesContext Context 
        {
            get { return _context is SreesContext ? (SreesContext)_context : throw new TypeLoadException("_context is not type of SreesContext"); }
        }

        public FeederRepository(SreesContext context) : base(context)
        {
        }

        public async Task<Dictionary<FeederType, int>> GetFeederCountByTypeAsync()
        {
            return await Context.Feeders
                .Where(f => !f.IsDeleted)
                .GroupBy(f => f.FeederType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type, x => x.Count);
        }

        public async Task<(IEnumerable<Feeder> Feeders, int TotalCount)> GetFeedersFilteredAsync(FeederFilterRequest filterRequest)
        {
            var query = Context.Feeders
                .Where(f => !f.IsDeleted);

            if (!string.IsNullOrWhiteSpace(filterRequest.SearchTerm))
            {
                var searchTerm = filterRequest.SearchTerm.ToLower();
                query = query.Where(f => f.Name != null && f.Name.ToLower().Contains(searchTerm));
            }

            if (filterRequest.FeederType.HasValue)
            {
                query = query.Where(f => f.FeederType == filterRequest.FeederType.Value);
            }

            if (filterRequest.DateFrom.HasValue)
            {
                query = query.Where(f => f.CreatedAt >= filterRequest.DateFrom.Value);
            }

            if (filterRequest.DateTo.HasValue)
            {
                var dateTo = filterRequest.DateTo.Value.AddDays(1);
                query = query.Where(f => f.CreatedAt < dateTo);
            }

            var totalCount = await query.CountAsync();

            var feeders = await query
                .OrderByDescending(f => f.CreatedAt)
                .Skip((filterRequest.PageNumber - 1) * filterRequest.PageSize)
                .Take(filterRequest.PageSize)
                .ToListAsync();

            return (feeders, totalCount);
        }
    }
}
