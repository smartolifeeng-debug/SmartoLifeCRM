# Smarto Life CRM Data Dictionary

## Purpose

This document defines the planned SQLite + Entity Framework Core database schema for Smarto Life CRM. It is a Phase 0 planning document only; it is not production code.

The schema follows the Master Specification:

- One unified contact structure for customers, suppliers, leads, and other roles.
- Local-first and offline-first SQLite storage.
- EF Core migration-friendly relational design.
- Dynamic categories, tags, business matters, settings, and custom fields.
- Soft delete, draft saving, audit logging, recycle bin, versioning, duplicate detection, merge history, and future sync readiness.
- International-ready Unicode text, phone numbers, addresses, currencies, and time zone-aware timestamps.

## SQLite and EF Core Type Rules

| Logical Type | SQLite Storage | EF Core Type | Notes |
|---|---:|---|---|
| Identifier | TEXT | Guid | Store GUIDs as text for stable offline/sync IDs. |
| String | TEXT | string | Unicode text. |
| Long Text | TEXT | string | Notes, JSON, extracted OCR text, templates. |
| Boolean | INTEGER | bool | 0 = false, 1 = true. |
| Integer | INTEGER | int / long | Counts, quantities where whole numbers are required. |
| Decimal Money | NUMERIC | decimal | Prices, totals, tax, discount. |
| Floating Point | REAL | double | GPS latitude, longitude, accuracy, confidence scores. |
| DateTime | TEXT | DateTimeOffset | ISO-8601 UTC unless explicitly local display. |
| JSON | TEXT | string / owned value converter | Flexible settings, mappings, metadata. |
| File Path | TEXT | string | Relative path under managed app storage where possible. |

## Standard Field Sets

The fields below are reused across many tables. When a table says it includes a standard field set, the field definitions in this section apply.

### StandardEntityFields

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Id | TEXT Guid | Required | New GUID | Must be unique and non-empty. | Primary technical key. | PK, unique index. |
| CreatedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Must be valid UTC timestamp. | Record creation timestamp. | Index where used in lists/reports. |
| ModifiedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Must be valid UTC timestamp and >= CreatedAtUtc. | Last modification timestamp. | Index where used in sync. |
| CreatedByUserId | TEXT Guid | Optional | NULL | Must reference existing user when available. | User who created the record. | FK to Users.Id, indexed. |
| ModifiedByUserId | TEXT Guid | Optional | NULL | Must reference existing user when available. | User who last modified the record. | FK to Users.Id, indexed. |

### SoftDeleteFields

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| IsDeleted | INTEGER Boolean | Required | 0 | Must be 0 or 1. | Soft delete flag. | Indexed. |
| DeletedAtUtc | TEXT DateTimeOffset | Optional | NULL | Required when IsDeleted = 1. | Soft delete timestamp. | Index for recycle bin. |
| DeletedByUserId | TEXT Guid | Optional | NULL | Must reference existing user when available. | User who deleted the record. | FK to Users.Id. |

### DraftVersionSyncFields

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| IsDraft | INTEGER Boolean | Required | 0 | Must be 0 or 1. | Allows partial draft records. | Indexed for draft filters. |
| RecordVersion | INTEGER | Required | 1 | Must be >= 1 and increment on important edits. | Optimistic version and history support. | Used for conflict handling. |
| PublicCode | TEXT | Optional | NULL | Must be unique inside table when present. | Human-readable code or number. | Unique filtered index where used. |
| SyncState | TEXT | Required | LocalOnly | Allowed values: LocalOnly, PendingCreate, PendingUpdate, PendingDelete, Synced, Conflict. | Future sync state. | Indexed. |
| LastSyncedAtUtc | TEXT DateTimeOffset | Optional | NULL | Valid UTC timestamp. | Last successful sync time. | Indexed for future sync. |
| SourceDeviceId | TEXT | Optional | NULL | Max 100 chars. | Device/source for future sync conflict tracing. | Indexed where needed. |

## Identity, Security, and Permissions

### Users

Stores login accounts and future staff ownership.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| UserName | TEXT | Required | None | 3-80 chars, unique, trimmed. | Login/display username. | Unique index. |
| FullName | TEXT | Required | None | 1-150 chars. | Staff member full name. | Indexed. |
| Email | TEXT | Optional | NULL | Valid email if present, max 254 chars. | User email. | Unique filtered index. |
| Mobile | TEXT | Optional | NULL | E.164 preferred, max 30 chars. | User phone/mobile. | Indexed. |
| PasswordHash | TEXT | Required | None | Must be generated by approved password hasher. | Password hash, never plain text. | None. |
| PasswordSalt | TEXT | Optional | NULL | Required only if password hasher needs external salt. | Salt metadata. | None. |
| IsActive | INTEGER Boolean | Required | 1 | 0 or 1. | Allows disabling login. | Indexed. |
| LastLoginAtUtc | TEXT DateTimeOffset | Optional | NULL | Valid UTC timestamp. | Last login time. | Indexed. |
| PreferredLanguage | TEXT | Optional | NULL | ISO language code, max 20 chars. | User language preference. | None. |
| TimeZoneId | TEXT | Optional | NULL | Valid OS/IANA time zone ID where possible. | User time zone preference. | None. |

### Roles

Defines Admin, Staff, and future roles.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | 2-80 chars, unique. | Role name. | Unique index. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Role purpose. | None. |
| IsSystemRole | INTEGER Boolean | Required | 0 | 0 or 1. | Prevents accidental deletion of built-in roles. | Indexed. |

### Permissions

Defines module and action permissions.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ModuleKey | TEXT | Required | None | 2-80 chars. | Module, such as Contacts or Backup. | Composite unique with ActionKey. |
| ActionKey | TEXT | Required | None | 2-80 chars. | Action, such as View, Edit, Delete, Restore, Export. | Composite unique with ModuleKey. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Permission explanation. | None. |

### UserRoles

Many-to-many user role assignment.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| UserId | TEXT Guid | Required | None | Must reference Users.Id. | Assigned user. | FK Users.Id, composite PK with RoleId. |
| RoleId | TEXT Guid | Required | None | Must reference Roles.Id. | Assigned role. | FK Roles.Id, composite PK with UserId. |
| AssignedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Assignment timestamp. | Indexed. |
| AssignedByUserId | TEXT Guid | Optional | NULL | Must reference Users.Id when present. | Admin who assigned role. | FK Users.Id. |

### RolePermissions

