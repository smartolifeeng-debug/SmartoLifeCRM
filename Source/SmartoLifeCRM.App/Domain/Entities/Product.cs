namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class Product : BusinessEntityBase
{
    public string? ProductName { get; set; }

    public string? ProductCode { get; set; }

    public string? PartNumber { get; set; }

    public string? SupplierPartNumber { get; set; }

    public string? Barcode { get; set; }

    public string? Brand { get; set; }

    public Guid? ProductCategoryId { get; set; }

    public ProductCategory? ProductCategory { get; set; }

    public string? Description { get; set; }

    public string? Unit { get; set; } = "Piece";

    public decimal? CostPrice { get; set; }

    public decimal? SellingPrice { get; set; }

    public decimal StockQuantity { get; set; }

    public string? ProductImagePath { get; set; }

    public string? DatasheetFilePath { get; set; }

    public string Status { get; set; } = "Active";

    public string? Notes { get; set; }

    public ICollection<ProductTagAssignment> Tags { get; set; } = new List<ProductTagAssignment>();
}

