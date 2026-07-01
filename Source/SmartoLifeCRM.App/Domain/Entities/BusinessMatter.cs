namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class BusinessMatter : SoftDeleteEntityBase
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? ColorHex { get; set; }

    public int SortOrder { get; set; }

    public bool IsPipelineState { get; set; }

    public ICollection<ContactBusinessMatter> Contacts { get; set; } = new List<ContactBusinessMatter>();
}