Many-to-many role permission assignment.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| RoleId | TEXT Guid | Required | None | Must reference Roles.Id. | Role. | FK Roles.Id, composite PK with PermissionId. |
| PermissionId | TEXT Guid | Required | None | Must reference Permissions.Id. | Permission. | FK Permissions.Id, composite PK with RoleId. |
| AssignedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Assignment timestamp. | Indexed. |

## Unified Contact Module

### Contacts

Unified master table for customers, suppliers, leads, and other contact records.

Includes: StandardEntityFields, SoftDeleteFields, DraftVersionSyncFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ContactName | TEXT | Required unless IsDraft = 1 | NULL | Max 200 chars, trimmed. | Main contact display name. | Indexed for search. |
| CompanyName | TEXT | Optional | NULL | Max 250 chars, trimmed. | Company/business name. | Indexed; normalized duplicate check. |
| ContactPerson | TEXT | Optional | NULL | Max 200 chars. | Person inside company. | Indexed. |
| JobTitle | TEXT | Optional | NULL | Max 150 chars. | Contact person's title. | None. |
| StatusId | TEXT Guid | Optional | NULL | Must reference ContactStatuses.Id. | Contact lifecycle/status. | FK ContactStatuses.Id, indexed. |
| PrimaryMobile | TEXT | Optional | NULL | E.164 preferred, max 30 chars. | Mobile number. | Indexed; duplicate check. |
| WhatsAppNumber | TEXT | Optional | NULL | E.164 preferred, max 30 chars. | WhatsApp number. | Indexed; duplicate check. |
| Email | TEXT | Optional | NULL | Valid email, max 254 chars. | Email address. | Indexed; duplicate check. |
| Website | TEXT | Optional | NULL | Valid URL/domain, max 300 chars. | Website. | Indexed by normalized domain. |
| AdditionalPhone | TEXT | Optional | NULL | Max 60 chars. | Extra phone/landline. | Indexed. |
| Notes | TEXT | Optional | NULL | No hard limit beyond SQLite practical size. | General notes. | Full-text index later. |
| OwnerUserId | TEXT Guid | Optional | NULL | Must reference Users.Id. | Assigned user/contact owner. | FK Users.Id, indexed. |
| MarketingPermissionStatus | TEXT | Required | Unknown | Allowed: Unknown, Granted, Denied, OptedOut. | Current WhatsApp/marketing consent state. | Indexed. |
| DoNotContact | INTEGER Boolean | Required | 0 | 0 or 1. | Blocks marketing/contacting workflows. | Indexed. |
| PreferredLanguage | TEXT | Optional | NULL | ISO code, max 20 chars. | Contact language preference. | None. |
| PreferredCurrency | TEXT | Optional | NULL | ISO currency code, max 10 chars. | Future quotation/report preference. | None. |
| NormalizedName | TEXT | Optional | NULL | Generated by app from ContactName. | Duplicate/search helper. | Indexed. |
| NormalizedCompany | TEXT | Optional | NULL | Generated by app from CompanyName. | Duplicate/search helper. | Indexed. |
| NormalizedEmail | TEXT | Optional | NULL | Lowercase email. | Duplicate/search helper. | Indexed. |
| NormalizedWebsiteDomain | TEXT | Optional | NULL | Domain only. | Duplicate/search helper. | Indexed. |

### ContactStatuses

Dynamic status lookup.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | 2-100 chars, unique among active rows. | Status name, such as Active or Inactive. | Unique filtered index. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Status explanation. | None. |
| SortOrder | INTEGER | Required | 0 | >= 0. | Display ordering. | Indexed. |
| IsSystem | INTEGER Boolean | Required | 0 | 0 or 1. | Built-in status flag. | Indexed. |

### ContactRoles

Role lookup for unified contacts.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| RoleKey | TEXT | Required | None | Allowed initial keys: Customer, Supplier, Lead, Other. | Stable role key. | Unique index. |
| DisplayName | TEXT | Required | None | 2-100 chars. | UI label. | Indexed. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Role meaning. | None. |
| SortOrder | INTEGER | Required | 0 | >= 0. | Display order. | Indexed. |

### ContactRoleAssignments

Allows one contact to be Customer, Supplier, Lead, and/or Other.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ContactId | TEXT Guid | Required | None | Must reference Contacts.Id. | Contact. | FK Contacts.Id, composite unique with RoleId, indexed. |
| RoleId | TEXT Guid | Required | None | Must reference ContactRoles.Id. | Assigned role. | FK ContactRoles.Id, composite unique with ContactId. |
| IsPrimaryRole | INTEGER Boolean | Required | 0 | Only one primary role per contact recommended. | Preferred role label. | Filtered index. |

### ContactAddresses

Structured international address storage.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ContactId | TEXT Guid | Required | None | Must reference Contacts.Id. | Owning contact. | FK Contacts.Id, indexed. |
| AddressType | TEXT | Required | Main | Allowed: Main, Billing, Shipping, Office, Other. | Address purpose. | Indexed. |
| Country | TEXT | Optional | NULL | Max 100 chars. | Country. | Indexed. |
| StateProvince | TEXT | Optional | NULL | Max 100 chars. | State/province. | Indexed. |
| City | TEXT | Optional | NULL | Max 100 chars. | City. | Indexed. |
| Zone | TEXT | Optional | NULL | Max 100 chars. | Zone. | Indexed. |
| Area | TEXT | Optional | NULL | Max 150 chars. | Area/neighborhood. | Indexed. |
| Street | TEXT | Optional | NULL | Max 200 chars. | Street. | None. |
| BuildingNumber | TEXT | Optional | NULL | Max 50 chars. | Building number. | None. |
| FloorUnit | TEXT | Optional | NULL | Max 80 chars. | Floor/unit. | None. |
| FullAddress | TEXT | Optional | NULL | Max 1000 chars. | Complete display address. | Full-text index later. |
| AddressNotes | TEXT | Optional | NULL | Max 1000 chars. | Delivery/location notes. | None. |
| IsPrimary | INTEGER Boolean | Required | 1 | 0 or 1. | Primary address flag. | Indexed. |

### ContactLocations

