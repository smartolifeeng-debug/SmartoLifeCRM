namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class ContactCategory : EntityBase
{
    public Guid ContactId { get; set; }

    public Contact Contact { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public bool IsPrimary { get; set; } = true;
}

