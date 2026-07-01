namespace SmartoLifeCRM.App.Domain.Entities;

public abstract class SoftDeleteEntityBase : EntityBase
{
    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAtUtc { get; set; }

    public Guid? DeletedByUserId { get; set; }
}

