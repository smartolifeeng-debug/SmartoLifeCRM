using Microsoft.EntityFrameworkCore;
using SmartoLifeCRM.App.Domain.Entities;

namespace SmartoLifeCRM.App.Data;

public sealed class SmartoLifeDbContext : DbContext
{
    public SmartoLifeDbContext(DbContextOptions<SmartoLifeDbContext> options)
        : base(options)
    {
    }

    public DbSet<CompanyProfile> CompanyProfiles => Set<CompanyProfile>();

    public DbSet<Contact> Contacts => Set<Contact>();

    public DbSet<ContactStatus> ContactStatuses => Set<ContactStatus>();

    public DbSet<ContactRole> ContactRoles => Set<ContactRole>();

    public DbSet<ContactRoleAssignment> ContactRoleAssignments => Set<ContactRoleAssignment>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<ContactCategory> ContactCategories => Set<ContactCategory>();

    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<ContactTag> ContactTags => Set<ContactTag>();

    public DbSet<BusinessMatter> BusinessMatters => Set<BusinessMatter>();

    public DbSet<ContactBusinessMatter> ContactBusinessMatters => Set<ContactBusinessMatter>();

    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

    public DbSet<ProductTag> ProductTags => Set<ProductTag>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<ProductTagAssignment> ProductTagAssignments => Set<ProductTagAssignment>();

    public DbSet<Quotation> Quotations => Set<Quotation>();