GPS location data for maps and nearby contact workflows.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ContactId | TEXT Guid | Required | None | Must reference Contacts.Id. | Owning contact. | FK Contacts.Id, indexed. |
| AddressId | TEXT Guid | Optional | NULL | Must reference ContactAddresses.Id. | Related address. | FK ContactAddresses.Id. |
| Latitude | REAL | Optional | NULL | -90 to 90. | GPS latitude. | Composite index with Longitude. |
| Longitude | REAL | Optional | NULL | -180 to 180. | GPS longitude. | Composite index with Latitude. |
| AccuracyMeters | REAL | Optional | NULL | >= 0. | GPS accuracy. | None. |
| CaptureMethod | TEXT | Required | Manual | Allowed: Manual, DeviceGps, MapPin, Imported, QR, OCR. | How location was captured. | Indexed. |
| CapturedAtUtc | TEXT DateTimeOffset | Optional | NULL | Valid UTC timestamp. | Capture time. | Indexed. |
| IsPrimary | INTEGER Boolean | Required | 1 | 0 or 1. | Primary map point. | Indexed. |

### Categories

Dynamic business type lookup, such as Trader, Supermarket, Distributor.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | 2-120 chars, unique among active rows. | Business type. | Unique filtered index. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Category explanation. | None. |
| ColorHex | TEXT | Optional | NULL | Valid #RRGGBB if present. | UI color. | None. |
| SortOrder | INTEGER | Required | 0 | >= 0. | Display order. | Indexed. |
| IsSystem | INTEGER Boolean | Required | 0 | 0 or 1. | Built-in seed flag. | Indexed. |

### ContactCategories

Contact-to-category assignment.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ContactId | TEXT Guid | Required | None | Must reference Contacts.Id. | Contact. | FK Contacts.Id, composite unique with CategoryId. |
| CategoryId | TEXT Guid | Required | None | Must reference Categories.Id. | Business category. | FK Categories.Id, indexed. |
| IsPrimary | INTEGER Boolean | Required | 1 | 0 or 1. | Primary category. | Indexed. |

### Tags

Dynamic interest/product/service tag lookup.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | 2-120 chars, unique among active rows. | Interest/topic tag. | Unique filtered index. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Tag explanation. | None. |
| ColorHex | TEXT | Optional | NULL | Valid #RRGGBB if present. | UI chip color. | None. |
| SortOrder | INTEGER | Required | 0 | >= 0. | Display order. | Indexed. |

### ContactTags

Many-to-many contact tag assignment.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ContactId | TEXT Guid | Required | None | Must reference Contacts.Id. | Contact. | FK Contacts.Id, composite unique with TagId. |
| TagId | TEXT Guid | Required | None | Must reference Tags.Id. | Tag. | FK Tags.Id, indexed. |

### BusinessMatters

Dynamic business topic or interaction state, such as Inquiry, Follow-up, Hot Lead.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | 2-120 chars, unique among active rows. | Business matter name. | Unique filtered index. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Meaning/use. | None. |
| ColorHex | TEXT | Optional | NULL | Valid #RRGGBB if present. | UI chip color. | None. |
| SortOrder | INTEGER | Required | 0 | >= 0. | Display order. | Indexed. |
| IsPipelineState | INTEGER Boolean | Required | 0 | 0 or 1. | Enables pipeline-style use later. | Indexed. |

### ContactBusinessMatters

Many-to-many contact business matter assignment.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ContactId | TEXT Guid | Required | None | Must reference Contacts.Id. | Contact. | FK Contacts.Id, composite unique with BusinessMatterId. |
| BusinessMatterId | TEXT Guid | Required | None | Must reference BusinessMatters.Id. | Assigned business matter. | FK BusinessMatters.Id, indexed. |
| AssignedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Assignment time. | Indexed. |

## Products and Quotations

### ProductCategories

Dynamic product category lookup.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | 2-120 chars, unique among active rows. | Product category name. | Unique filtered index. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Category explanation. | None. |
| SortOrder | INTEGER | Required | 0 | >= 0. | Display order. | Indexed. |

### ProductTags

Dynamic product tag lookup.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | 2-120 chars, unique among active rows. | Product tag. | Unique filtered index. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Tag explanation. | None. |
| SortOrder | INTEGER | Required | 0 | >= 0. | Display order. | Indexed. |

### Products

Product catalog for future quotation line items.

Includes: StandardEntityFields, SoftDeleteFields, DraftVersionSyncFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ProductName | TEXT | Required unless IsDraft = 1 | NULL | Max 250 chars. | Product name. | Indexed. |
| ProductCode | TEXT | Optional | NULL | Max 80 chars, unique when present. | Internal product code. | Unique filtered index. |
| PartNumber | TEXT | Optional | NULL | Max 100 chars. | Manufacturer/internal part number. | Indexed. |
| SupplierPartNumber | TEXT | Optional | NULL | Max 100 chars. | Supplier part number. | Indexed. |
| Barcode | TEXT | Optional | NULL | Max 100 chars, unique when present. | Barcode. | Unique filtered index. |
| Brand | TEXT | Optional | NULL | Max 120 chars. | Product brand. | Indexed. |
| ProductCategoryId | TEXT Guid | Optional | NULL | Must reference ProductCategories.Id. | Product category. | FK ProductCategories.Id, indexed. |
| Description | TEXT | Optional | NULL | Long text. | Product description. | Full-text index later. |
| Unit | TEXT | Optional | Piece | Max 50 chars. | Unit of measure. | Indexed. |
| CostPrice | NUMERIC Decimal | Optional | NULL | >= 0. | Cost price. | None. |
| SellingPrice | NUMERIC Decimal | Optional | NULL | >= 0. | Selling price. | None. |
| StockQuantity | NUMERIC Decimal | Optional | 0 | >= 0 unless negative stock is enabled. | Current stock count. | Indexed. |
| ProductImagePath | TEXT | Optional | NULL | Relative file path. | Main product image. | None. |
| DatasheetFilePath | TEXT | Optional | NULL | Relative file path. | Product datasheet. | None. |
| Status | TEXT | Required | Active | Allowed: Active, Inactive, Discontinued, Draft. | Product status. | Indexed. |
| Notes | TEXT | Optional | NULL | Long text. | Product notes. | None. |

### ProductTagAssignments

Many-to-many product tag assignment.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ProductId | TEXT Guid | Required | None | Must reference Products.Id. | Product. | FK Products.Id, composite unique with ProductTagId. |
| ProductTagId | TEXT Guid | Required | None | Must reference ProductTags.Id. | Product tag. | FK ProductTags.Id, indexed. |

### Quotations

Quotation header linked to a unified contact.

