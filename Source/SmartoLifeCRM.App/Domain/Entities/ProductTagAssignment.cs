namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class ProductTagAssignment : EntityBase
{
    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public Guid ProductTagId { get; set; }

    public ProductTag ProductTag { get; set; } = null!;
}

