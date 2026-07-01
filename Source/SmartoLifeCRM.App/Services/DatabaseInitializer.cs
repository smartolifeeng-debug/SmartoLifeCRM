using Microsoft.EntityFrameworkCore;
using SmartoLifeCRM.App.Data;

namespace SmartoLifeCRM.App.Services;

public sealed class DatabaseInitializer : IDatabaseInitializer
{
    private readonly SmartoLifeDbContext _dbContext;

    public DatabaseInitializer(SmartoLifeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        // Phase 1 creates the local SQLite schema so the shell can start immediately.
        // EF migrations are enabled through the design-time DbContext factory.
        await _dbContext.Database.EnsureCreatedAsync(cancellationToken);

        if (!await _dbContext.CompanyProfiles.AnyAsync(cancellationToken))
        {
            _dbContext.CompanyProfiles.Add(new()
            {
                CompanyName = "Smarto Life",
                OwnerFounder = "Mohammed Rafi",
                LogoPath = "Assets/Logo/SmartoLife_Logo.png",
                Slogan = "Smart Solutions for Tomorrow",
                Email = "Rafi@smartolife.com",
                Currency = "USD",
                Language = "en",
                TimeZoneId = TimeZoneInfo.Local.Id
            });

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