Includes: StandardEntityFields, SoftDeleteFields, DraftVersionSyncFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| QuotationNumber | TEXT | Required | Auto-generated | Unique, max 80 chars. | Business quotation number. | Unique index. |
| ContactId | TEXT Guid | Required unless IsDraft = 1 | NULL | Must reference Contacts.Id. | Customer/supplier contact. | FK Contacts.Id, indexed. |
| ContactRoleId | TEXT Guid | Optional | NULL | Must reference ContactRoles.Id. | Role context for quotation. | FK ContactRoles.Id. |
| QuotationDateUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Quotation date. | Indexed. |
| ValidUntilUtc | TEXT DateTimeOffset | Optional | NULL | Must be >= QuotationDateUtc when present. | Validity date. | Indexed. |
| Subject | TEXT | Optional | NULL | Max 250 chars. | Quotation subject. | Indexed. |
| Reference | TEXT | Optional | NULL | Max 150 chars. | Reference number/text. | Indexed. |
| Currency | TEXT | Required | Company default | ISO currency code, max 10 chars. | Currency. | Indexed. |
| Subtotal | NUMERIC Decimal | Required | 0 | >= 0. | Sum before discount/tax. | None. |
| DiscountTotal | NUMERIC Decimal | Required | 0 | >= 0. | Total discount. | None. |
| TaxTotal | NUMERIC Decimal | Required | 0 | >= 0. | Total tax. | None. |
| GrandTotal | NUMERIC Decimal | Required | 0 | >= 0. | Final total. | Indexed. |
| Status | TEXT | Required | Draft | Allowed: Draft, Sent, Accepted, Rejected, Expired, Cancelled. | Quotation status. | Indexed. |
| Notes | TEXT | Optional | NULL | Long text. | Internal or printed notes. | None. |

### QuotationItems

Quotation line items.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| QuotationId | TEXT Guid | Required | None | Must reference Quotations.Id. | Parent quotation. | FK Quotations.Id, indexed. |
| ProductId | TEXT Guid | Optional | NULL | Must reference Products.Id. | Linked product. | FK Products.Id, indexed. |
| LineNumber | INTEGER | Required | 1 | >= 1, unique per quotation. | Display order. | Composite unique with QuotationId. |
| Description | TEXT | Required | None | Max 1000 chars. | Line item description. | None. |
| Quantity | NUMERIC Decimal | Required | 1 | > 0. | Quantity. | None. |
| Unit | TEXT | Optional | NULL | Max 50 chars. | Unit of measure. | None. |
| UnitPrice | NUMERIC Decimal | Required | 0 | >= 0. | Unit price. | None. |
| DiscountAmount | NUMERIC Decimal | Required | 0 | >= 0. | Discount amount. | None. |
| TaxRate | NUMERIC Decimal | Required | 0 | 0 to 100. | Tax percentage. | None. |
| TaxAmount | NUMERIC Decimal | Required | 0 | >= 0. | Tax amount. | None. |
| LineTotal | NUMERIC Decimal | Required | 0 | >= 0. | Final line total. | None. |
| Notes | TEXT | Optional | NULL | Max 1000 chars. | Line notes. | None. |

## WhatsApp Marketing, Consent, and Follow-Up

### MessageTemplates

Reusable WhatsApp/email/message templates.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | 2-150 chars, unique among active rows. | Template name. | Unique filtered index. |
| Channel | TEXT | Required | WhatsApp | Allowed: WhatsApp, Email, SMS, Other. | Intended channel. | Indexed. |
| BodyText | TEXT | Required | None | Non-empty. | Message body. | Full-text index later. |
| Language | TEXT | Optional | NULL | ISO language code. | Template language. | Indexed. |
| IsActive | INTEGER Boolean | Required | 1 | 0 or 1. | Template availability. | Indexed. |

### MarketingCampaigns

Campaign header for WhatsApp marketing and response tracking.

Includes: StandardEntityFields, SoftDeleteFields, DraftVersionSyncFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| CampaignName | TEXT | Required unless IsDraft = 1 | NULL | Max 200 chars. | Campaign name. | Indexed. |
| CampaignDateUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Campaign date/time. | Indexed. |
| MessageTemplateId | TEXT Guid | Optional | NULL | Must reference MessageTemplates.Id. | Template used. | FK MessageTemplates.Id. |
| MessageText | TEXT | Required unless IsDraft = 1 | NULL | Non-empty when sending. | Final campaign message. | None. |
| CategoryFilterJson | TEXT JSON | Optional | NULL | Valid JSON. | Selected category filters. | None. |
| TagFilterJson | TEXT JSON | Optional | NULL | Valid JSON. | Selected tag filters. | None. |
| BusinessMatterFilterJson | TEXT JSON | Optional | NULL | Valid JSON. | Selected matter filters. | None. |
| RoleFilterJson | TEXT JSON | Optional | NULL | Valid JSON. | Customer/supplier/lead filters. | None. |
| MarketingSource | TEXT | Optional | NULL | Max 150 chars. | Source/channel description. | Indexed. |
| Status | TEXT | Required | Draft | Allowed: Draft, Ready, Sent, Completed, Cancelled. | Campaign status. | Indexed. |
| Notes | TEXT | Optional | NULL | Long text. | Campaign notes. | None. |

### MarketingCampaignRecipients

Per-contact campaign delivery, response, and follow-up.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| CampaignId | TEXT Guid | Required | None | Must reference MarketingCampaigns.Id. | Campaign. | FK MarketingCampaigns.Id, composite unique with ContactId. |
| ContactId | TEXT Guid | Required | None | Must reference Contacts.Id. | Recipient contact. | FK Contacts.Id, indexed. |
| DeliveryStatus | TEXT | Required | Pending | Allowed: Pending, Sent, Failed, Delivered, Unknown. | Delivery state. | Indexed. |
| ReadStatus | TEXT | Required | Unknown | Allowed: Unknown, Read, Unread. | Read state when known. | Indexed. |
| ReplyStatus | TEXT | Required | NoReply | Allowed: NoReply, Replied, Interested, NotInterested. | Reply state. | Indexed. |
| ReplyText | TEXT | Optional | NULL | Long text. | Captured reply. | Full-text index later. |
| IsInterested | INTEGER Boolean | Optional | NULL | 0, 1, or NULL unknown. | Interest result. | Indexed. |
| FollowUpNeeded | INTEGER Boolean | Required | 0 | 0 or 1. | Follow-up flag. | Indexed. |
| FollowUpDateUtc | TEXT DateTimeOffset | Optional | NULL | Required when follow-up needed and scheduled. | Follow-up date. | Indexed. |
| SentAtUtc | TEXT DateTimeOffset | Optional | NULL | Valid UTC timestamp. | Sent time. | Indexed. |
| ReadAtUtc | TEXT DateTimeOffset | Optional | NULL | Valid UTC timestamp. | Read time. | None. |
| RepliedAtUtc | TEXT DateTimeOffset | Optional | NULL | Valid UTC timestamp. | Reply time. | Indexed. |
| Notes | TEXT | Optional | NULL | Long text. | Recipient notes. | None. |

