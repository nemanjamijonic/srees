using Microsoft.EntityFrameworkCore;
using SREES.Common.Constants;
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
    }
}
