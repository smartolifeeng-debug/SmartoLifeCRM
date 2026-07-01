# Smarto Life CRM Database Schema Blueprint

## Database Direction

Use SQLite locally with Entity Framework Core migrations. Store business data in normalized relational tables, use join tables for many-to-many relationships, and keep all records audit-friendly, timestamped, versioned, soft-delete capable, and future sync-ready.

## Common Record Columns

Most business tables should include:

- `Id`
- `PublicId` or formatted business number where needed
- `CreatedAtUtc`
- `ModifiedAtUtc`
- `CreatedByUserId`
- `ModifiedByUserId`
- `DeletedAtUtc`
- `DeletedByUserId`
- `IsDeleted`
- `IsDraft`
- `RecordVersion`
- `SyncState`
- `LastSyncedAtUtc`

## Core Tables

### Contacts

Unified master table for customers, suppliers, leads, and other contacts.

Important fields:

- Contact name
- Company name
- Contact person
- Job title
- Status
- Mobile
- WhatsApp
- Email
- Website
- Additional phone
- Notes
- Owner user
- Draft status
- Soft delete status
- Record version

### ContactRoles

Lookup table for roles:

- Customer
- Supplier
- Lead
- Other

### ContactRoleAssignments

Join table allowing one contact to have multiple roles at the same time.

### ContactAddresses

Stores structured and international-ready address data:

- Country
- State/province
- City
- Zone
- Area
- Street
- Building number
- Floor/unit
- Full address
- Address notes

### ContactLocations

Stores GPS data:

- Latitude
- Longitude
- Accuracy
- Capture method
- Captured time

### Categories

Dynamic business type lookup.

### ContactCategories

Join table if multiple categories are later allowed. If Phase 1 starts with one category per contact, keep the model expandable.

### Tags

Dynamic product, service, and interest topics.

### ContactTags

Many-to-many relationship between contacts and tags.

### BusinessMatters

Dynamic business topic or interaction type lookup.

### ContactBusinessMatters

Many-to-many relationship between contacts and business matters.

### Products

Product catalog with code, part number, barcode, brand, category, tags, prices, stock, status, images, datasheets, notes, and attachments.

### ProductCategories

Dynamic product category lookup.

### ProductTags

Dynamic product tag lookup.

### ProductTagAssignments

Many-to-many relationship between products and product tags.

### Quotations

Quotation header linked to a contact.

### QuotationItems

Line items linked to quotations and optionally linked to products.

### MarketingCampaigns

WhatsApp campaign header, template, message, segment filters, source, notes, and campaign status.

### MarketingCampaignRecipients

Per-contact delivery/read/reply/interested/follow-up tracking.

### Scans

QR and OCR scan history with source, time, result status, confidence, extracted text, original file path, and linked contact if saved.

### Attachments

Generic attachment table supporting contacts, products, quotations, scans, company profile, and future modules.

### Activities

Timeline table for calls, notes, WhatsApp replies, scans, edits, status changes, reminders, and important events.

### Reminders

Follow-ups, overdue tasks, campaign response alerts, backup alerts, and scan alerts.

### Notifications

Notification center records.

### AuditLogs

Entity change logs, user action logs, security events, restore events, export events, and important governance records.

### DeletedRecords

Recycle bin index for restore workflows.

### DuplicateCandidates

Stores detected potential duplicates for review and merge.

### MergeHistory

Records merge decisions without losing history.

### CustomFieldDefinitions

Admin-defined fields by entity/module.

### CustomFieldValues

Stores custom values linked to entity records.

### Users

Login accounts.

### Roles

Admin, staff, and future roles.

### Permissions

View/edit/delete/restore/export and module-specific permission definitions.

### UserRoles

User-role join table.

### RolePermissions

Role-permission join table.

### CompanyProfile

Single source of truth for Smarto Life company details, branding, currency, language, timezone, domain, VPS, SMTP, backup, and future API settings.

### Settings

Application defaults and behavior settings.

### ImportJobs

Import wizard history, source file, mapping, validation status, row counts, and errors.

### ExportJobs

Export history.

### BackupJobs

Backup and restore history, file metadata, status, encryption metadata, and restore events.

### SystemLogs

System errors, scan failures, backup issues, sync problems, and technical troubleshooting events.

### PrintTemplates

Quotation, contact card, label, report, and future PDF template definitions.

### RetentionPolicies

Data retention, archival, and safe cleanup rules.

## Duplicate Detection Strategy

Start with practical matching:

- Mobile number
- WhatsApp number
- Email
- Company name normalized
- Contact name normalized
- Website/domain

Later improve with weighted scoring and merge review.

## Future Sync Readiness

Each syncable table should be designed for:

- Stable IDs
- Modified timestamps
- Record versions
- Soft deletes
- Conflict detection
- Last writer metadata
- Device/source tracking

