using Microsoft.EntityFrameworkCore.Diagnostics;
using SREES.DAL.Context;
using SREES.DAL.Repository.Implementations;
using SREES.DAL.Repository.Interfaces;
using SREES.DAL.UOW.Interafaces;

namespace SREES.DAL.UOW.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        public SreesContext? _context;
        private IUserRepository? UserRepository;
        private IOutageRepository? OutageRepository;
        private IRegionRepository? RegionRepository;
        private ISubstationRepository? SubstationRepository;
        private IPoleRepository? PoleRepository;

        public UnitOfWork(SreesContext context)
        {
            _context = context;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context!.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        public IUserRepository GetUserRepository()
        {
            return UserRepository ??= new UserRepository(_context!);
        }

        public IOutageRepository GetOutageRepository()
        {
            return OutageRepository ??= new OutageRepository(_context!);
        }

        public IRegionRepository GetRegionRepository()
        {
            return RegionRepository ??= new RegionRepository(_context!);
        }

        public ISubstationRepository GetSubstationRepository()
        {
            return SubstationRepository ??= new SubstationRepository(_context!);
        }

        public IPoleRepository GetPoleRepository()
        {
            return PoleRepository ??= new PoleRepository(_context!);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }

            _context = null;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }

            _context = null;
        }
    }
}
