namespace SmartoLifeCRM.App.Domain.Entities;

public sealed class CompanyProfile : EntityBase
{
    public string CompanyName { get; set; } = "Smarto Life";

    public string? OwnerFounder { get; set; } = "Mohammed Rafi";

    public string? LogoPath { get; set; } = "Assets/Logo/SmartoLife_Logo.png";

    public string? Slogan { get; set; } = "Smart Solutions for Tomorrow";

    public string? Description { get; set; }

    public string? Email { get; set; } = "Rafi@smartolife.com";

    public string? Phone { get; set; }

    public string? WhatsApp { get; set; }

    public string? Website { get; set; }

    public string? Address { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? BusinessRegistration { get; set; }

    public string? TaxNumber { get; set; }

    public string Currency { get; set; } = "USD";

    public string Language { get; set; } = "en";

    public string TimeZoneId { get; set; } = TimeZoneInfo.Local.Id;

    public string? Domain { get; set; }

    public string? VpsInformationJson { get; set; }

    public string? SmtpSettingsJson { get; set; }

    public string? BackupSettingsJson { get; set; }

    public string? FutureApiSettingsJson { get; set; }
}

