# Drill 07 - Soft Delete Drill

## Overview

This drill implements the Soft Delete pattern for the `Student` entity.

Instead of permanently removing a student record from the database, the application marks the student as deleted using `IsDeleted` and stores the deletion timestamp in `DeletedAt`.

Normal API queries exclude deleted students, while a separate debug/admin endpoint can retrieve all records, including deleted students.

---

## Objectives

* Add `IsDeleted` to the `Student` entity.
* Add nullable `DeletedAt` to the `Student` entity.
* Implement a Soft Delete operation.
* Prevent physical deletion of student records.
* Exclude deleted students from normal GET requests.
* Provide an endpoint to retrieve deleted records when required.
* Create an EF Core migration.
* Verify that deleted rows remain in the database.

---

## Soft Delete Concept

Instead of permanently deleting a record:

```text
DELETE FROM Students
```

the application updates the record:

```text
IsDeleted = true
DeletedAt = current timestamp
```

Example:

```text
Before:
IsDeleted = false
DeletedAt = NULL

After:
IsDeleted = true
DeletedAt = 2026-09-14...
```

The original database row remains available.

---

## Student Entity Changes

The following properties were added to the `Student` entity:

```csharp
public bool IsDeleted { get; set; }

public DateTime? DeletedAt { get; set; }
```

`DeletedAt` is nullable because a student that has not been deleted does not have a deletion timestamp.

---

## Soft Delete Behavior

The DELETE endpoint does not use:

```csharp
_context.Students.Remove(student);
```

Instead, it updates the deletion fields:

```csharp
student.IsDeleted = true;
student.DeletedAt = DateTime.UtcNow;

await _context.SaveChangesAsync();
```

This keeps the database row while marking it as deleted.

---

## Delete Endpoint

### Request

```http
DELETE /api/Student/{id}
```

Example:

```http
DELETE /api/Student/105
```

### Expected Response

```text
204 No Content
```

The student is not physically removed from the database.

---

## Normal GET Behavior

The normal student list excludes deleted records:

```csharp
var students = await _context.Students
    .Where(s => !s.IsDeleted)
    .ToListAsync();
```

Therefore, after deleting Student `105`, the following request does not return that student:

```http
GET /api/Student
```

---

## Student Tracks Endpoint

The Student Tracks endpoint also excludes deleted students.

```csharp
var student = await _context.Students
    .Include(s => s.Enrollments)
    .ThenInclude(e => e.Track)
    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
```

Therefore, a deleted student cannot retrieve tracks through the normal endpoint.

Example:

```http
GET /api/Student/105/tracks
```

If Student `105` is soft deleted, the endpoint returns:

```text
404 Not Found
```

---

## Include Deleted Records

A separate endpoint is provided for admin/debug purposes:

```http
GET /api/Student/all-including-deleted
```

This endpoint retrieves all student records, including records where:

```text
IsDeleted = true
```

This makes it possible to verify that a soft-deleted record still exists in the database.

---

## Database Verification

### Before Delete

The student record initially contains:

```text
IsDeleted = 0
DeletedAt = NULL
```

Query:

```sql
SELECT Id, FullName, IsDeleted, DeletedAt
FROM Students
WHERE Id = 105;
```

---

### After Delete

After calling:

```http
DELETE /api/Student/105
```

the same row remains in the database, but its values become:

```text
IsDeleted = 1
DeletedAt = <deletion timestamp>
```

Query:

```sql
SELECT Id, FullName, IsDeleted, DeletedAt
FROM Students
WHERE Id = 105;
```

This proves that the record was soft deleted instead of physically removed.

---

## Migration

The migration created for this drill is:

```text
AddSoftDeleteFields
```

Commands:

```powershell
dotnet build
dotnet ef migrations add AddSoftDeleteFields
dotnet ef database update
```

---

## Test Scenario

The following scenario was used to verify the implementation.

### Step 1 - Check Student Before Delete

```http
GET /api/Student
```

Student `105` is visible.

Database:

```text
Id = 105
IsDeleted = 0
DeletedAt = NULL
```

### Step 2 - Delete Student

```http
DELETE /api/Student/105
```

Expected:

```text
204 No Content
```

### Step 3 - Check Database

```sql
SELECT Id, FullName, IsDeleted, DeletedAt
FROM Students
WHERE Id = 105;
```

The row still exists with:

```text
IsDeleted = 1
DeletedAt = <timestamp>
```

### Step 4 - Check Normal GET

```http
GET /api/Student
```

Student `105` is no longer returned.

### Step 5 - Check Include Deleted Endpoint

```http
GET /api/Student/all-including-deleted
```

Student `105` appears with:

```text
IsDeleted = true
```

---

## Expected Behavior

| Test Case                | Expected Result               |
| ------------------------ | ----------------------------- |
| Delete student           | Record is soft deleted        |
| Database row             | Remains in database           |
| `IsDeleted`              | Changes to `true`             |
| `DeletedAt`              | Stores deletion timestamp     |
| Normal GET               | Excludes deleted students     |
| Student tracks           | Deleted students are excluded |
| Include deleted endpoint | Returns deleted records       |
| Physical DELETE          | Not performed                 |

---

## Key Learning

This drill demonstrates how Soft Delete can be used to preserve database records while preventing them from appearing in normal application queries.

Key concepts:

* Soft Delete
* `IsDeleted`
* `DeletedAt`
* Conditional query filtering
* Database record preservation
* EF Core migrations
* API behavior after soft deletion
* Admin/debug access to deleted records
