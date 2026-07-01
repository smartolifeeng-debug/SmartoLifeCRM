namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class QuotationItem : SoftDeleteEntityBase
{
    public Guid QuotationId { get; set; }

    public Quotation Quotation { get; set; } = null!;

    public Guid? ProductId { get; set; }

    public Product? Product { get; set; }

    public int LineNumber { get; set; } = 1;

    public string Description { get; set; } = string.Empty;

    public decimal Quantity { get; set; } = 1;

    public string? Unit { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxRate { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal LineTotal { get; set; }

    public string? Notes { get; set; }
}

