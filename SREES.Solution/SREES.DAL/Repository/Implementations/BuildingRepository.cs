using Microsoft.EntityFrameworkCore;
using SREES.Common.Models.Dtos.Buildings;
using SREES.Common.Repositories.Implementations;
using SREES.DAL.Context;
using SREES.DAL.Models;
using SREES.DAL.Repository.Interfaces;

namespace SREES.DAL.Repository.Implementations
{
    public class BuildingRepository : Repository<Building>, IBuildingRepository
    {
        public SreesContext Context 
        {
            get { return _context is SreesContext ? (SreesContext)_context : throw new TypeLoadException("_context is not type of SreesContext"); }
        }

        public BuildingRepository(SreesContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Building>> GetAllWithPoleAsync()
        {
            return await Context.Buildings
                .Include(b => b.Pole)
                .Where(b => !b.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> GetTotalBuildingCountAsync()
        {
            return await Context.Buildings
                .Where(b => !b.IsDeleted)
                .CountAsync();
        }

        public async Task<(IEnumerable<Building> Buildings, int TotalCount)> GetBuildingsFilteredAsync(BuildingFilterRequest filterRequest)
        {
            var query = Context.Buildings
                .Include(b => b.Pole)
                .Where(b => !b.IsDeleted);

            // Apply search term filter (search by owner name or address)
            if (!string.IsNullOrWhiteSpace(filterRequest.SearchTerm))
            {
                var searchTerm = filterRequest.SearchTerm.ToLower();
                query = query.Where(b =>
                    (b.OwnerName != null && b.OwnerName.ToLower().Contains(searchTerm)) ||
                    (b.Address != null && b.Address.ToLower().Contains(searchTerm)));
            }

            // Apply pole type filter
            if (filterRequest.PoleType.HasValue)
            {
                query = query.Where(b => b.Pole != null && b.Pole.PoleType == filterRequest.PoleType.Value);
            }

            // Apply date from filter
            if (filterRequest.DateFrom.HasValue)
            {
                query = query.Where(b => b.CreatedAt >= filterRequest.DateFrom.Value);
            }

            // Apply date to filter
            if (filterRequest.DateTo.HasValue)
            {
                var dateTo = filterRequest.DateTo.Value.AddDays(1); // Include the entire day
                query = query.Where(b => b.CreatedAt < dateTo);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var buildings = await query
                .OrderByDescending(b => b.CreatedAt)
                .Skip((filterRequest.PageNumber - 1) * filterRequest.PageSize)
                .Take(filterRequest.PageSize)
                .ToListAsync();

            return (buildings, totalCount);
        }
    }
}
