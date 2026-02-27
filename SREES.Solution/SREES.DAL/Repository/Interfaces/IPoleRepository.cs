using SREES.Common.Constants;
using SREES.Common.Models.Dtos.Poles;
using SREES.Common.Repositories.Interfaces;
using SREES.DAL.Models;

namespace SREES.DAL.Repository.Interfaces
{
    public interface IPoleRepository : IRepository<Pole>
    {
        Task<Dictionary<PoleType, int>> GetPoleCountByTypeAsync();
        Task<(IEnumerable<Pole> Poles, int TotalCount)> GetPolesFilteredAsync(PoleFilterRequest filterRequest);
    }
}
