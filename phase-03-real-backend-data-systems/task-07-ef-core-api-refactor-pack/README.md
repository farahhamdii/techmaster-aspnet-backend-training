# Task 07 - EF Core/API Refactor Pack

## Overview

This task focuses on refactoring a poorly structured EF Core API into cleaner, safer, and review-ready backend code.

The original implementation contained several common backend problems such as synchronous database operations, returning EF Core entities directly, missing validation, hard deletes, duplicated queries, and business logic inside the controller.

The refactored implementation improves the existing Enrollment API while preserving its business purpose and functionality.

---

## Task Objectives

The main objectives of this task are:

* Refactor bad EF Core/API code without breaking its functionality.
* Improve API structure and separation of responsibilities.
* Use DTOs instead of exposing EF Core entities.
* Move business logic into the service layer.
* Use asynchronous EF Core operations.
* Apply projection instead of returning full entities.
* Add pagination to list endpoints.
* Implement proper validation and HTTP status codes.
* Prevent duplicate active enrollments.
* Validate training track capacity.
* Validate payment amounts.
* Replace hard delete with soft delete.
* Improve API error responses.

---

# Original Bad Code

The original bad implementation was provided as part of the TechMaster Task 07 requirements.

The original code used a controller that directly accessed the database and performed CRUD operations without proper separation of concerns.

Example:

```csharp
[HttpGet]
public IActionResult GetAll()
{
    var data = _db.Enrollments
        .Include(e => e.Student)
        .Include(e => e.TrainingTrack)
        .Include(e => e.Payments)
        .ToList();

    return Ok(data);
}
```

The original implementation was preserved conceptually as the baseline for comparison and refactoring.

---

# Problems Found in the Original Code

## 1. Synchronous EF Core operations

The original code used:

```csharp
.ToList();
```

and:

```csharp
.SaveChanges();
```

This blocks the request thread while database operations are running.

---

## 2. EF Core entities were accepted directly

The create endpoint accepted:

```csharp
Create(Enrollment enrollment)
```

Accepting entities directly from API requests can expose internal database structure and allows clients to send fields they should not control.

---

## 3. EF Core entities were returned directly

The GET endpoint returned complete `Enrollment` entities.

This can expose navigation properties and unnecessary database information.

---

## 4. No pagination

The original GET endpoint returned all enrollments at once.

This can become inefficient when the database contains a large number of records.

---

## 5. No projection

The original code loaded:

```csharp
Student
TrainingTrack
Payments
```

using `Include()` and then returned the full entity graph.

Only the required response fields should be selected.

---

## 6. Business logic was inside the controller

Enrollment creation, payment creation, and database operations were performed directly inside the controller.

This makes the controller difficult to maintain and test.

---

## 7. No duplicate enrollment check

The original implementation allowed the same student to create multiple active enrollments for the same training track.

---

## 8. Training track capacity was ignored

The original code did not check whether the training track had reached its capacity before creating an enrollment.

---

## 9. Payment amount was not validated

The original payment endpoint accepted any amount, including zero or negative values.

---

## 10. Incorrect HTTP status codes

The original code returned:

```csharp
return Ok("not found");
```

when an enrollment did not exist.

A missing resource should return:

```http
404 Not Found
```

---

## 11. Hard delete was used

The original code used:

```csharp
_db.Enrollments.Remove(item);
```

This permanently removes historical enrollment data.

---

## 12. No soft delete

Historical records should remain available for reporting and auditing.

Soft delete is more appropriate for enrollment records.

---

## 13. Synchronous database save during payment

The payment endpoint used:

```csharp
_db.SaveChanges();
```

instead of an asynchronous operation.

---

## 14. No consistent error response structure

The original implementation returned plain strings such as:

```text
"not found"
```

and:

```text
"deleted"
```

instead of using a consistent API response structure.

---

# Refactoring Improvements

## 1. Async EF Core operations

Database operations were changed to asynchronous methods such as:

```csharp
ToListAsync()
FirstOrDefaultAsync()
AnyAsync()
CountAsync()
SaveChangesAsync()
```

This improves scalability and avoids blocking request threads.

---

## 2. DTO-based requests

Enrollment and payment requests use DTOs instead of accepting EF Core entities directly.

Example:

```csharp
CreateEnrollmentRequest
```

This provides better control over the API contract.

---

## 3. DTO-based responses

The API returns:

```csharp
EnrollmentDetailsResponse
```

