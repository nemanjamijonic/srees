using SREES.Common.Constants;
using SREES.Common.Repositories.Interfaces;
using SREES.DAL.Models;

namespace SREES.DAL.Repository.Interfaces
{
    public interface IOutageRepository : IRepository<Outage>
    {
        Task<Dictionary<OutageStatus, int>> GetOutageCountByStatusAsync();
        
        /// <summary>
        /// Preuzima sve Outage sa uklju?enim relacijama (Region, Customer, Building, DetectedPole, DetectedFeeder, DetectedSubstation)
        /// </summary>
        Task<IEnumerable<Outage>> GetAllWithIncludesAsync();
        
        /// <summary>
        /// Preuzima Outage po ID-u sa uklju?enim relacijama
        /// </summary>
        Task<Outage?> GetByIdWithIncludesAsync(int id);
        
        /// <summary>
        /// Preuzima sve Outage za odre?eni Feeder
        /// </summary>
        Task<IEnumerable<Outage>> GetOutagesByFeederIdAsync(int feederId);
        
        /// <summary>
        /// Preuzima sve Outage za odre?enu Substation
        /// </summary>
        Task<IEnumerable<Outage>> GetOutagesBySubstationIdAsync(int substationId);
    }
}
