# Smarto Life CRM Phase 0 Blueprint

## Locked Product Direction

Smarto Life CRM is a local-first, offline-first Windows desktop business CRM for Smarto Life. It must be stable, fast, premium, touchscreen-friendly, simple for non-technical users, international-ready, and prepared for future cloud sync, VPS hosting, custom domain, SSL, remote backup, and web/mobile expansion.

The system is not a basic CRM. It is a full business operations platform covering unified contacts, customers, suppliers, products, quotations, WhatsApp marketing, GPS/map workflows, QR and OCR business card scanning, backup/restore, auditability, permissions, import/export, reminders, notifications, consent, duplicate cleanup, templates, logs, onboarding, and deployment readiness.

## Technology Direction

- Platform: Windows desktop first.
- Language: C#.
- Runtime: .NET.
- Desktop UI: modern .NET desktop architecture selected for long-term maintainability and touchscreen-friendly layouts.
- Data access: Entity Framework Core.
- Local database: SQLite.
- Architecture: local-first modular monolith with clear domain boundaries and future sync-ready data design.
- File storage: local structured storage for attachments, scans, imports, exports, reports, backups, and logs.
- Future expansion: sync service, hosted API, VPS deployment, domain, SSL, health checks, remote backup, and web access can be added without replacing the local data model.

## Core Architecture Principles

- Use one unified contact model for customers, suppliers, leads, and other business relationships.
- Represent roles through relational role assignments, not duplicated customer/supplier tables.
- Keep categories, tags, and business matters dynamic and admin-managed.
- Prefer normalized relational tables with join tables for many-to-many relationships.
- Use soft delete and recycle bin behavior instead of hard deletion by default.
- Track record versions, timestamps, ownership, audit logs, activity timeline entries, and important system events.
- Design for draft-safe saves, duplicate prevention, merge workflows, and future conflict handling.
- Keep company profile information centralized and reusable for documents, quotations, reports, exports, and future email/PDF templates.

## Unified Contact Model

Contacts represent companies, people, leads, customers, suppliers, and other business records in one shared structure.

Supported roles:

- Customer
- Supplier
- Customer & Supplier
- Lead
- Other

Business meaning:

- Customer: Smarto Life sends material or services to them.
- Supplier: they supply material or services to Smarto Life.
- One company can hold multiple roles at the same time.

The UI should reuse the same page, same form, same detail screen, same search, same map workflow, and same database model. Only labels, filters, and titles should change based on selected role.

## Module Map

- Dashboard: first visual experience, KPIs, quick actions, backup status, recent activity, map summary, recent records.
- Unified Contacts: list, grid, detail, add/edit, quick add, role filtering, map-linked view, duplicate detection, drafts, merge, recycle restore.
- Categories: dynamic business type system.
- Tags: dynamic product/service/business interest system.
- Business Matters: dynamic interaction/topic/pipeline organization.
- Products: catalog, stock fields, pricing, images, datasheets, attachments, quotation readiness.
- Quotations: linked contacts, line items, draft/status/history, future print/PDF templates.
- WhatsApp Marketing: segmentation, campaign history, consent awareness, replies, follow-ups.
- GPS and Map: captured coordinates, accuracy, source, nearby contacts, territory readiness.
- QR Scanning: camera/image/file scan, field extraction, review before save, scan history.
- OCR Scanning: business card OCR, extracted text, confidence, QR detection if visible, duplicate review.
- Backup and Restore: encrypted backups, password-protected restore, manual backup, history, restore wizard, future auto backup.
- Search and Filters: global search, quick filters, advanced filters, search history, GPS area search.
- Governance: audit log, soft delete, recycle bin, version history, validation, data integrity.
- Dynamic Fields: admin-managed custom fields without schema redesign.
- Users and Permissions: login, admin/staff roles, permissions for view/edit/delete/restore/export.
- Activity Timeline: calls, notes, replies, scans, edits, status changes, important events.
- Attachments: contact, product, quotation, marketing, scan, and company files.
- Import/Export: Excel/CSV import, field mapping wizard, validation, preview, history, Excel/CSV export.
- Reminders and Notifications: follow-ups, overdue tasks, backup alerts, scan alerts, marketing replies.
- Security and Privacy: login protection, auditability, consent, opt-out, secure storage.
- Company Profile: single source of truth for Smarto Life company data and branding.
- Help and Onboarding: non-technical guidance inside the app.
- Logs: system errors, scan failures, backup issues, sync problems, technical events.
- Templates: quotations, contact cards, labels, reports, PDFs.
- Retention and Archiving: retention policies, archival, cleanup without losing business history.

## Phased Delivery

### Phase 1

Build the application shell, dashboard, unified contact module, category system, tag system, business matter system, search, settings base, company profile base, and local database foundation.

### Phase 2

Add products, quotations, GPS capture, map view, and quotation-ready product line workflows.

### Phase 3

Add WhatsApp marketing, campaign segmentation, response tracking, QR business card scanning, and OCR physical card scanning.

### Phase 4

Add backup/restore, recycle bin, audit log, optimization, and final UI polish for core daily use.

### Phase 5

Add users, permissions, activity timeline, attachments, import/export, reminders, notification center, consent tracking, duplicate merge, help/onboarding, settings/defaults, security hardening, deployment readiness, logs, templates, retention, and future VPS/domain/cloud sync preparation.

## UI/UX Direction

- Orange primary theme, black secondary, white background.
- Premium, clean, modern, business-ready interface.
- Desktop-first with touchscreen-friendly spacing, large buttons, readable typography, and clear visual hierarchy.
- Reduce typing through dropdowns, chips, quick filters, defaults, recent values, templates, and guided forms.
- Keep navigation simple: Dashboard, Contacts, Products, Quotations, Marketing, Map, Scans, Reports, Settings.
- Provide consistent list, grid, detail, edit, search, filter, and action patterns across modules.

## Company Profile Defaults

- Company Name: Smarto Life
- Owner / Founder: Mohammed Rafi
- Email: Rafi@smartolife.com
- Slogan: Smart Solutions for Tomorrow
- Primary Color: Orange
- Secondary Color: Black
- Background: White

Company profile data must be editable from Settings and reused by quotations, reports, PDF templates, backup reports, email templates, splash/login screens, and future generated documents. Do not hard-code company details elsewhere.

