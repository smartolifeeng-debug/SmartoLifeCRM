namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class Quotation : BusinessEntityBase
{
    public string QuotationNumber { get; set; } = string.Empty;

    public Guid? ContactId { get; set; }

    public Contact? Contact { get; set; }

    public Guid? ContactRoleId { get; set; }

    public ContactRole? ContactRole { get; set; }

    public DateTimeOffset QuotationDateUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? ValidUntilUtc { get; set; }

    public string? Subject { get; set; }

    public string? Reference { get; set; }

    public string Currency { get; set; } = "USD";

    public decimal Subtotal { get; set; }

    public decimal DiscountTotal { get; set; }

    public decimal TaxTotal { get; set; }

    public decimal GrandTotal { get; set; }

    public string Status { get; set; } = "Draft";

    public string? Notes { get; set; }

    public ICollection<QuotationItem> Items { get; set; } = new List<QuotationItem>();
}

