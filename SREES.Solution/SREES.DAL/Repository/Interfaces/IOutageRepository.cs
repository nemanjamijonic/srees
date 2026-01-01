using SREES.Common.Constants;
using SREES.Common.Repositories.Interfaces;
using SREES.DAL.Models;

namespace SREES.DAL.Repository.Interfaces
{
    public interface IOutageRepository : IRepository<Outage>
    {
        Task<Dictionary<OutageStatus, int>> GetOutageCountByStatusAsync();
    }
}
