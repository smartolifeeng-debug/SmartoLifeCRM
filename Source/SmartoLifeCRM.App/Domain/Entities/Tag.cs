namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class Tag : SoftDeleteEntityBase
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? ColorHex { get; set; }

    public int SortOrder { get; set; }

    public ICollection<ContactTag> Contacts { get; set; } = new List<ContactTag>();
}

