# Task 01 – EF Core Modeling Drill Pack

## Overview

10 practical EF Core drills covering database modeling, relationships, seed data, soft delete, audit fields, DTO projection, and pagination.

## Drills

### Drill 01 – DbContext & First Migration

**Concept:** DbContext / DbSet / Migration

Create the Student entity and `AppDbContext`, configure SQL Server, create the initial migration, and verify the `Students` table.

**Evidence:** Migration files + SQL Server table screenshot.

---

### Drill 02 – One-to-One Relationship

**Concept:** Student → StudentProfile

Create a one-to-one relationship between `Student` and `StudentProfile` using a foreign key and navigation properties.

**Evidence:** ERD or database screenshot proving the relationship.

---

### Drill 03 – One-to-Many Relationship

**Concept:** Instructor → TrainingTracks

An instructor can teach many tracks, while each track has one main instructor. Use `InstructorId` as the foreign key and configure navigation properties.

**Evidence:** Database relationship + GET response.

---

### Drill 04 – Many-to-Many via Join Entity

**Concept:** Student + Track → Enrollment

Create `Enrollment` as a join entity because the relationship contains additional business data such as status, enrollment date, and final grade.

**Evidence:** ERD + Enrollment table + sample response.

---

### Drill 05 – One-to-One Payment Summary

**Concept:** Enrollment → PaymentSummary

Create a one-to-one `PaymentSummary` for each enrollment with payment amounts and payment status.

**Evidence:** Screenshot showing the payment summary relationship.

---

### Drill 06 – Seed Data

**Concept:** HasData / Manual Seed

Add realistic initial data for students, instructors, tracks, and enrollments. Seed data should be repeatable without creating duplicates.

**Evidence:** Swagger or SQL Server screenshot showing seeded records.

---

### Drill 07 – Soft Delete

**Concept:** IsDeleted / DeletedAt

Implement soft delete instead of physically removing records. Deleted records should be excluded from normal GET requests.

**Evidence:** Before/after database + API response.

---

### Drill 08 – Audit Fields

**Concept:** CreatedAt / UpdatedAt

Track when records are created and updated using UTC time. Audit fields should be handled by the system, not entered manually by users.

**Evidence:** API response or database screenshot showing audit fields.

---

### Drill 09 – Projection DTO

**Concept:** Select Projection

Return DTOs instead of exposing EF Core entities directly. Use `Select()` to return only the fields required by the use case.

**Evidence:** Response screenshot + projection code snippet.

---

### Drill 10 – Pagination

**Concept:** Skip / Take / Total Count

Implement server-side pagination using `pageNumber` and `pageSize`.

Use:

```text
skip = (pageNumber - 1) × pageSize
```

Return:

```text
items
totalCount
pageNumber
pageSize
totalPages
```

Validate `pageNumber > 0` and `pageSize` between `1` and `50`.

**Evidence:** Swagger screenshot + pagination formula explanation.

---

## Completion Checklist

* [x] Drill 01 – DbContext & First Migration
* [x] Drill 02 – One-to-One Relationship
* [x] Drill 03 – One-to-Many Relationship
* [x] Drill 04 – Many-to-Many via Join Entity
* [x] Drill 05 – One-to-One Payment Summary
* [x] Drill 06 – Seed Data
* [x] Drill 07 – Soft Delete
* [x] Drill 08 – Audit Fields
* [x] Drill 09 – Projection DTO
* [x] Drill 10 – Pagination

## Key Learning

These drills build a practical foundation in **EF Core modeling, relationships, database migrations, data management, querying, DTOs, and pagination** before moving to the main Training Center API.
