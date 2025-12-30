using SREES.Common.Repositories.Interfaces;
using SREES.DAL.Models;

namespace SREES.DAL.Repository.Interfaces
{
    public interface IBuildingRepository : IRepository<Building>
    {
        Task<IEnumerable<Building>> GetAllWithPoleAsync();
    }
}
