using Microsoft.EntityFrameworkCore;
using SREES.Common.Models.Dtos.Regions;
using SREES.Common.Repositories.Implementations;
using SREES.DAL.Context;
using SREES.DAL.Models;
using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.Repository.Implementations
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        public SreesContext Context 
        {
            get { return _context is SreesContext ? (SreesContext)_context : throw new TypeLoadException("_context is not type of SreesContext"); }
        }

        public RegionRepository(SreesContext context) : base(context)
        {
        }

        public async Task<int> GetTotalRegionCountAsync()
        {
            return await Context.Regions
                .Where(r => !r.IsDeleted)
                .CountAsync();
        }

        public async Task<(IEnumerable<Region> Regions, int TotalCount)> GetRegionsFilteredAsync(RegionFilterRequest filterRequest)
        {
            var query = Context.Regions
                .Where(r => !r.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterRequest.SearchTerm))
                query = query.Where(r => r.Name.Contains(filterRequest.SearchTerm));

            if (filterRequest.DateFrom.HasValue)
                query = query.Where(r => r.CreatedAt >= filterRequest.DateFrom.Value);

            if (filterRequest.DateTo.HasValue)
                query = query.Where(r => r.CreatedAt <= filterRequest.DateTo.Value);

            var totalCount = await query.CountAsync();

            var regions = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((filterRequest.PageNumber - 1) * filterRequest.PageSize)
                .Take(filterRequest.PageSize)
                .ToListAsync();

            return (regions, totalCount);
        }
    }
}
