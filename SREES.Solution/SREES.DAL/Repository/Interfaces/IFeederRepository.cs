using SREES.Common.Constants;
using SREES.Common.Models.Dtos.Feeders;
using SREES.Common.Repositories.Interfaces;
using SREES.DAL.Models;

namespace SREES.DAL.Repository.Interfaces
{
    public interface IFeederRepository : IRepository<Feeder>
    {
        Task<Dictionary<FeederType, int>> GetFeederCountByTypeAsync();
        Task<(IEnumerable<Feeder> Feeders, int TotalCount)> GetFeedersFilteredAsync(FeederFilterRequest filterRequest);
    }
}
