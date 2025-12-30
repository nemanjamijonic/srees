using Microsoft.EntityFrameworkCore;
using SREES.Common.Repositories.Implementations;
using SREES.DAL.Context;
using SREES.DAL.Models;
using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.Repository.Implementations
{
    public class BuildingRepository : Repository<Building>, IBuildingRepository
    {
        public SreesContext Context 
        {
            get { return _context is SreesContext ? (SreesContext)_context : throw new TypeLoadException("_context is not type of SreesContext"); }
        }

        public BuildingRepository(SreesContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Building>> GetAllWithPoleAsync()
        {
            return await Context.Buildings
                .Include(b => b.Pole)
                .Where(b => !b.IsDeleted)
                .ToListAsync();
        }
    }
}