    public DbSet<QuotationItem> QuotationItems => Set<QuotationItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCompanyProfile(modelBuilder);
        ConfigureContacts(modelBuilder);
        ConfigureLookups(modelBuilder);
        ConfigureProductsAndQuotations(modelBuilder);
        ConfigureJoinTables(modelBuilder);
        SeedPhaseOneData(modelBuilder);
        SeedPhaseTwoMilestoneOneData(modelBuilder);
    }

    private static void ConfigureCompanyProfile(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyProfile>(entity =>
        {
            entity.Property(x => x.CompanyName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.OwnerFounder).HasMaxLength(200);
            entity.Property(x => x.LogoPath).HasMaxLength(500);
            entity.Property(x => x.Slogan).HasMaxLength(250);
            entity.Property(x => x.Email).HasMaxLength(254);
            entity.Property(x => x.Phone).HasMaxLength(30);
            entity.Property(x => x.WhatsApp).HasMaxLength(30);
            entity.Property(x => x.Website).HasMaxLength(300);
            entity.Property(x => x.BusinessRegistration).HasMaxLength(150);
            entity.Property(x => x.TaxNumber).HasMaxLength(150);
            entity.Property(x => x.Currency).HasMaxLength(10).IsRequired();
            entity.Property(x => x.Language).HasMaxLength(20).IsRequired();
            entity.Property(x => x.TimeZoneId).HasMaxLength(150).IsRequired();
            entity.HasIndex(x => x.CompanyName).IsUnique();
        });
    }

    private static void ConfigureContacts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.Property(x => x.ContactName).HasMaxLength(200);
            entity.Property(x => x.CompanyName).HasMaxLength(250);
            entity.Property(x => x.ContactPerson).HasMaxLength(200);
            entity.Property(x => x.JobTitle).HasMaxLength(150);
            entity.Property(x => x.PrimaryMobile).HasMaxLength(30);
            entity.Property(x => x.WhatsAppNumber).HasMaxLength(30);
            entity.Property(x => x.Email).HasMaxLength(254);
            entity.Property(x => x.Website).HasMaxLength(300);
            entity.Property(x => x.AdditionalPhone).HasMaxLength(60);
            entity.Property(x => x.MarketingPermissionStatus).HasMaxLength(30).IsRequired();
            entity.Property(x => x.PreferredLanguage).HasMaxLength(20);
            entity.Property(x => x.PreferredCurrency).HasMaxLength(10);
            entity.Property(x => x.NormalizedName).HasMaxLength(200);
            entity.Property(x => x.NormalizedCompany).HasMaxLength(250);
            entity.Property(x => x.NormalizedEmail).HasMaxLength(254);
            entity.Property(x => x.NormalizedWebsiteDomain).HasMaxLength(300);
            entity.Property(x => x.SyncState).HasMaxLength(30).IsRequired();
            entity.Property(x => x.SourceDeviceId).HasMaxLength(100);
            entity.HasIndex(x => x.ContactName);
            entity.HasIndex(x => x.CompanyName);
            entity.HasIndex(x => x.PrimaryMobile);
            entity.HasIndex(x => x.WhatsAppNumber);
            entity.HasIndex(x => x.Email);
            entity.HasIndex(x => x.NormalizedCompany);
            entity.HasIndex(x => x.NormalizedEmail);
            entity.HasIndex(x => x.IsDeleted);
            entity.HasIndex(x => x.IsDraft);
            entity.HasIndex(x => x.SyncState);
        });
    }

    private static void ConfigureLookups(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactStatus>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.HasIndex(x => x.Name).IsUnique();
            entity.HasIndex(x => x.SortOrder);
        });

        modelBuilder.Entity<ContactRole>(entity =>
        {
            entity.Property(x => x.RoleKey).HasMaxLength(80).IsRequired();
            entity.Property(x => x.DisplayName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.HasIndex(x => x.RoleKey).IsUnique();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.Property(x => x.ColorHex).HasMaxLength(7);
            entity.HasIndex(x => x.Name).IsUnique();
            entity.HasIndex(x => x.SortOrder);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.Property(x => x.ColorHex).HasMaxLength(7);
            entity.HasIndex(x => x.Name).IsUnique();
            entity.HasIndex(x => x.SortOrder);
        });

        modelBuilder.Entity<BusinessMatter>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.Property(x => x.ColorHex).HasMaxLength(7);
            entity.HasIndex(x => x.Name).IsUnique();
            entity.HasIndex(x => x.SortOrder);
            entity.HasIndex(x => x.IsPipelineState);
        });
    }

    private static void ConfigureProductsAndQuotations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.HasIndex(x => x.Name).IsUnique();
            entity.HasIndex(x => x.SortOrder);
        });

        modelBuilder.Entity<ProductTag>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.HasIndex(x => x.Name).IsUnique();
            entity.HasIndex(x => x.SortOrder);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(x => x.ProductName).HasMaxLength(250);
            entity.Property(x => x.ProductCode).HasMaxLength(80);
            entity.Property(x => x.PartNumber).HasMaxLength(100);
            entity.Property(x => x.SupplierPartNumber).HasMaxLength(100);
            entity.Property(x => x.Barcode).HasMaxLength(100);
            entity.Property(x => x.Brand).HasMaxLength(120);
            entity.Property(x => x.Unit).HasMaxLength(50);
            entity.Property(x => x.ProductImagePath).HasMaxLength(500);
            entity.Property(x => x.DatasheetFilePath).HasMaxLength(500);
            entity.Property(x => x.Status).HasMaxLength(30).IsRequired();
            entity.Property(x => x.SyncState).HasMaxLength(30).IsRequired();
            entity.Property(x => x.SourceDeviceId).HasMaxLength(100);
            entity.HasIndex(x => x.ProductName);
            entity.HasIndex(x => x.ProductCode).IsUnique();
            entity.HasIndex(x => x.PartNumber);
            entity.HasIndex(x => x.SupplierPartNumber);
            entity.HasIndex(x => x.Barcode).IsUnique();
            entity.HasIndex(x => x.Brand);
            entity.HasIndex(x => x.ProductCategoryId);
            entity.HasIndex(x => x.Status);
            entity.HasIndex(x => x.IsDeleted);
            entity.HasIndex(x => x.IsDraft);
            entity.HasOne(x => x.ProductCategory).WithMany(x => x.Products).HasForeignKey(x => x.ProductCategoryId);
        });

        modelBuilder.Entity<ProductTagAssignment>(entity =>
        {
            entity.HasIndex(x => new { x.ProductId, x.ProductTagId }).IsUnique();
            entity.HasOne(x => x.Product).WithMany(x => x.Tags).HasForeignKey(x => x.ProductId);
            entity.HasOne(x => x.ProductTag).WithMany(x => x.Products).HasForeignKey(x => x.ProductTagId);
        });

        modelBuilder.Entity<Quotation>(entity =>
        {
            entity.Property(x => x.QuotationNumber).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Subject).HasMaxLength(250);
            entity.Property(x => x.Reference).HasMaxLength(150);
            entity.Property(x => x.Currency).HasMaxLength(10).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(30).IsRequired();
            entity.Property(x => x.SyncState).HasMaxLength(30).IsRequired();
            entity.Property(x => x.SourceDeviceId).HasMaxLength(100);
            entity.HasIndex(x => x.QuotationNumber).IsUnique();
            entity.HasIndex(x => x.ContactId);
            entity.HasIndex(x => x.ContactRoleId);
            entity.HasIndex(x => x.QuotationDateUtc);
            entity.HasIndex(x => x.ValidUntilUtc);
            entity.HasIndex(x => x.Status);
            entity.HasIndex(x => x.IsDeleted);
            entity.HasIndex(x => x.IsDraft);
            entity.HasOne(x => x.Contact).WithMany().HasForeignKey(x => x.ContactId);
            entity.HasOne(x => x.ContactRole).WithMany().HasForeignKey(x => x.ContactRoleId);
        });

        modelBuilder.Entity<QuotationItem>(entity =>
        {
            entity.Property(x => x.Description).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.Unit).HasMaxLength(50);
            entity.Property(x => x.Notes).HasMaxLength(1000);
            entity.HasIndex(x => new { x.QuotationId, x.LineNumber }).IsUnique();
            entity.HasIndex(x => x.ProductId);
            entity.HasIndex(x => x.IsDeleted);
            entity.HasOne(x => x.Quotation).WithMany(x => x.Items).HasForeignKey(x => x.QuotationId);
            entity.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        });
    }

    private static void ConfigureJoinTables(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactRoleAssignment>(entity =>
        {
            entity.HasIndex(x => new { x.ContactId, x.RoleId }).IsUnique();
            entity.HasOne(x => x.Contact).WithMany(x => x.RoleAssignments).HasForeignKey(x => x.ContactId);
            entity.HasOne(x => x.Role).WithMany(x => x.Assignments).HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<ContactCategory>(entity =>
        {
            entity.HasIndex(x => new { x.ContactId, x.CategoryId }).IsUnique();
            entity.HasOne(x => x.Contact).WithMany(x => x.Categories).HasForeignKey(x => x.ContactId);
            entity.HasOne(x => x.Category).WithMany(x => x.Contacts).HasForeignKey(x => x.CategoryId);
        });

        modelBuilder.Entity<ContactTag>(entity =>
        {
            entity.HasIndex(x => new { x.ContactId, x.TagId }).IsUnique();
            entity.HasOne(x => x.Contact).WithMany(x => x.Tags).HasForeignKey(x => x.ContactId);
            entity.HasOne(x => x.Tag).WithMany(x => x.Contacts).HasForeignKey(x => x.TagId);
        });

        modelBuilder.Entity<ContactBusinessMatter>(entity =>
        {
            entity.HasIndex(x => new { x.ContactId, x.BusinessMatterId }).IsUnique();
            entity.HasOne(x => x.Contact).WithMany(x => x.BusinessMatters).HasForeignKey(x => x.ContactId);
            entity.HasOne(x => x.BusinessMatter).WithMany(x => x.Contacts).HasForeignKey(x => x.BusinessMatterId);
        });
    }

    private static void SeedPhaseOneData(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTimeOffset(2026, 7, 1, 0, 0, 0, TimeSpan.Zero);

        modelBuilder.Entity<CompanyProfile>().HasData(new CompanyProfile
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            CompanyName = "Smarto Life",
            OwnerFounder = "Mohammed Rafi",
            LogoPath = "Assets/Logo/SmartoLife_Logo.png",
            Slogan = "Smart Solutions for Tomorrow",
            Email = "Rafi@smartolife.com",
            Currency = "USD",
            Language = "en",
            TimeZoneId = TimeZoneInfo.Local.Id,
            CreatedAtUtc = seedDate,
            ModifiedAtUtc = seedDate
        });

        modelBuilder.Entity<ContactRole>().HasData(
            Role("22222222-2222-2222-2222-222222222201", "Customer", "Customer", "We send material or services to them.", 1, seedDate),
            Role("22222222-2222-2222-2222-222222222202", "Supplier", "Supplier", "They supply material or services to us.", 2, seedDate),
            Role("22222222-2222-2222-2222-222222222203", "Lead", "Lead", "Potential future customer or supplier.", 3, seedDate),
            Role("22222222-2222-2222-2222-222222222204", "Other", "Other", "Other contact relationship.", 4, seedDate));

        modelBuilder.Entity<ContactStatus>().HasData(
            Status("33333333-3333-3333-3333-333333333301", "Active", "Active business contact.", 1, seedDate),
            Status("33333333-3333-3333-3333-333333333302", "Inactive", "Inactive contact.", 2, seedDate),
            Status("33333333-3333-3333-3333-333333333303", "Draft", "Draft contact pending completion.", 3, seedDate));

        modelBuilder.Entity<Category>().HasData(
            Category("44444444-4444-4444-4444-444444444401", "Trader", 1, seedDate),
            Category("44444444-4444-4444-4444-444444444402", "Supermarket", 2, seedDate),
            Category("44444444-4444-4444-4444-444444444403", "Wholesaler", 3, seedDate),
            Category("44444444-4444-4444-4444-444444444404", "Distributor", 4, seedDate),
            Category("44444444-4444-4444-4444-444444444405", "Contractor", 5, seedDate),
            Category("44444444-4444-4444-4444-444444444406", "Other", 99, seedDate));

        modelBuilder.Entity<Tag>().HasData(
            Tag("55555555-5555-5555-5555-555555555501", "Shoes", 1, seedDate),
            Tag("55555555-5555-5555-5555-555555555502", "Perfume", 2, seedDate),
            Tag("55555555-5555-5555-5555-555555555503", "Mobile Phones", 3, seedDate),
            Tag("55555555-5555-5555-5555-555555555504", "CCTV", 4, seedDate),
            Tag("55555555-5555-5555-5555-555555555505", "Smart Home", 5, seedDate),
            Tag("55555555-5555-5555-5555-555555555506", "Accessories", 6, seedDate));

        modelBuilder.Entity<BusinessMatter>().HasData(
            Matter("66666666-6666-6666-6666-666666666601", "Inquiry", 1, true, seedDate),
            Matter("66666666-6666-6666-6666-666666666602", "Follow-up", 2, true, seedDate),
            Matter("66666666-6666-6666-6666-666666666603", "Marketing", 3, false, seedDate),
            Matter("66666666-6666-6666-6666-666666666604", "Interested", 4, true, seedDate),
            Matter("66666666-6666-6666-6666-666666666605", "Not Interested", 5, true, seedDate),
            Matter("66666666-6666-6666-6666-666666666606", "Supplier Contact", 6, false, seedDate));
    }

    private static void SeedPhaseTwoMilestoneOneData(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTimeOffset(2026, 7, 1, 0, 0, 0, TimeSpan.Zero);

        modelBuilder.Entity<ProductCategory>().HasData(
            ProductCategorySeed("77777777-7777-7777-7777-777777777701", "Electronics", 1, seedDate),
            ProductCategorySeed("77777777-7777-7777-7777-777777777702", "Smart Home", 2, seedDate),
            ProductCategorySeed("77777777-7777-7777-7777-777777777703", "Networking", 3, seedDate),
            ProductCategorySeed("77777777-7777-7777-7777-777777777704", "Accessories", 4, seedDate),
            ProductCategorySeed("77777777-7777-7777-7777-777777777705", "Other", 99, seedDate));

        modelBuilder.Entity<ProductTag>().HasData(
            ProductTagSeed("88888888-8888-8888-8888-888888888801", "CCTV", 1, seedDate),
            ProductTagSeed("88888888-8888-8888-8888-888888888802", "Fiber", 2, seedDate),
            ProductTagSeed("88888888-8888-8888-8888-888888888803", "Networking", 3, seedDate),
            ProductTagSeed("88888888-8888-8888-8888-888888888804", "LED Display", 4, seedDate),
            ProductTagSeed("88888888-8888-8888-8888-888888888805", "Digital Signage", 5, seedDate),
            ProductTagSeed("88888888-8888-8888-8888-888888888806", "Office Equipment", 6, seedDate));
    }

    private static ContactRole Role(string id, string key, string displayName, string description, int sortOrder, DateTimeOffset seedDate) =>
        new()
        {
            Id = Guid.Parse(id),
            RoleKey = key,
            DisplayName = displayName,
            Description = description,
            SortOrder = sortOrder,
            CreatedAtUtc = seedDate,
            ModifiedAtUtc = seedDate
        };

    private static ContactStatus Status(string id, string name, string description, int sortOrder, DateTimeOffset seedDate) =>
        new()
        {
            Id = Guid.Parse(id),
            Name = name,
            Description = description,
            SortOrder = sortOrder,
            IsSystem = true,
            CreatedAtUtc = seedDate,
            ModifiedAtUtc = seedDate
        };

    private static Category Category(string id, string name, int sortOrder, DateTimeOffset seedDate) =>
        new()
        {
            Id = Guid.Parse(id),
            Name = name,
            SortOrder = sortOrder,
            IsSystem = true,
            CreatedAtUtc = seedDate,
            ModifiedAtUtc = seedDate
        };

    private static Tag Tag(string id, string name, int sortOrder, DateTimeOffset seedDate) =>
        new()
        {
            Id = Guid.Parse(id),
            Name = name,
            SortOrder = sortOrder,
            CreatedAtUtc = seedDate,
            ModifiedAtUtc = seedDate
        };

    private static BusinessMatter Matter(string id, string name, int sortOrder, bool isPipelineState, DateTimeOffset seedDate) =>
        new()
        {
            Id = Guid.Parse(id),
            Name = name,
            SortOrder = sortOrder,
            IsPipelineState = isPipelineState,
            CreatedAtUtc = seedDate,
            ModifiedAtUtc = seedDate
        };

    private static ProductCategory ProductCategorySeed(string id, string name, int sortOrder, DateTimeOffset seedDate) =>
        new()
        {
            Id = Guid.Parse(id),
            Name = name,
            SortOrder = sortOrder,
            CreatedAtUtc = seedDate,
            ModifiedAtUtc = seedDate
        };

    private static ProductTag ProductTagSeed(string id, string name, int sortOrder, DateTimeOffset seedDate) =>
        new()
        {
            Id = Guid.Parse(id),
            Name = name,
            SortOrder = sortOrder,
            CreatedAtUtc = seedDate,
            ModifiedAtUtc = seedDate
        };
}

