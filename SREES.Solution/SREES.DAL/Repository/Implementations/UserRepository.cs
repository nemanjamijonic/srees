using Microsoft.EntityFrameworkCore;
using SREES.Common.Repositories.Implementations;
using SREES.DAL.Context;
using SREES.DAL.Models;
using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.Repository.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public SreesContext Context 
        {
            get { return _context is SreesContext ? (SreesContext)_context : throw new TypeLoadException("_context is not type of SreesContext"); }
        }

        public UserRepository(SreesContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await Context.Users
                .Where(u => !u.IsDeleted && u.Email == email)
                .FirstOrDefaultAsync();
        }
    }
}
