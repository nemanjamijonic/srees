using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SREES.API.Hubs;
using SREES.BLL.Mappings;
using SREES.BLL.Services.Implementation;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Helpers;
using SREES.Common.Services.Implementations;
using SREES.Common.Services.Interfaces;
using SREES.DAL.Context;
using SREES.DAL.Repository.Implementations;
using SREES.DAL.Repository.Interfaces;
using SREES.DAL.Seed;
using SREES.DAL.UOW.Implementations;
using SREES.DAL.UOW.Interafaces;
using SREES.Services.Implementations;
using SREES.Services.Interfaces;

namespace SREES.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // CORS policy za Angular frontend i SignalR
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
            
            // SignalR
            builder.Services.AddSignalR();
            builder.Services.AddScoped<INotificationHubService, NotificationHubService>();
            builder.Services.AddScoped<IRealTimeNotificationService, SignalRNotificationService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContextPool<SreesContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SreesDatabase"), sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    sqlServerOptions.CommandTimeout(200);
                }
            ));

            // JWT Authentication sa podrškom za SignalR
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = JwtManager.GetTokenValidationParameters();
                    
                    // Podrška za SignalR - token iz query stringa
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            
                            // Ako je zahtev za SignalR hub, uzmi token iz query stringa
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();

            // AutoMapper - Dodaj sve profile
            builder.Services.AddAutoMapper(typeof(OutageProfile), typeof(UserProfile), typeof(RegionProfile), typeof(SubstationProfile), typeof(PoleProfile), typeof(BuildingProfile), typeof(FeederProfile), typeof(CustomerProfile), typeof(NotificationProfile));

            builder.Services.AddScoped<ICachingService, CachingService>();
            RegisterRepositories(builder);
            RegisterBllServices(builder);
            RegisterApplicationServices(builder);

            var app = builder.Build();

            // Izvršavanje migracija na pokretanju aplikacije
            MigrateDatabase(app);

            // Seed podataka (samo u Development okruženju)
            if (app.Environment.IsDevelopment())
            {
                await SeedDatabaseAsync(app);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // CORS mora biti PRE UseHttpsRedirection
            app.UseCors(corsPolicyName);

            app.UseHttpsRedirection();

            // Authentication i Authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            
            // SignalR Hub endpoint
            app.MapHub<NotificationHub>("/hubs/notifications");

            app.Run();
        }

        private static void RegisterRepositories(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOutageRepository, OutageRepository>();
            builder.Services.AddScoped<IRegionRepository, RegionRepository>();
            builder.Services.AddScoped<ISubstationRepository, SubstationRepository>();
            builder.Services.AddScoped<IPoleRepository, PoleRepository>();
            builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
            builder.Services.AddScoped<IFeederRepository, FeederRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        }

        private static void RegisterBllServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOutageService, OutageService>();
            builder.Services.AddScoped<IOutageDetectionService, OutageDetectionService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IRegionService, RegionService>();
            builder.Services.AddScoped<ISubstationService, SubstationService>();
            builder.Services.AddScoped<IPoleService, PoleService>();
            builder.Services.AddScoped<IBuildingService, BuildingService>();
            builder.Services.AddScoped<IFeederService, FeederService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
        }

        private static void RegisterApplicationServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserApplicationService, UserApplicationService>();
            builder.Services.AddScoped<IOutageApplicationService, OutageApplicationService>();
            builder.Services.AddScoped<IRegionApplicationService, RegionApplicationService>();
            builder.Services.AddScoped<ISubstationApplicationService, SubstationApplicationService>();
            builder.Services.AddScoped<IPoleApplicationService, PoleApplicationService>();
            builder.Services.AddScoped<IBuildingApplicationService, BuildingApplicationService>();
            builder.Services.AddScoped<IFeederApplicationService, FeederApplicationService>();
            builder.Services.AddScoped<ICustomerApplicationService, CustomerApplicationService>();
            builder.Services.AddScoped<INotificationApplicationService, NotificationApplicationService>();
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

        private static async Task SeedDatabaseAsync(WebApplication app)
        {
            try
            {
                await DatabaseSeeder.SeedAsync(app.Services);
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Greška pri seed-ovanju baze podataka");
                // Ne prekidamo aplikaciju ako seed ne uspe
            }
        }
    }
}