### ContactConsentEvents

Consent and opt-out history.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ContactId | TEXT Guid | Required | None | Must reference Contacts.Id. | Contact. | FK Contacts.Id, indexed. |
| ConsentType | TEXT | Required | WhatsAppMarketing | Allowed: WhatsAppMarketing, EmailMarketing, PhoneContact, Other. | Consent category. | Indexed. |
| ConsentStatus | TEXT | Required | Unknown | Allowed: Granted, Denied, OptedOut, Unknown. | Consent result. | Indexed. |
| ConsentSource | TEXT | Optional | NULL | Max 150 chars. | How consent was obtained. | Indexed. |
| ConsentDateUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Consent/opt-out date. | Indexed. |
| Notes | TEXT | Optional | NULL | Long text. | Consent notes. | None. |

## QR, OCR, Attachments, and Documents

### Scans

QR and OCR scan history.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ScanType | TEXT | Required | QR | Allowed: QR, OCR, QRAndOCR. | Scan mode. | Indexed. |
| ScanSource | TEXT | Required | FileUpload | Allowed: Camera, ImageUpload, FileUpload, Imported. | Source of scan. | Indexed. |
| OriginalFilePath | TEXT | Optional | NULL | Relative file path. | Saved image/file path. | None. |
| ExtractedText | TEXT | Optional | NULL | Long text. | OCR or QR extracted text. | Full-text index later. |
| ParsedDataJson | TEXT JSON | Optional | NULL | Valid JSON. | Parsed contact fields. | None. |
| ConfidenceScore | REAL | Optional | NULL | 0 to 1. | OCR/parser confidence. | Indexed. |
| ResultStatus | TEXT | Required | PendingReview | Allowed: PendingReview, Saved, DuplicateFound, Failed, Ignored. | Scan workflow status. | Indexed. |
| FailureReason | TEXT | Optional | NULL | Max 1000 chars. | Failure explanation. | None. |
| LinkedContactId | TEXT Guid | Optional | NULL | Must reference Contacts.Id. | Contact created/linked from scan. | FK Contacts.Id, indexed. |
| ScannedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Scan time. | Indexed. |

### Attachments

Generic file attachment table.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| OwnerEntityType | TEXT | Required | None | Allowed planned values: Contact, Product, Quotation, Scan, CompanyProfile, Campaign, Other. | Attached entity type. | Composite index with OwnerEntityId. |
| OwnerEntityId | TEXT Guid | Required | None | Must refer to owner table by type. | Attached entity ID. | Composite index with OwnerEntityType. |
| FileName | TEXT | Required | None | Max 255 chars. | Original/display file name. | Indexed. |
| StoredFilePath | TEXT | Required | None | Relative managed file path. | Stored file location. | Unique index recommended. |
| ContentType | TEXT | Optional | NULL | MIME type, max 150 chars. | File content type. | Indexed. |
| FileSizeBytes | INTEGER | Required | 0 | >= 0. | File size. | Indexed. |
| Sha256Hash | TEXT | Optional | NULL | 64 hex chars when present. | Integrity/duplicate file check. | Indexed. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Attachment description. | None. |

## Activity, Reminders, Notifications, and Search

### Activities

Unified contact and system activity timeline.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ContactId | TEXT Guid | Optional | NULL | Must reference Contacts.Id when present. | Related contact. | FK Contacts.Id, indexed. |
| RelatedEntityType | TEXT | Optional | NULL | Max 80 chars. | Related module/entity type. | Composite index with RelatedEntityId. |
| RelatedEntityId | TEXT Guid | Optional | NULL | Valid GUID. | Related entity ID. | Composite index with RelatedEntityType. |
| ActivityType | TEXT | Required | Note | Allowed: Call, Note, WhatsAppReply, Scan, Edit, StatusChange, Reminder, Quotation, Campaign, System. | Timeline event type. | Indexed. |
| Title | TEXT | Required | None | Max 200 chars. | Activity title. | Indexed. |
| Body | TEXT | Optional | NULL | Long text. | Activity details. | Full-text index later. |
| ActivityAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Event time. | Indexed. |
| IsImportant | INTEGER Boolean | Required | 0 | 0 or 1. | Important marker. | Indexed. |

### Reminders

Follow-up and task reminders.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ContactId | TEXT Guid | Optional | NULL | Must reference Contacts.Id when present. | Related contact. | FK Contacts.Id, indexed. |
| AssignedToUserId | TEXT Guid | Optional | NULL | Must reference Users.Id. | Responsible user. | FK Users.Id, indexed. |
| Title | TEXT | Required | None | Max 200 chars. | Reminder title. | Indexed. |
| Description | TEXT | Optional | NULL | Long text. | Reminder details. | None. |
| DueAtUtc | TEXT DateTimeOffset | Required | None | Valid UTC timestamp. | Due date/time. | Indexed. |
| CompletedAtUtc | TEXT DateTimeOffset | Optional | NULL | Valid UTC timestamp. | Completion date/time. | Indexed. |
| Status | TEXT | Required | Open | Allowed: Open, Completed, Snoozed, Cancelled, Overdue. | Reminder state. | Indexed. |
| ReminderType | TEXT | Required | FollowUp | Allowed: FollowUp, Backup, Scan, CampaignReply, Task, Other. | Reminder category. | Indexed. |

### Notifications

Notification center records.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| UserId | TEXT Guid | Optional | NULL | Must reference Users.Id when user-specific. | Target user. | FK Users.Id, indexed. |
| NotificationType | TEXT | Required | Info | Allowed: Info, Warning, Error, FollowUp, Backup, Scan, MarketingReply. | Notification category. | Indexed. |
| Title | TEXT | Required | None | Max 200 chars. | Notification title. | Indexed. |
| Message | TEXT | Optional | NULL | Long text. | Notification body. | None. |
| RelatedEntityType | TEXT | Optional | NULL | Max 80 chars. | Linked entity type. | Composite index with RelatedEntityId. |
| RelatedEntityId | TEXT Guid | Optional | NULL | Valid GUID. | Linked entity ID. | Composite index with RelatedEntityType. |
| IsRead | INTEGER Boolean | Required | 0 | 0 or 1. | Read/unread state. | Indexed. |
| ReadAtUtc | TEXT DateTimeOffset | Optional | NULL | Required when IsRead = 1. | Read timestamp. | None. |

