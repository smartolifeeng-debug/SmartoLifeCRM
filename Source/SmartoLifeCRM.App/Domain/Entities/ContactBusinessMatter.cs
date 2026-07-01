namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class ContactBusinessMatter : EntityBase
{
    public Guid ContactId { get; set; }

    public Contact Contact { get; set; } = null!;

    public Guid BusinessMatterId { get; set; }

    public BusinessMatter BusinessMatter { get; set; } = null!;

    public DateTimeOffset AssignedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}

