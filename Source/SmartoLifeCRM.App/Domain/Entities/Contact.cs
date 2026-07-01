namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class Contact : BusinessEntityBase
{
    public string? ContactName { get; set; }

    public string? CompanyName { get; set; }

    public string? ContactPerson { get; set; }

    public string? JobTitle { get; set; }

    public Guid? StatusId { get; set; }

    public ContactStatus? Status { get; set; }

    public string? PrimaryMobile { get; set; }

    public string? WhatsAppNumber { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public string? AdditionalPhone { get; set; }

    public string? Notes { get; set; }

    public Guid? OwnerUserId { get; set; }

    public string MarketingPermissionStatus { get; set; } = "Unknown";

    public bool DoNotContact { get; set; }

    public string? PreferredLanguage { get; set; }

    public string? PreferredCurrency { get; set; }

    public string? NormalizedName { get; set; }

    public string? NormalizedCompany { get; set; }

    public string? NormalizedEmail { get; set; }

    public string? NormalizedWebsiteDomain { get; set; }

    public ICollection<ContactRoleAssignment> RoleAssignments { get; set; } = new List<ContactRoleAssignment>();

    public ICollection<ContactCategory> Categories { get; set; } = new List<ContactCategory>();

    public ICollection<ContactTag> Tags { get; set; } = new List<ContactTag>();

    public ICollection<ContactBusinessMatter> BusinessMatters { get; set; } = new List<ContactBusinessMatter>();
}