### SearchHistory

Recent user search history.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| UserId | TEXT Guid | Optional | NULL | Must reference Users.Id when present. | Search owner. | FK Users.Id, indexed. |
| SearchText | TEXT | Required | None | Max 500 chars. | Search query. | Indexed. |
| ModuleKey | TEXT | Optional | NULL | Max 80 chars. | Module searched. | Indexed. |
| FilterJson | TEXT JSON | Optional | NULL | Valid JSON. | Applied filters. | None. |
| SearchedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Search time. | Indexed. |

### SavedFilters

Reusable filters and segments.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| UserId | TEXT Guid | Optional | NULL | Must reference Users.Id when user-specific. | Filter owner. | FK Users.Id, indexed. |
| Name | TEXT | Required | None | Max 150 chars. | Filter/segment name. | Indexed. |
| ModuleKey | TEXT | Required | Contacts | Max 80 chars. | Module for filter. | Indexed. |
| FilterJson | TEXT JSON | Required | None | Valid JSON. | Filter definition. | None. |
| IsShared | INTEGER Boolean | Required | 0 | 0 or 1. | Shared with other users. | Indexed. |

## Audit, Recycle Bin, Duplicates, and Merge

### AuditLogs

Security, governance, and data-change audit log.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Id | TEXT Guid | Required | New GUID | Unique. | Audit log ID. | PK. |
| UserId | TEXT Guid | Optional | NULL | Must reference Users.Id when available. | Acting user. | FK Users.Id, indexed. |
| Action | TEXT | Required | None | Max 100 chars. | Action name. | Indexed. |
| EntityType | TEXT | Optional | NULL | Max 100 chars. | Entity type changed. | Composite index with EntityId. |
| EntityId | TEXT Guid | Optional | NULL | Valid GUID. | Entity ID changed. | Composite index with EntityType. |
| OldValuesJson | TEXT JSON | Optional | NULL | Valid JSON. | Previous values. | None. |
| NewValuesJson | TEXT JSON | Optional | NULL | Valid JSON. | New values. | None. |
| IpOrDeviceInfo | TEXT | Optional | NULL | Max 300 chars. | Device/IP metadata. | None. |
| CreatedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Audit timestamp. | Indexed. |

### DeletedRecords

Recycle bin index for soft-deleted records.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Id | TEXT Guid | Required | New GUID | Unique. | Recycle bin record ID. | PK. |
| EntityType | TEXT | Required | None | Max 100 chars. | Deleted entity type. | Composite index with EntityId. |
| EntityId | TEXT Guid | Required | None | Valid GUID. | Deleted entity ID. | Composite unique with EntityType. |
| DisplayName | TEXT | Required | None | Max 250 chars. | User-friendly deleted record name. | Indexed. |
| DeletedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Deletion time. | Indexed. |
| DeletedByUserId | TEXT Guid | Optional | NULL | Must reference Users.Id. | User who deleted. | FK Users.Id. |
| RestoreStatus | TEXT | Required | Restorable | Allowed: Restorable, Restored, Blocked, Purged. | Restore state. | Indexed. |

### DuplicateCandidates

Potential duplicate contacts or records.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| EntityType | TEXT | Required | Contact | Max 100 chars. | Duplicate entity type. | Indexed. |
| SourceEntityId | TEXT Guid | Required | None | Valid GUID. | First record. | Composite unique with MatchEntityId. |
| MatchEntityId | TEXT Guid | Required | None | Valid GUID. | Possible duplicate record. | Composite unique with SourceEntityId. |
| MatchScore | REAL | Required | 0 | 0 to 1. | Duplicate confidence score. | Indexed. |
| MatchReasonsJson | TEXT JSON | Optional | NULL | Valid JSON. | Why it matched. | None. |
| ReviewStatus | TEXT | Required | Pending | Allowed: Pending, ConfirmedDuplicate, NotDuplicate, Merged, Ignored. | Review state. | Indexed. |
| ReviewedByUserId | TEXT Guid | Optional | NULL | Must reference Users.Id. | Reviewer. | FK Users.Id. |
| ReviewedAtUtc | TEXT DateTimeOffset | Optional | NULL | Valid UTC timestamp. | Review time. | Indexed. |

### MergeHistory

Records merge decisions and preserves traceability.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| EntityType | TEXT | Required | Contact | Max 100 chars. | Merged entity type. | Indexed. |
| PrimaryEntityId | TEXT Guid | Required | None | Valid GUID. | Record kept after merge. | Indexed. |
| MergedEntityId | TEXT Guid | Required | None | Valid GUID. | Record merged into primary. | Indexed. |
| MergeSummaryJson | TEXT JSON | Required | None | Valid JSON. | Field-level merge decisions. | None. |
| MergedByUserId | TEXT Guid | Optional | NULL | Must reference Users.Id. | User who merged. | FK Users.Id. |
| MergedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Merge time. | Indexed. |

## Custom Fields and Settings

### CustomFieldDefinitions

Admin-defined dynamic fields.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| EntityType | TEXT | Required | Contact | Allowed planned values: Contact, Product, Quotation, Campaign, Other. | Entity/module field applies to. | Composite unique with FieldKey. |
| FieldKey | TEXT | Required | None | 2-80 chars, alphanumeric/underscore, unique per entity type. | Stable field key. | Composite unique with EntityType. |
| DisplayName | TEXT | Required | None | 2-150 chars. | UI label. | Indexed. |
| FieldType | TEXT | Required | Text | Allowed: Text, Number, Date, Boolean, Choice, MultiChoice, Url, Email, Phone. | Value type. | Indexed. |
| IsRequired | INTEGER Boolean | Required | 0 | 0 or 1. | Required flag. | Indexed. |
| DefaultValue | TEXT | Optional | NULL | Must match FieldType if present. | Default value. | None. |
| OptionsJson | TEXT JSON | Optional | NULL | Valid JSON for choice fields. | Choices/options. | None. |
| ValidationJson | TEXT JSON | Optional | NULL | Valid JSON. | Custom validation rules. | None. |
| SortOrder | INTEGER | Required | 0 | >= 0. | Display order. | Indexed. |
| IsActive | INTEGER Boolean | Required | 1 | 0 or 1. | Active flag. | Indexed. |

### CustomFieldValues

