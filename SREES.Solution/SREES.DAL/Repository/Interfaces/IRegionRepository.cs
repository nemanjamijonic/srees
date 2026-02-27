using SREES.Common.Models.Dtos.Regions;
using SREES.Common.Repositories.Interfaces;
using SREES.DAL.Models;

namespace SREES.DAL.Repository.Interfaces
{
    public interface IRegionRepository : IRepository<Region>
    {
        Task<int> GetTotalRegionCountAsync();
        Task<(IEnumerable<Region> Regions, int TotalCount)> GetRegionsFilteredAsync(RegionFilterRequest filterRequest);
    }
}
