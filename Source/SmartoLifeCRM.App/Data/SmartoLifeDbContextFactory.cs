using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartoLifeCRM.App.Data;

public sealed class SmartoLifeDbContextFactory : IDesignTimeDbContextFactory<SmartoLifeDbContext>
{
    public SmartoLifeDbContext CreateDbContext(string[] args)
    {
        var appDataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SmartoLifeCRM");

        Directory.CreateDirectory(appDataDirectory);

        var options = new DbContextOptionsBuilder<SmartoLifeDbContext>()
            .UseSqlite($"Data Source={Path.Combine(appDataDirectory, "SmartoLifeCRM.db")}")
            .Options;

        return new SmartoLifeDbContext(options);
    }
}

