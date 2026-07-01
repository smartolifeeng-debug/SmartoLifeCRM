namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class ProductCategory : SoftDeleteEntityBase
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}

