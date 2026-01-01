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
    }
}
