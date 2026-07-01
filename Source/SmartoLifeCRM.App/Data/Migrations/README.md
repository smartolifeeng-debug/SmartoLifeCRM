# EF Core Migrations

This folder is reserved for Entity Framework Core migrations.

Phase 1 includes:

- `SmartoLifeDbContext`
- SQLite configuration
- Design-time DbContext factory
- Database startup initialization

After local restore succeeds, create the first migration with:

```powershell
dotnet tool restore
dotnet ef migrations add InitialPhaseOne --project Source\SmartoLifeCRM.App\SmartoLifeCRM.App.csproj --startup-project Source\SmartoLifeCRM.App\SmartoLifeCRM.App.csproj --output-dir Data\Migrations
```

