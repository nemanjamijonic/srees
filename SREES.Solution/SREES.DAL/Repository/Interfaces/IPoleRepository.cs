using SREES.Common.Constants;
using SREES.Common.Repositories.Interfaces;
using SREES.DAL.Models;

namespace SREES.DAL.Repository.Interfaces
{
    public interface IPoleRepository : IRepository<Pole>
    {
        Task<Dictionary<PoleType, int>> GetPoleCountByTypeAsync();
    }
}
