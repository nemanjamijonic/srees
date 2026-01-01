using Microsoft.EntityFrameworkCore;
using SREES.Common.Constants;
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
    }
}
