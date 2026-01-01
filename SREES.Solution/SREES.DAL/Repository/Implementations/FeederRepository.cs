using Microsoft.EntityFrameworkCore;
using SREES.Common.Constants;
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
    }
}
