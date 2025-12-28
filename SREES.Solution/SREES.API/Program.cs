using Microsoft.EntityFrameworkCore;
using SREES.BLL.Mappings;
using SREES.BLL.Services.Implementation;
using SREES.BLL.Services.Interfaces;
using SREES.DAL.Context;
using SREES.DAL.Repository.Implementations;
using SREES.DAL.Repository.Interfaces;
using SREES.DAL.UOW.Implementations;
using SREES.DAL.UOW.Interafaces;
using SREES.Services.Implementations;
using SREES.Services.Interfaces;

namespace SREES.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // CORS policy za Angular frontend
            const string corsPolicyName = "AllowAngularApp";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName, policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:4200",
                        "https://localhost:4200",
                        "http://localhost:4201",
                        "https://localhost:4201"
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition");
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContextPool<SreesContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SreesDatabase"), sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    sqlServerOptions.CommandTimeout(200);
                }
            ));

            // AutoMapper - Dodaj sve profile
            builder.Services.AddAutoMapper(typeof(OutageProfile), typeof(UserProfile), typeof(RegionProfile), typeof(SubstationProfile));

            RegisterRepositories(builder);
            RegisterBllServices(builder);
            RegisterApplicationServices(builder);

            var app = builder.Build();

            // Izvršavanje migracija na pokretanju aplikacije
            MigrateDatabase(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // CORS mora biti PRE UseHttpsRedirection
            app.UseCors(corsPolicyName);

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void RegisterRepositories(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOutageRepository, OutageRepository>();
            builder.Services.AddScoped<IRegionRepository, RegionRepository>();
            builder.Services.AddScoped<ISubstationRepository, SubstationRepository>();
        }

        private static void RegisterBllServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOutageService, OutageService>();
            builder.Services.AddScoped<IRegionService, RegionService>();
            builder.Services.AddScoped<ISubstationService, SubstationService>();
        }

        private static void RegisterApplicationServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserApplicationService, UserApplicationService>();
            builder.Services.AddScoped<IOutageApplicationService, OutageApplicationService>();
            builder.Services.AddScoped<IRegionApplicationService, RegionApplicationService>();
            builder.Services.AddScoped<ISubstationApplicationService, SubstationApplicationService>();
        }

        private static void MigrateDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SreesContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Greška pri izvršavanju migracija baze podataka");
                    throw;
                }
            }
        }
    }
}