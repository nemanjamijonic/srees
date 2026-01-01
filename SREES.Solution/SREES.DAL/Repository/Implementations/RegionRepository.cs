using Microsoft.EntityFrameworkCore;
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
    }
}
