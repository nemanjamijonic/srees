using Microsoft.EntityFrameworkCore;
using SREES.Common.Constants;
using SREES.Common.Models.Dtos.Substations;
using SREES.Common.Repositories.Implementations;
using SREES.DAL.Context;
using SREES.DAL.Models;
using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.Repository.Implementations
{
    public class SubstationRepository : Repository<Substation>, ISubstationRepository
    {
        public SreesContext Context 
        {
            get { return _context is SreesContext ? (SreesContext)_context : throw new TypeLoadException("_context is not type of SreesContext"); }
        }

        public SubstationRepository(SreesContext context) : base(context)
        {
        }

        public async Task<Dictionary<SubstationType, int>> GetSubstationCountByTypeAsync()
        {
            return await Context.Substations
                .Where(s => !s.IsDeleted)
                .GroupBy(s => s.SubstationType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type, x => x.Count);
        }

        public async Task<(IEnumerable<Substation> Substations, int TotalCount)> GetSubstationsFilteredAsync(SubstationFilterRequest filterRequest)
        {
            var query = Context.Substations
                .Where(s => !s.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterRequest.SearchTerm))
                query = query.Where(s => s.Name.Contains(filterRequest.SearchTerm));

            if (filterRequest.SubstationType.HasValue)
                query = query.Where(s => s.SubstationType == filterRequest.SubstationType.Value);

            if (filterRequest.DateFrom.HasValue)
                query = query.Where(s => s.CreatedAt >= filterRequest.DateFrom.Value);

            if (filterRequest.DateTo.HasValue)
                query = query.Where(s => s.CreatedAt <= filterRequest.DateTo.Value);

            var totalCount = await query.CountAsync();

            var substations = await query
                .OrderByDescending(s => s.CreatedAt)
                .Skip((filterRequest.PageNumber - 1) * filterRequest.PageSize)
                .Take(filterRequest.PageSize)
                .ToListAsync();

            return (substations, totalCount);
        }
    }
}
