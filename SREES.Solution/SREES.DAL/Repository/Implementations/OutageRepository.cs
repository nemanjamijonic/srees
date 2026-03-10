using Microsoft.EntityFrameworkCore;
using SREES.Common.Constants;
using SREES.Common.Repositories.Implementations;
using SREES.DAL.Context;
using SREES.DAL.Models;
using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.Repository.Implementations
{
    public class OutageRepository : Repository<Outage>, IOutageRepository
    {
        public SreesContext Context 
        {
            get { return _context is SreesContext ? (SreesContext)_context : throw new TypeLoadException("_context is not type of SreesContext"); }
        }

        public OutageRepository(SreesContext context) : base(context)
        {
        }

        public async Task<Dictionary<OutageStatus, int>> GetOutageCountByStatusAsync()
        {
            return await Context.Outages
                .Where(o => !o.IsDeleted)
                .GroupBy(o => o.OutageStatus)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        public async Task<IEnumerable<Outage>> GetAllWithIncludesAsync()
        {
            return await Context.Outages
                .Where(o => !o.IsDeleted)
                .Include(o => o.Region)
                .Include(o => o.Customer)
                .Include(o => o.Building)
                .Include(o => o.DetectedPole)
                .Include(o => o.DetectedFeeder)
                .Include(o => o.DetectedSubstation)
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Outage?> GetByIdWithIncludesAsync(int id)
        {
            return await Context.Outages
                .Where(o => !o.IsDeleted && o.Id == id)
                .Include(o => o.Region)
                .Include(o => o.Customer)
                .Include(o => o.Building)
                .Include(o => o.DetectedPole)
                .Include(o => o.DetectedFeeder)
                .Include(o => o.DetectedSubstation)
                .Include(o => o.User)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Outage>> GetOutagesByFeederIdAsync(int feederId)
        {
            return await Context.Outages
                .Where(o => !o.IsDeleted && o.DetectedFeederId == feederId)
                .Include(o => o.Region)
                .Include(o => o.Customer)
                .Include(o => o.DetectedFeeder)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Outage>> GetOutagesBySubstationIdAsync(int substationId)
        {
            return await Context.Outages
                .Where(o => !o.IsDeleted && o.DetectedSubstationId == substationId)
                .Include(o => o.Region)
                .Include(o => o.Customer)
                .Include(o => o.DetectedSubstation)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}
