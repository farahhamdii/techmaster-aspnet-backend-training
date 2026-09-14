# Drill 10 – Pagination Drill

## 📌 Concept

**Server-Side Pagination using Skip, Take, and Total Count**

This drill demonstrates how to implement proper server-side pagination for a student listing.

The API accepts `pageNumber` and `pageSize` as query parameters and returns the requested page together with pagination metadata.

---

## 🎯 Objective

The goal of this drill is to:

* Accept `pageNumber` and `pageSize` from query parameters.
* Validate pagination parameters.
* Use LINQ `Skip()` and `Take()`.
* Calculate the total number of records.
* Calculate the total number of pages.
* Return pagination metadata with the requested items.
* Implement pagination on the server side.

---

## 🏗️ Scenario

The API contains a list of students.

Instead of returning all students in one response, the API divides the data into pages.

The client can specify:

```text
pageNumber
pageSize
```

Example:

```http
GET /api/Student/paged?pageNumber=1&pageSize=10
```

This means:

> Return page 1 with a maximum of 10 students.

---

## 📦 Pagination Result DTO

A generic `PaginationResult<T>` DTO is used to return both the data and its pagination metadata.

```csharp
public class PaginationResult<T>
{
    public List<T> Items { get; set; } = new();

    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalPages { get; set; }
}
```

### Response Properties

| Property     | Description                              |
| ------------ | ---------------------------------------- |
| `Items`      | Students returned for the requested page |
| `TotalCount` | Total number of students                 |
| `PageNumber` | Current page number                      |
| `PageSize`   | Number of items requested per page       |
| `TotalPages` | Total number of available pages          |

---

## 🔧 Implementation

The pagination logic is implemented inside `StudentService`.

```csharp
public async Task<PaginationResult<StudentListItemDto>> GetStudentsAsync(
    int pageNumber,
    int pageSize)
{
    var totalCount = await _context.Students
        .Where(s => !s.IsDeleted)
        .CountAsync();

    var totalPages = (int)Math.Ceiling(
        (double)totalCount / pageSize);

    var skip = (pageNumber - 1) * pageSize;

    var items = await _context.Students
        .Where(s => !s.IsDeleted)
        .Select(s => new StudentListItemDto
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            IsActive = s.IsActive
        })
        .Skip(skip)
        .Take(pageSize)
        .ToListAsync();

    return new PaginationResult<StudentListItemDto>
    {
        Items = items,
        TotalCount = totalCount,
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalPages = totalPages
    };
}
```

---

## 🧮 Pagination Formula

### Skip Formula

The number of records to skip is calculated using:

```text
skip = (pageNumber - 1) × pageSize
```

### Example

For:

```text
pageNumber = 1
pageSize = 10
```

The calculation is:

```text
skip = (1 - 1) × 10
     = 0
```

So the API starts from the first student.

For:

```text
pageNumber = 2
pageSize = 10
```

The calculation becomes:

```text
skip = (2 - 1) × 10
     = 10
```

So the API skips the first 10 students and returns the next 10.

---

## 📊 Total Pages Formula

The total number of pages is calculated using:

```text
totalPages = Ceiling(totalCount / pageSize)
```

For example, if:

```text
totalCount = 24
pageSize = 10
```

Then:

```text
totalPages = Ceiling(24 / 10)
           = 3
```

Therefore:

```text
Page 1 → Students 1–10
Page 2 → Students 11–20
Page 3 → Students 21–24
```

---

## 🔗 API Endpoint

```http
GET /api/Student/paged
```

### Query Parameters

```text
pageNumber
pageSize
```

Example:

```http
GET /api/Student/paged?pageNumber=1&pageSize=10
```

---

## ✅ Validation Rules

The API validates the pagination parameters before executing the query.

### `pageNumber`

`pageNumber` must be greater than `0`.

```csharp
if (pageNumber <= 0)
    return BadRequest("pageNumber must be greater than 0.");
```

### `pageSize`

`pageSize` must be between `1` and `50`.

```csharp
if (pageSize < 1 || pageSize > 50)
    return BadRequest("pageSize must be between 1 and 50.");
```

---

## 🧪 Required Test Cases

### Test Case 1 – First Page

**Request:**

```http
GET /api/Student/paged?pageNumber=1&pageSize=5
```

**Expected Behavior:**

* Returns `200 OK`.
* Returns the first page of students.
* `pageNumber` is `1`.
* `pageSize` is `5`.
* Response contains `totalCount` and `totalPages`.

---

### Test Case 2 – Invalid Page Number

**Request:**

```http
GET /api/Student/paged?pageNumber=0
```

**Expected Behavior:**

```text
400 Bad Request
```

Because:

```text
pageNumber must be greater than 0
```

---

### Test Case 3 – Invalid Page Size

**Request:**

```http
GET /api/Student/paged?pageSize=100
```

**Expected Behavior:**

```text
400 Bad Request
```

Because the maximum allowed page size is `50`.

---

### Test Case 4 – Requesting a Page Beyond Available Data

Example:

```http
GET /api/Student/paged?pageNumber=2&pageSize=10
```

If the database contains only 4 students:

```json
{
  "items": [],
  "totalCount": 4,
  "pageNumber": 2,
  "pageSize": 10,
  "totalPages": 1
}
```

This is a valid response because page 2 contains no records.

---

## 📸 Evidence

### Swagger Screenshot

Add a screenshot showing the successful pagination request and response in Swagger.

**Example request:**

```text
/api/Student/paged?pageNumber=2&pageSize=10
```

**Example response:**

```json
{
  "items": [],
  "totalCount": 4,
  "pageNumber": 2,
  "pageSize": 10,
  "totalPages": 1
}
```

> The empty `items` array is expected when requesting a page beyond the available pages.

---

## 🔍 Important LINQ Methods

### `CountAsync()`

```csharp
var totalCount = await _context.Students
    .Where(s => !s.IsDeleted)
    .CountAsync();
```

Counts the total number of non-deleted students.

### `Skip()`

```csharp
.Skip(skip)
```

Skips the records belonging to previous pages.

### `Take()`

```csharp
.Take(pageSize)
```

Returns only the number of records requested for the current page.

### `ToListAsync()`

```csharp
.ToListAsync();
```

Executes the query asynchronously and returns the requested records.

---

## 🔄 Pagination Flow

```text
Client
   ↓
pageNumber + pageSize
   ↓
Controller Validation
   ↓
StudentService
   ↓
CountAsync()
   ↓
Calculate TotalPages
   ↓
Calculate Skip
   ↓
Select DTO
   ↓
Skip()
   ↓
Take()
   ↓
ToListAsync()
   ↓
PaginationResult<T>
   ↓
API Response
```

---

## 💡 Why Server-Side Pagination?

Returning all records at once can become inefficient when the database contains a large amount of data.

Server-side pagination allows the API to return only the records required for the current page.

### Benefits

* Reduces the amount of data transferred.
* Reduces API response size.
* Improves performance for large datasets.
* Prevents loading unnecessary records.
* Provides a better experience for clients consuming large datasets.

---

## 📝 Key Learning

This drill demonstrates how to combine:

```text
CountAsync()
+
Skip()
+
Take()
+
DTO Projection
```

to build a proper server-side pagination system.

The important formulas are:

```text
skip = (pageNumber - 1) × pageSize
```

and:

```text
totalPages = Ceiling(totalCount / pageSize)
```

---


## 📚 Key Takeaway

> **Pagination allows the API to return a controlled subset of data instead of loading and returning all records at once.**

Using `Skip()` and `Take()` with `CountAsync()` provides the foundation for efficient server-side pagination in an EF Core API.
