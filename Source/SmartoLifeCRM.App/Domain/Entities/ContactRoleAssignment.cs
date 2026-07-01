namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class ContactRoleAssignment : EntityBase
{
    public Guid ContactId { get; set; }

    public Contact Contact { get; set; } = null!;

    public Guid RoleId { get; set; }

    public ContactRole Role { get; set; } = null!;

    public bool IsPrimaryRole { get; set; }
}