Stores values for custom fields.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| FieldDefinitionId | TEXT Guid | Required | None | Must reference CustomFieldDefinitions.Id. | Custom field definition. | FK CustomFieldDefinitions.Id, composite unique with EntityId. |
| EntityType | TEXT | Required | None | Must match definition entity type. | Entity type. | Composite index with EntityId. |
| EntityId | TEXT Guid | Required | None | Valid GUID. | Entity ID. | Composite index with EntityType. |
| ValueText | TEXT | Optional | NULL | Must match field type validation. | Stored value as text/JSON. | Indexed for short searchable values. |

### CompanyProfile

Single source of truth for Smarto Life company information.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| CompanyName | TEXT | Required | Smarto Life | Max 200 chars. | Company name. | Unique single-row constraint. |
| OwnerFounder | TEXT | Optional | Mohammed Rafi | Max 200 chars. | Owner/founder. | None. |
| LogoPath | TEXT | Optional | Assets/Logo/SmartoLife_Logo.png | Relative path. | Official logo file. | None. |
| Slogan | TEXT | Optional | Smart Solutions for Tomorrow | Max 250 chars. | Company slogan. | None. |
| Description | TEXT | Optional | NULL | Long text. | Company description. | None. |
| Email | TEXT | Optional | Rafi@smartolife.com | Valid email. | Company email. | Indexed. |
| Phone | TEXT | Optional | NULL | Max 30 chars. | Phone. | None. |
| WhatsApp | TEXT | Optional | NULL | Max 30 chars. | WhatsApp. | None. |
| Website | TEXT | Optional | NULL | Valid URL/domain. | Website. | None. |
| Address | TEXT | Optional | NULL | Long text. | Company address. | None. |
| Latitude | REAL | Optional | NULL | -90 to 90. | Company GPS latitude. | None. |
| Longitude | REAL | Optional | NULL | -180 to 180. | Company GPS longitude. | None. |
| BusinessRegistration | TEXT | Optional | NULL | Max 150 chars. | Registration number/details. | None. |
| TaxNumber | TEXT | Optional | NULL | Max 150 chars. | Tax number. | None. |
| Currency | TEXT | Required | USD | ISO currency code. | Default currency. | Indexed. |
| Language | TEXT | Required | en | ISO language code. | Default language. | Indexed. |
| TimeZoneId | TEXT | Required | Local system time zone | Valid time zone ID. | Default timezone. | None. |
| Domain | TEXT | Optional | NULL | Valid domain. | Future domain. | None. |
| VpsInformationJson | TEXT JSON | Optional | NULL | Valid JSON. | Future VPS info. | None. |
| SmtpSettingsJson | TEXT JSON | Optional | NULL | Valid JSON, secrets encrypted later. | Future SMTP settings. | None. |
| BackupSettingsJson | TEXT JSON | Optional | NULL | Valid JSON. | Backup configuration. | None. |
| FutureApiSettingsJson | TEXT JSON | Optional | NULL | Valid JSON, secrets encrypted later. | Future API settings. | None. |

### Settings

Application settings and defaults.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| SettingKey | TEXT | Required | None | 2-150 chars, unique. | Stable setting key. | Unique index. |
| SettingValue | TEXT | Optional | NULL | Must match ValueType. | Stored value. | None. |
| ValueType | TEXT | Required | Text | Allowed: Text, Number, Boolean, Json, Date. | Setting type. | Indexed. |
| Category | TEXT | Required | General | Max 80 chars. | Settings group. | Indexed. |
| IsUserEditable | INTEGER Boolean | Required | 1 | 0 or 1. | Editable in UI. | Indexed. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Setting explanation. | None. |

## Import, Export, Backup, Logs, Templates, and Retention

### ImportJobs

Excel/CSV import wizard history.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| FileName | TEXT | Required | None | Max 255 chars. | Source file name. | Indexed. |
| FilePath | TEXT | Required | None | Relative path. | Stored source file path. | None. |
| ModuleKey | TEXT | Required | Contacts | Max 80 chars. | Target module. | Indexed. |
| MappingJson | TEXT JSON | Optional | NULL | Valid JSON. | Column mapping. | None. |
| Status | TEXT | Required | Pending | Allowed: Pending, Validating, Completed, Failed, Cancelled. | Import status. | Indexed. |
| TotalRows | INTEGER | Required | 0 | >= 0. | Total rows. | None. |
| SuccessfulRows | INTEGER | Required | 0 | >= 0 and <= TotalRows. | Imported rows. | None. |
| FailedRows | INTEGER | Required | 0 | >= 0 and <= TotalRows. | Failed rows. | None. |
| ErrorSummary | TEXT | Optional | NULL | Long text. | Error summary. | None. |

### ImportJobRows

Row-level import validation and error tracking.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ImportJobId | TEXT Guid | Required | None | Must reference ImportJobs.Id. | Parent import job. | FK ImportJobs.Id, indexed. |
| RowNumber | INTEGER | Required | 1 | >= 1. | Source row number. | Composite unique with ImportJobId. |
| RawDataJson | TEXT JSON | Required | None | Valid JSON. | Original row data. | None. |
| ParsedDataJson | TEXT JSON | Optional | NULL | Valid JSON. | Parsed mapped values. | None. |
| Status | TEXT | Required | Pending | Allowed: Pending, Valid, Imported, Failed, Skipped. | Row status. | Indexed. |
| ErrorText | TEXT | Optional | NULL | Long text. | Row error message. | None. |
| CreatedEntityType | TEXT | Optional | NULL | Max 80 chars. | Created entity type. | Composite index with CreatedEntityId. |
| CreatedEntityId | TEXT Guid | Optional | NULL | Valid GUID. | Created entity ID. | Composite index with CreatedEntityType. |

### ExportJobs

Export history.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| ModuleKey | TEXT | Required | Contacts | Max 80 chars. | Exported module. | Indexed. |
| ExportFormat | TEXT | Required | CSV | Allowed: CSV, Excel, PDF, JSON. | Export format. | Indexed. |
| FileName | TEXT | Required | None | Max 255 chars. | Export file name. | Indexed. |
| FilePath | TEXT | Required | None | Relative path. | Export file path. | None. |
| FilterJson | TEXT JSON | Optional | NULL | Valid JSON. | Export filters. | None. |
| RowCount | INTEGER | Required | 0 | >= 0. | Exported rows. | None. |
| Status | TEXT | Required | Completed | Allowed: Pending, Completed, Failed, Cancelled. | Export status. | Indexed. |
| ErrorText | TEXT | Optional | NULL | Long text. | Export error. | None. |

