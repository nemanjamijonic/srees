using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SREES.Common.Constants;
using SREES.DAL.Context;
using SREES.DAL.Models;

namespace SREES.DAL.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SreesContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SreesContext>>();

            try
            {
                await SeedRegionsAsync(context, logger);
                await SeedSubstationsAsync(context, logger);
                await SeedFeedersAsync(context, logger);
                await SeedPolesAsync(context, logger);
                await SeedBuildingsAsync(context, logger);
                await SeedCustomersAsync(context, logger);
                await SeedUsersAsync(context, logger);

                logger.LogInformation("Seed podaci uspešno dodati u bazu");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Greška pri dodavanju seed podataka");
                throw;
            }
        }

        private static async Task SeedRegionsAsync(SreesContext context, ILogger logger)
        {
            if (await context.Regions.AnyAsync())
            {
                logger.LogInformation("Regioni već postoje, preskačem seed");
                return;
            }

            var regions = new List<Region>
            {
                new Region { Name = "Novi Sad - Centar", Latitude = 45.2551, Longitude = 19.8451 },
                new Region { Name = "Novi Sad - Liman", Latitude = 45.2401, Longitude = 19.8301 },
                new Region { Name = "Novi Sad - Detelinara", Latitude = 45.2651, Longitude = 19.8151 },
                new Region { Name = "Novi Sad - Podbara", Latitude = 45.2601, Longitude = 19.8551 },
                new Region { Name = "Petrovaradin", Latitude = 45.2501, Longitude = 19.8651 }
            };

            await context.Regions.AddRangeAsync(regions);
            await context.SaveChangesAsync();
            logger.LogInformation("Dodato {Count} regiona", regions.Count);
        }

        private static async Task SeedSubstationsAsync(SreesContext context, ILogger logger)
        {
            if (await context.Substations.AnyAsync())
            {
                logger.LogInformation("Trafo-stanice već postoje, preskačem seed");
                return;
            }

            var regions = await context.Regions.ToListAsync();
            if (!regions.Any()) return;

            var substations = new List<Substation>
            {
                new Substation { Name = "TS Centar 1", SubstationType = SubstationType.DistributionSubstation, Latitude = 45.2555, Longitude = 19.8455, RegionId = regions[0].Id },
                new Substation { Name = "TS Centar 2", SubstationType = SubstationType.DistributionSubstation, Latitude = 45.2545, Longitude = 19.8445, RegionId = regions[0].Id },
                new Substation { Name = "TS Liman", SubstationType = SubstationType.TransmissionSubstation, Latitude = 45.2405, Longitude = 19.8305, RegionId = regions[1].Id },
                new Substation { Name = "TS Detelinara", SubstationType = SubstationType.DistributionSubstation, Latitude = 45.2655, Longitude = 19.8155, RegionId = regions[2].Id },
                new Substation { Name = "TS Podbara", SubstationType = SubstationType.DistributionSubstation, Latitude = 45.2605, Longitude = 19.8555, RegionId = regions[3].Id },
                new Substation { Name = "TS Petrovaradin", SubstationType = SubstationType.InjectionSubstation, Latitude = 45.2505, Longitude = 19.8655, RegionId = regions[4].Id }
            };

            await context.Substations.AddRangeAsync(substations);
            await context.SaveChangesAsync();
            logger.LogInformation("Dodato {Count} trafo-stanica", substations.Count);
        }

        private static async Task SeedFeedersAsync(SreesContext context, ILogger logger)
        {
            if (await context.Feeders.AnyAsync())
            {
                logger.LogInformation("Fideri već postoje, preskačem seed");
                return;
            }

            var substations = await context.Substations.ToListAsync();
            if (!substations.Any()) return;

            var feeders = new List<Feeder>
            {
                new Feeder { Name = "Fider Centar A", FeederType = FeederType.Feeder11, SubstationId = substations[0].Id, Latitude = 45.2560, Longitude = 19.8460 },
                new Feeder { Name = "Fider Centar B", FeederType = FeederType.Feeder33, SubstationId = substations[0].Id, Latitude = 45.2550, Longitude = 19.8450 },
                new Feeder { Name = "Fider Liman A", FeederType = FeederType.Feeder11, SubstationId = substations[2].Id, Latitude = 45.2410, Longitude = 19.8310 },
                new Feeder { Name = "Fider Detelinara", FeederType = FeederType.Feeder11, SubstationId = substations[3].Id, Latitude = 45.2660, Longitude = 19.8160 },
                new Feeder { Name = "Fider Podbara", FeederType = FeederType.Feeder33, SubstationId = substations[4].Id, Latitude = 45.2610, Longitude = 19.8560 },
                new Feeder { Name = "Fider Petrovaradin", FeederType = FeederType.Feeder11, SubstationId = substations[5].Id, Latitude = 45.2510, Longitude = 19.8660 }
            };

            await context.Feeders.AddRangeAsync(feeders);
            await context.SaveChangesAsync();
            logger.LogInformation("Dodato {Count} fidera", feeders.Count);
        }

        private static async Task SeedPolesAsync(SreesContext context, ILogger logger)
        {
            if (await context.Poles.AnyAsync())
            {
                logger.LogInformation("Stubovi već postoje, preskačem seed");
                return;
            }

            var regions = await context.Regions.ToListAsync();
            if (!regions.Any()) return;

            var poles = new List<Pole>
            {
                // Centar
                new Pole { Name = "Stub C-001", Latitude = 45.2553, Longitude = 19.8453, Address = "Bulevar Oslobođenja 1", PoleType = PoleType.LvPole, RegionId = regions[0].Id },
                new Pole { Name = "Stub C-002", Latitude = 45.2557, Longitude = 19.8457, Address = "Bulevar Oslobođenja 5", PoleType = PoleType.MvPole, RegionId = regions[0].Id },
                new Pole { Name = "Stub C-003", Latitude = 45.2548, Longitude = 19.8448, Address = "Jevrejska 10", PoleType = PoleType.HvPole, RegionId = regions[0].Id },
                // Liman
                new Pole { Name = "Stub L-001", Latitude = 45.2403, Longitude = 19.8303, Address = "Narodnog Fronta 20", PoleType = PoleType.LvPole, RegionId = regions[1].Id },
                new Pole { Name = "Stub L-002", Latitude = 45.2407, Longitude = 19.8307, Address = "Narodnog Fronta 30", PoleType = PoleType.MvPole, RegionId = regions[1].Id },
                // Detelinara
                new Pole { Name = "Stub D-001", Latitude = 45.2653, Longitude = 19.8153, Address = "Kornelija Stankovića 15", PoleType = PoleType.LvPole, RegionId = regions[2].Id },
                new Pole { Name = "Stub D-002", Latitude = 45.2657, Longitude = 19.8157, Address = "Kornelija Stankovića 25", PoleType = PoleType.HvPole, RegionId = regions[2].Id },
                // Podbara
                new Pole { Name = "Stub P-001", Latitude = 45.2603, Longitude = 19.8553, Address = "Jovana Subotića 5", PoleType = PoleType.MvPole, RegionId = regions[3].Id },
                // Petrovaradin
                new Pole { Name = "Stub PV-001", Latitude = 45.2503, Longitude = 19.8653, Address = "Preradovićeva 10", PoleType = PoleType.LvPole, RegionId = regions[4].Id },
                new Pole { Name = "Stub PV-002", Latitude = 45.2507, Longitude = 19.8657, Address = "Preradovićeva 20", PoleType = PoleType.MvPole, RegionId = regions[4].Id }
            };

            await context.Poles.AddRangeAsync(poles);
            await context.SaveChangesAsync();
            logger.LogInformation("Dodato {Count} stubova", poles.Count);
        }

        private static async Task SeedBuildingsAsync(SreesContext context, ILogger logger)
        {
            if (await context.Buildings.AnyAsync())
            {
                logger.LogInformation("Zgrade već postoje, preskačem seed");
                return;
            }

            var regions = await context.Regions.ToListAsync();
            var poles = await context.Poles.ToListAsync();
            if (!regions.Any() || !poles.Any()) return;

            var buildings = new List<Building>
            {
                new Building { Latitude = 45.2554, Longitude = 19.8454, OwnerName = "Stambena zgrada Centar 1", Address = "Bulevar Oslobođenja 2", RegionId = regions[0].Id, PoleId = poles[0].Id },
                new Building { Latitude = 45.2556, Longitude = 19.8456, OwnerName = "Poslovna zgrada Centar", Address = "Bulevar Oslobođenja 4", RegionId = regions[0].Id, PoleId = poles[1].Id },
                new Building { Latitude = 45.2404, Longitude = 19.8304, OwnerName = "Stambena zgrada Liman", Address = "Narodnog Fronta 22", RegionId = regions[1].Id, PoleId = poles[3].Id },
                new Building { Latitude = 45.2654, Longitude = 19.8154, OwnerName = "Stambena zgrada Detelinara", Address = "Kornelija Stankovića 17", RegionId = regions[2].Id, PoleId = poles[5].Id },
                new Building { Latitude = 45.2604, Longitude = 19.8554, OwnerName = "Kuća Podbara", Address = "Jovana Subotića 7", RegionId = regions[3].Id, PoleId = poles[7].Id },
                new Building { Latitude = 45.2504, Longitude = 19.8654, OwnerName = "Vila Petrovaradin", Address = "Preradovićeva 12", RegionId = regions[4].Id, PoleId = poles[8].Id }
            };

            await context.Buildings.AddRangeAsync(buildings);
            await context.SaveChangesAsync();
            logger.LogInformation("Dodato {Count} zgrada", buildings.Count);
        }

        private static async Task SeedCustomersAsync(SreesContext context, ILogger logger)
        {
            if (await context.Customers.AnyAsync())
            {
                logger.LogInformation("Kupci već postoje, preskačem seed");
                return;
            }

            var buildings = await context.Buildings.ToListAsync();
            if (!buildings.Any()) return;

            var customers = new List<Customer>
            {
                new Customer { FirstName = "Petar", LastName = "Petrović", Address = "Bulevar Oslobođenja 2, Stan 5", CustomerType = CustomerType.Residential, IsActive = true, BuildingId = buildings[0].Id },
                new Customer { FirstName = "Marko", LastName = "Marković", Address = "Bulevar Oslobođenja 2, Stan 10", CustomerType = CustomerType.Residential, IsActive = true, BuildingId = buildings[0].Id },
                new Customer { FirstName = "Tech Solutions", LastName = "d.o.o.", Address = "Bulevar Oslobođenja 4", CustomerType = CustomerType.Commercial, IsActive = true, BuildingId = buildings[1].Id },
                new Customer { FirstName = "Jovan", LastName = "Jovanović", Address = "Narodnog Fronta 22, Stan 3", CustomerType = CustomerType.Residential, IsActive = true, BuildingId = buildings[2].Id },
                new Customer { FirstName = "Ana", LastName = "Anić", Address = "Kornelija Stankovića 17, Stan 1", CustomerType = CustomerType.Residential, IsActive = true, BuildingId = buildings[3].Id },
                new Customer { FirstName = "Nikola", LastName = "Nikolić", Address = "Jovana Subotića 7", CustomerType = CustomerType.Residential, IsActive = true, BuildingId = buildings[4].Id },
                new Customer { FirstName = "Industrija", LastName = "a.d.", Address = "Preradovićeva 12", CustomerType = CustomerType.Industrial, IsActive = true, BuildingId = buildings[5].Id }
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
            logger.LogInformation("Dodato {Count} kupaca", customers.Count);
        }

        private static async Task SeedUsersAsync(SreesContext context, ILogger logger)
        {
            if (await context.Users.AnyAsync())
            {
                logger.LogInformation("Korisnici već postoje, preskačem seed");
                return;
            }

            // Password: "Admin123!" i "User123!" - BCrypt hash
            var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
            var userPasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!");

            var users = new List<User>
            {
                new User { FirstName = "Admin", LastName = "Administrator", Email = "admin@srees.rs", PasswordHash = adminPasswordHash, Role = Role.Admin },
                new User { FirstName = "Operater", LastName = "Operater", Email = "operater@srees.rs", PasswordHash = userPasswordHash, Role = Role.User },
                new User { FirstName = "Petar", LastName = "Petrović", Email = "petar@email.com", PasswordHash = userPasswordHash, Role = Role.User },
                new User { FirstName = "Marko", LastName = "Marković", Email = "marko@email.com", PasswordHash = userPasswordHash, Role = Role.User }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
            logger.LogInformation("Dodato {Count} korisnika", users.Count);
        }
    }
}