instead of returning the `Enrollment` entity directly.

---

## 4. Service layer

Business logic is handled by:

```text
EnrollmentService
PaymentService
```

The controller is responsible mainly for:

* Receiving requests
* Calling services
* Returning HTTP responses

---

## 5. Projection

The enrollment query uses:

```csharp
.Select(e => new EnrollmentDetailsResponse
{
    ...
})
```

Only the required fields are selected from the database.

---

## 6. AsNoTracking for read operations

Read-only queries use:

```csharp
.AsNoTracking()
```

This avoids unnecessary EF Core change tracking for data that is not being modified.

---

## 7. Pagination

The refactored list endpoint supports:

```text
pageNumber
pageSize
```

and uses:

```csharp
Skip()
Take()
```

before executing the database query.

The response also provides:

* Current page
* Page size
* Total count
* Total pages

---

## 8. Duplicate enrollment validation

Before creating an enrollment, the service checks whether the student already has an active or pending enrollment in the same track.

---

## 9. Track capacity validation

Before creating an enrollment, the service checks the number of active enrollments against the training track capacity.

---

## 10. Payment validation

The payment endpoint validates that:

```text
Amount > 0
```

Invalid payment amounts return:

```http
400 Bad Request
```

---

## 11. Soft delete

Instead of permanently deleting an enrollment:

```csharp
_db.Enrollments.Remove(item);
```

the refactored implementation sets:

```csharp
enrollment.IsDeleted = true;
```

The record remains in the database while being excluded from normal active queries.

---

## 12. Correct HTTP status codes

The refactored API uses appropriate status codes:

| Situation                                |     Status Code |
| ---------------------------------------- | --------------: |
| Successful GET                           |          200 OK |
| Successful update                        |          200 OK |
| Successful delete                        |          200 OK |
| Successful creation                      |     201 Created |
| Invalid request                          | 400 Bad Request |
| Resource not found                       |   404 Not Found |
| Duplicate enrollment / business conflict |    409 Conflict |

---

## 13. Consistent error response

Errors use the existing:

```csharp
ApiResponse<T>
```

structure.

Example:

```json
{
  "success": false,
  "message": "Enrollment not found."
}
```

---

## 14. Soft-deleted records are excluded

Normal enrollment queries filter out records where:

```csharp
IsDeleted == true
```

This prevents deleted records from appearing in normal API responses.

---

# Before vs After

## Before

```text
Controller
    ↓
DbContext
    ↓
Database
```

The controller handled database access and business rules directly.

## After

```text
Controller
    ↓
Service
    ↓
DbContext
    ↓
Database
```

The refactored structure separates API responsibilities from business and data-access logic.

---

# Refactored Endpoints

## Get paginated enrollments

```http
GET /api/refactored-enrollments?pageNumber=1&pageSize=10
```

Optional filters:

```text
status
trackId
studentId
paymentStatus
```

Example:

```http
GET /api/refactored-enrollments?status=Active&pageNumber=1&pageSize=10
```

---

## Get enrollment by ID

```http
GET /api/refactored-enrollments/{id}
```

Returns `404 Not Found` when the enrollment does not exist.

---

## Create enrollment

```http
POST /api/refactored-enrollments
```

The service validates:

* Student existence
* Student activity
* Training track existence
* Training track activity
* Duplicate enrollment
* Track capacity

---

## Create payment

```http
POST /api/refactored-enrollments/pay
```

The payment amount must be greater than zero.

---

## Soft delete enrollment

```http
DELETE /api/refactored-enrollments/{id}
```

The enrollment is marked as deleted instead of being physically removed.

---

# Example Paginated Response

```json
{
  "success": true,
  "message": "Enrollments retrieved successfully.",
  "data": {
    "items": [],
    "pageNumber": 1,
    "pageSize": 10,
    "totalCount": 4,
    "totalPages": 1
  }
}
```

---



# Final Refactoring Result

The final implementation provides a cleaner backend structure while preserving the original enrollment business purpose.

The main improvements are:

* DTO-based API contracts
* Service-layer business logic
* Async EF Core operations
* Projection
* Pagination
* Validation
* Duplicate enrollment protection
* Track capacity validation
* Payment validation
* Soft delete
* Correct HTTP status codes
* Consistent error responses
* Better separation of responsibilities

The refactoring focuses on improving the existing implementation rather than rewriting the system without a clear reason.
