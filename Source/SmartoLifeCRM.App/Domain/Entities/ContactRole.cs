namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class ContactRole : EntityBase
{
    public string RoleKey { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public ICollection<ContactRoleAssignment> Assignments { get; set; } = new List<ContactRoleAssignment>();
}