### BackupJobs

Backup and restore history.

Includes: StandardEntityFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| JobType | TEXT | Required | Backup | Allowed: Backup, Restore. | Backup or restore job. | Indexed. |
| BackupScope | TEXT | Required | Full | Allowed: Full, Selective. | Backup scope. | Indexed. |
| FileName | TEXT | Required | None | Max 255 chars. | Backup file name. | Indexed. |
| FilePath | TEXT | Required | None | Relative or configured path. | Backup file path. | None. |
| FileSizeBytes | INTEGER | Optional | NULL | >= 0. | Backup file size. | Indexed. |
| IsEncrypted | INTEGER Boolean | Required | 1 | 0 or 1. | Encryption flag. | Indexed. |
| EncryptionMethod | TEXT | Optional | NULL | Max 100 chars. | Encryption method metadata. | None. |
| Status | TEXT | Required | Pending | Allowed: Pending, Running, Completed, Failed, Cancelled. | Job status. | Indexed. |
| StartedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Start time. | Indexed. |
| CompletedAtUtc | TEXT DateTimeOffset | Optional | NULL | >= StartedAtUtc. | Completion time. | Indexed. |
| ErrorText | TEXT | Optional | NULL | Long text. | Error details. | None. |
| RestoreSourceBackupJobId | TEXT Guid | Optional | NULL | Must reference BackupJobs.Id for restore. | Source backup for restore. | FK BackupJobs.Id. |

### SystemLogs

Technical logs for troubleshooting.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Id | TEXT Guid | Required | New GUID | Unique. | Log ID. | PK. |
| Level | TEXT | Required | Info | Allowed: Trace, Debug, Info, Warning, Error, Critical. | Log level. | Indexed. |
| Category | TEXT | Required | General | Max 100 chars. | Source category. | Indexed. |
| Message | TEXT | Required | None | Non-empty. | Log message. | Full-text index later. |
| ExceptionText | TEXT | Optional | NULL | Long text. | Exception details. | None. |
| RelatedEntityType | TEXT | Optional | NULL | Max 80 chars. | Related entity type. | Composite index with RelatedEntityId. |
| RelatedEntityId | TEXT Guid | Optional | NULL | Valid GUID. | Related entity ID. | Composite index with RelatedEntityType. |
| CreatedAtUtc | TEXT DateTimeOffset | Required | Current UTC time | Valid UTC timestamp. | Log timestamp. | Indexed. |

### PrintTemplates

Templates for quotations, contact cards, labels, reports, and PDFs.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | Max 150 chars, unique per TemplateType. | Template name. | Composite unique with TemplateType. |
| TemplateType | TEXT | Required | Quotation | Allowed: Quotation, ContactCard, Label, Report, BackupReport, Other. | Template purpose. | Indexed. |
| Content | TEXT | Required | None | Non-empty template markup/text. | Template body. | None. |
| Language | TEXT | Optional | NULL | ISO language code. | Template language. | Indexed. |
| IsDefault | INTEGER Boolean | Required | 0 | Only one default per TemplateType recommended. | Default template flag. | Filtered index. |
| IsActive | INTEGER Boolean | Required | 1 | 0 or 1. | Active flag. | Indexed. |

### RetentionPolicies

Data retention and archiving rules.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | Max 150 chars, unique. | Policy name. | Unique filtered index. |
| EntityType | TEXT | Required | None | Max 100 chars. | Target entity type. | Indexed. |
| RetentionDays | INTEGER | Optional | NULL | > 0 when present. | Days to retain active data. | None. |
| ArchiveAfterDays | INTEGER | Optional | NULL | > 0 when present. | Days before archive. | None. |
| PurgeAfterDays | INTEGER | Optional | NULL | > 0 and greater than archive/retention days when present. | Days before purge if allowed. | None. |
| IsEnabled | INTEGER Boolean | Required | 0 | 0 or 1. | Policy enabled flag. | Indexed. |
| RulesJson | TEXT JSON | Optional | NULL | Valid JSON. | Additional conditions. | None. |

## Map and Future Territory Support

### MapAreas

Saved business map areas for filtering, territory analysis, and future route planning.

Includes: StandardEntityFields, SoftDeleteFields.

| Field Name | Data Type | Required | Default Value | Validation Rules | Description | Relationships / Indexes |
|---|---:|---|---|---|---|---|
| Name | TEXT | Required | None | Max 150 chars. | Area/territory name. | Indexed. |
| Description | TEXT | Optional | NULL | Max 500 chars. | Area description. | None. |
| ShapeType | TEXT | Required | Radius | Allowed: Radius, Polygon, Rectangle. | Area shape type. | Indexed. |
| CenterLatitude | REAL | Optional | NULL | -90 to 90. | Center latitude for radius. | Composite index with CenterLongitude. |
| CenterLongitude | REAL | Optional | NULL | -180 to 180. | Center longitude for radius. | Composite index with CenterLatitude. |
| RadiusMeters | REAL | Optional | NULL | > 0 for Radius shape. | Radius distance. | None. |
| GeometryJson | TEXT JSON | Optional | NULL | Valid JSON for polygon/rectangle. | Shape geometry. | None. |
| OwnerUserId | TEXT Guid | Optional | NULL | Must reference Users.Id. | Area owner. | FK Users.Id, indexed. |

## Initial Index and Constraint Strategy

- Unique indexes: user name, role name, permission module/action, contact role key, active lookup names, quotation number, product code, barcode, setting key.
- Search indexes: contact name, company name, mobile, WhatsApp, email, category/tag/matter joins, product name/code/brand, quotation number/status/date, campaign status/date.
- Map indexes: latitude/longitude pairs and map area center coordinates.
- Recycle/audit indexes: entity type + entity ID, deleted date, audit created date.
- Duplicate detection indexes: normalized contact name, normalized company, normalized email, normalized website domain, mobile, WhatsApp.
- Many-to-many join constraints: composite unique indexes prevent duplicate role/category/tag/matter assignments.

## Notes for Phase 1 Implementation

- Seed Smarto Life company profile from the Master Specification.
- Seed default roles, permissions, contact roles, starter categories, starter tags, and starter business matters.
- Keep the schema migration-ready and avoid hard-coded company data outside `CompanyProfile`.
- Use soft delete for business records by default.
- Use audit logging for create, update, delete, restore, export, backup, login, and permission changes.
- Keep QR, OCR, WhatsApp, sync, VPS, and PDF features represented in the schema even if their full workflows are built in later phases.

