# Task 04 - Querying, Filtering & Reporting

## Overview

This task focuses on building real data queries using **Entity Framework Core and LINQ**.

The implemented queries cover:

* Filtering
* Searching
* Sorting
* Pagination
* DTO Projection
* Aggregation
* Grouping
* Reporting
* Conditional `IQueryable` composition

All query results are returned through readable DTOs instead of exposing EF Core entities directly.

---

## Technologies

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* LINQ
* Swagger
* Postman

---

## Query Implementations

### Query 01 - Student Search

Search students using optional search criteria.

**Concepts:**

* `Where`
* `Contains`

---

### Query 02 - Student Active Filter

Filter students based on their active status.

**Concepts:**

* Conditional filtering
* `Where`

---

### Query 03 - Paged Students List

Returns students using pagination with metadata.

**Endpoint:**

```http
GET /api/students?pageNumber=1&pageSize=10
```

**Response includes:**

* `totalCount`
* `totalPages`
* `pageNumber`
* `pageSize`
* `items`

**Concepts:**

* `CountAsync`
* `Skip`
* `Take`
* `OrderBy`
* DTO Projection

---

### Query 04 - Track Search

Search tracks by title, code, or description.

**Endpoint:**

```http
GET /api/tracks?keyword=backend
```

Only active/open tracks are returned by default.

**Concepts:**

* `Where`
* `Contains`

---

### Query 05 - Filter Tracks By Level

Filter tracks using their level.

**Endpoint:**

```http
GET /api/tracks?level=Beginner
```

Invalid enum values are rejected.

**Concepts:**

* Enum validation
* `Where`

---

### Query 06 - Filter Tracks By Instructor

Returns tracks assigned to a specific instructor.

**Endpoint:**

```http
GET /api/tracks?instructorId=2
```

**Concepts:**

* `Where`
* Foreign key filtering

---

### Query 07 - Tracks With Available Seats

Returns tracks where the capacity is greater than the number of active enrollments.

**Endpoint:**

```http
GET /api/reports/tracks-with-available-seats
```

The response includes:

* Track
* Capacity
* Active enrollments
* Remaining seats

**Concepts:**

* `Count`
* Related data querying
* DTO Projection
* Calculated fields

---

### Query 08 - Enrollment List With Details

Returns enrollment information with related student and track information.

**Endpoint:**

```http
GET /api/enrollments
```

The API returns DTOs instead of exposing the complete entity graph.

**Concepts:**

* `Include`
* `Select`
* DTO Projection

---

### Query 09 - Filter Enrollments By Status

Filters enrollments by status.

**Endpoint:**

```http
GET /api/enrollments?status=Pending
```

Supported statuses:

* Pending
* Active
* Completed
* Cancelled

**Concepts:**

* Enum validation
* `Where`

---

### Query 10 - Student Enrollment History

Returns all enrollments for a specific student with track information.

**Endpoint:**

```http
GET /api/students/{id}/enrollments
```

Returns `404 Not Found` when the student does not exist.

**Concepts:**

* `Where`
* `Select`
* DTO Projection

---

### Query 16 - Top Tracks By Enrollment

Returns tracks ordered by active enrollment count.

**Endpoint:**

```http
GET /api/reports/top-tracks
```

Default:

```text
Top 5
```

A custom limit can be provided:

```http
GET /api/reports/top-tracks?top=10
```

**Concepts:**

* `Where`
* `GroupBy`
* `Count`
* `OrderByDescending`
* `Take`

---

### Query 17 - Instructor Workload

Returns instructor workload information for management reporting.

**Endpoint:**

```http
GET /api/reports/instructor-workload
```

The response includes:

* Instructor
* Number of tracks
* Number of active students

**Concepts:**

* Related data querying
* `Select`
* `SelectMany`
* `Count`

---

### Query 18 - Students Without Payments

Returns students who have an active or pending enrollment without a payment.

Cancelled enrollments are excluded.

**Endpoint:**

```http
GET /api/reports/students-without-payments
```

**Concepts:**

* `Any`
* Related collections
* Conditional filtering
* DTO Projection

---

### Query 19 - Advanced Enrollment Filter

Combines multiple optional filters in a single query.

**Endpoint:**

```http
GET /api/enrollments
```

Example:

```http
GET /api/enrollments?trackId=1&status=Active&paymentStatus=Paid
```

Each filter is applied only when its parameter has a value.

**Concepts:**

* `IQueryable`
* Conditional query composition
* `Where`
* Enum validation
* DTO Projection

---

## Query Design

The queries were designed to execute filtering and aggregation at the database level whenever possible.

Example:

```csharp
IQueryable<Enrollment> query = _context.Enrollments;

if (trackId.HasValue)
{
    query = query.Where(e => e.TrackId == trackId.Value);
}

if (!string.IsNullOrWhiteSpace(status))
{
    query = query.Where(e => e.Status == enrollmentStatus);
}

var result = await query.ToListAsync();
```

This allows multiple optional filters to be combined before executing the query.

---

## Validation

Invalid query parameters are handled with appropriate validation.

Examples:

* Invalid page number
* Invalid page size
* Invalid enum values
* Invalid `top` value
* Invalid filter values

Invalid requests return:

```http
400 Bad Request
```

---

## DTO Usage

Query responses use dedicated DTOs such as:

* `StudentListItemResponse`
* `PagedResultDto<T>`
* `TrackAvailableSeatsResponse`
* `TopTrackResponse`
* `InstructorWorkloadResponse`
* `StudentWithoutPaymentResponse`
* `EnrollmentFilterResponse`

This prevents exposing EF Core entities directly through the API.

---

## Testing

Each implemented endpoint was tested using:

* Swagger
* Postman

Testing covered:

* Valid requests
* Invalid parameters
* Empty results
* Pagination
* Filtering
* Reporting results
* Combined filters

---

## Selected Query Explanations

The following queries were selected for deeper technical explanation:

### 1. Query 03 - Pagination

Uses:

```text
CountAsync
Skip
Take
```

to return paginated data together with pagination metadata.

### 2. Query 07 - Available Seats

Counts active enrollments for each track and calculates:

```text
RemainingSeats = Capacity - ActiveEnrollments
```

### 3. Query 16 - Top Tracks

Uses:

```text
GroupBy
Count
OrderByDescending
Take
```

to identify tracks with the highest number of active enrollments.

### 4. Query 18 - Students Without Payments

Uses `Any()` on related payments to identify students with active/pending enrollments that have no payment.

### 5. Query 19 - Advanced Filtering

Uses `IQueryable` to build the query conditionally based on the provided parameters.

---

## Key Learning Outcomes

By completing this task, the following skills were practiced:

* Writing LINQ queries
* Filtering EF Core data
* Searching with `Contains`
* Implementing pagination
* Projecting entities into DTOs
* Grouping and aggregation
* Querying related entities
* Using `Any()` and `Count()`
* Building reports
* Composing dynamic `IQueryable` queries
* Validating query parameters
* Testing APIs using Swagger and Postman

---

## Conclusion

Task 04 demonstrates how to move beyond basic CRUD operations and build practical, data-driven queries and reports using **EF Core and LINQ**.

The implemented endpoints focus on readable DTO-based responses, database-side querying, validation, filtering, pagination, and reporting.
