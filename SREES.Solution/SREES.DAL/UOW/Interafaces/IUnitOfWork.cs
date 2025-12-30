using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.UOW.Interafaces
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        Task<int> CompleteAsync();
        IUserRepository GetUserRepository();
        IOutageRepository GetOutageRepository();
        IRegionRepository GetRegionRepository();
        ISubstationRepository GetSubstationRepository();
        IPoleRepository GetPoleRepository();
        IBuildingRepository GetBuildingRepository();
        IFeederRepository GetFeederRepository();
        ICustomerRepository GetCustomerRepository();
    }
}
