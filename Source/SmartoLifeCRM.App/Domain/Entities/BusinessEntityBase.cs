namespace SmartoLifeCRM.App.Domain.Entities;

public abstract class BusinessEntityBase : SoftDeleteEntityBase
{
    public bool IsDraft { get; set; }

    public int RecordVersion { get; set; } = 1;

    public string? PublicCode { get; set; }

    public string SyncState { get; set; } = "LocalOnly";

    public DateTimeOffset? LastSyncedAtUtc { get; set; }

    public string? SourceDeviceId { get; set; }
}

