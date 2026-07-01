# Phase 2 Plan

## Phase 2 Scope

Phase 2 adds the business operations modules defined in the Master Specification:

- Product module
- Quotation module
- GPS foundation
- Map view foundation
- Quotation-ready product line workflows

Phase 2 must not include:

- WhatsApp Marketing
- QR scanning
- OCR scanning
- Backup implementation
- Reports/PDF generation beyond quotation data readiness
- Import/export
- Phase 3 or later functionality

## Milestone Workflow

Each milestone must be implemented, built, manually validated, and committed before continuing.

The application must remain usable after every milestone.

## Milestone 2.1: Products and Quotations Foundation

Goal:

- Add database/domain foundation for products, product categories, product tags, quotations, and quotation items.
- Add dashboard/navigation visibility for Products and Quotations.
- Preserve the existing Phase 1 shell and unified contact architecture.

Included:

- `ProductCategories`
- `ProductTags`
- `Products`
- `ProductTagAssignments`
- `Quotations`
- `QuotationItems`
- EF Core relationships and indexes
- Starter product categories and tags
- Dashboard cards for Products and Quotations
- Navigation entries enabled for Products and Quotations shell status

Not included:

- Printable quotations
- PDF templates
- Product image upload workflow
- Datasheet upload workflow
- Full quotation editor
- Tax/discount automation beyond stored fields

Stop point:

- Stop after build/startup validation and milestone commit.

## Milestone 2.2: GPS Foundation

Goal:

- Add structured contact address and GPS location foundation from the Data Dictionary.

Included:

- `ContactAddresses`
- `ContactLocations`
- EF Core relationships and indexes
- Contact location shell display readiness

Not included:

- Live GPS device capture
- Route planning
- Territory analysis

Stop point:

- Stop after build/startup validation and milestone commit.

## Milestone 2.3: Map View Shell

Goal:

- Add touchscreen-friendly map module shell and map area data foundation.

Included:

- `MapAreas`
- Navigation entry for Map enabled
- Map placeholder panel with filter/search readiness

Not included:

- Online map provider integration
- Route planning
- Advanced geospatial calculations

Stop point:

- Stop after build/startup validation and milestone commit.

## Milestone 2.4: Phase 2 Polish and Validation

Goal:

- Review Phase 2 module relationships, UI consistency, and database readiness.

Included:

- Final Phase 2 documentation update
- Manual validation checklist
- Compile/startup verification
- Approval gate before Phase 3

