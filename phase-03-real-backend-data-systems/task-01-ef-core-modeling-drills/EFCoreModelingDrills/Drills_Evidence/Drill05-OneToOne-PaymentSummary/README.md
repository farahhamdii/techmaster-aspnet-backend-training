# Drill 05 - One-to-One Payment Summary

## Overview

This drill implements a One-to-One relationship between an `Enrollment` and its `PaymentSummary`.

Each enrollment can have one payment summary that tracks the total required amount, total paid amount, remaining amount, and current payment status.

The relationship is designed to support future payment-related extensions.

---

## Objectives

* Create a `PaymentSummary` entity.
* Establish a One-to-One relationship between `Enrollment` and `PaymentSummary`.
* Store payment amounts using decimal data types.
* Ensure each enrollment has at most one payment summary.
* Add payment status tracking.
* Retrieve enrollment payment information through a DTO.
* Apply database constraints and create an EF Core migration.

---

## Entity Relationship

```text
Enrollment
    │
    │ 1
    │
    │ 1
PaymentSummary
```

### Relationship Details

* One `Enrollment` can have one `PaymentSummary`.
* `PaymentSummary` contains the foreign key `EnrollmentId`.
* `EnrollmentId` is configured as a unique foreign key.
* The unique constraint prevents multiple payment summaries from being created for the same enrollment.
* Cascade delete is configured from `Enrollment` to `PaymentSummary`.

---

## PaymentSummary Fields

| Field           | Type       | Description                 |
| --------------- | ---------- | --------------------------- |
| Id              | int        | Primary key                 |
| EnrollmentId    | int        | Foreign key to Enrollment   |
| TotalRequired   | decimal    | Total amount required       |
| TotalPaid       | decimal    | Total amount already paid   |
| RemainingAmount | decimal    | Remaining amount to be paid |
| PaymentStatus   | string     | Current payment status      |
| Enrollment      | Enrollment | Navigation property         |

---

## Payment Status

The payment status is stored as a string.

Supported sample values:

```text
Pending
PartiallyPaid
Paid
```

Example:

```text
TotalRequired = 5000.00
TotalPaid = 2500.00
RemainingAmount = 2500.00
PaymentStatus = PartiallyPaid
```

---

## Decimal Configuration

All money-related fields are stored using:

```text
decimal(18,2)
```

This is used for:

* `TotalRequired`
* `TotalPaid`
* `RemainingAmount`

Using decimal provides appropriate precision for monetary values.

---

## EF Core Configuration

The relationship is configured using:

```csharp
builder.HasOne(p => p.Enrollment)
       .WithOne(e => e.PaymentSummary)
       .HasForeignKey<PaymentSummary>(p => p.EnrollmentId)
       .IsRequired()
       .OnDelete(DeleteBehavior.Cascade);

builder.HasIndex(p => p.EnrollmentId)
       .IsUnique();
```

The unique index on `EnrollmentId` ensures that an enrollment cannot have more than one payment summary.

---

## Migration

The following migration was created:

```text
AddPaymentSummary
```

Commands:

```powershell
dotnet build
dotnet ef migrations add AddPaymentSummary
dotnet ef database update
```

---

## API Endpoint

### Get Enrollment Payment Summary

```http
GET /api/Enrollment/{id}/payment-summary
```

Example:

```http
GET /api/Enrollment/1/payment-summary
```

The endpoint loads the enrollment together with its payment summary and returns a DTO instead of exposing the entity directly.

---

## DTO

The API uses:

```text
EnrollmentPaymentSummaryDto
```

The DTO contains:

```text
EnrollmentId
StudentId
TrackId
TotalRequired
TotalPaid
RemainingAmount
PaymentStatus
```

This keeps the API response focused on the required data and avoids exposing entity navigation properties.

---

## Sample Response

```json
{
  "enrollmentId": 1,
  "studentId": 1,
  "trackId": 1,
  "totalRequired": 5000.00,
  "totalPaid": 2500.00,
  "remainingAmount": 2500.00,
  "paymentStatus": "PartiallyPaid"
}
```

---

## Validation / Expected Behavior

### Existing Enrollment With Payment Summary

Returns:

```text
200 OK
```

with the payment summary DTO.

### Invalid Enrollment

Returns:

```text
404 Not Found
```

with:

```text
Enrollment not found.
```

### Enrollment Without Payment Summary

Returns:

```text
404 Not Found
```

with:

```text
Payment summary not found.
```

---

## Database Verification

The relationship can be verified using:

```sql
SELECT * FROM PaymentSummaries;
```

and:

```sql
SELECT
    p.Id,
    p.EnrollmentId,
    p.TotalRequired,
    p.TotalPaid,
    p.RemainingAmount,
    p.PaymentStatus
FROM PaymentSummaries p;
```

---


---

## Key Learning

This drill demonstrates how to:

* Configure One-to-One relationships in EF Core.
* Place a unique foreign key on the dependent entity.
* Configure decimal precision for monetary values.
* Apply unique database constraints.
* Retrieve related data using `Include`.
* Use DTOs to control API responses.
* Create and apply EF Core migrations.
