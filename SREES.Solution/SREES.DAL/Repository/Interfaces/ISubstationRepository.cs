using SREES.Common.Constants;
using SREES.Common.Models.Dtos.Substations;
using SREES.Common.Repositories.Interfaces;
using SREES.DAL.Models;

namespace SREES.DAL.Repository.Interfaces
{
    public interface ISubstationRepository : IRepository<Substation>
    {
        Task<Dictionary<SubstationType, int>> GetSubstationCountByTypeAsync();
        Task<(IEnumerable<Substation> Substations, int TotalCount)> GetSubstationsFilteredAsync(SubstationFilterRequest filterRequest);
    }
}
