namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class ContactTag : EntityBase
{
    public Guid ContactId { get; set; }

    public Contact Contact { get; set; } = null!;

    public Guid TagId { get; set; }

    public Tag Tag { get; set; } = null!;
}

