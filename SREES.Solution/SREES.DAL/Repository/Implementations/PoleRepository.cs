using Microsoft.EntityFrameworkCore;
using SREES.Common.Constants;
using SREES.Common.Models.Dtos.Poles;
using SREES.Common.Repositories.Implementations;
using SREES.DAL.Context;
using SREES.DAL.Models;
using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.Repository.Implementations
{
    public class PoleRepository : Repository<Pole>, IPoleRepository
    {
        public SreesContext Context 
        {
            get { return _context is SreesContext ? (SreesContext)_context : throw new TypeLoadException("_context is not type of SreesContext"); }
        }

        public PoleRepository(SreesContext context) : base(context)
        {
        }

        public async Task<Dictionary<PoleType, int>> GetPoleCountByTypeAsync()
        {
            return await Context.Poles
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.PoleType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type, x => x.Count);
        }

        public async Task<(IEnumerable<Pole> Poles, int TotalCount)> GetPolesFilteredAsync(PoleFilterRequest filterRequest)
        {
            var query = Context.Poles
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(filterRequest.SearchTerm))
            {
                var searchTerm = filterRequest.SearchTerm.ToLower();
                query = query.Where(p =>
                    (p.Name != null && p.Name.ToLower().Contains(searchTerm)) ||
                    (p.Address != null && p.Address.ToLower().Contains(searchTerm)));
            }

            if (filterRequest.PoleType.HasValue)
            {
                query = query.Where(p => p.PoleType == filterRequest.PoleType.Value);
            }

            if (filterRequest.DateFrom.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= filterRequest.DateFrom.Value);
            }

            if (filterRequest.DateTo.HasValue)
            {
                var dateTo = filterRequest.DateTo.Value.AddDays(1);
                query = query.Where(p => p.CreatedAt < dateTo);
            }

            var totalCount = await query.CountAsync();

            var poles = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((filterRequest.PageNumber - 1) * filterRequest.PageSize)
                .Take(filterRequest.PageSize)
                .ToListAsync();

            return (poles, totalCount);
        }
    }
}
