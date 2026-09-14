# Drill 08 - Audit Fields

## Overview

This drill introduces audit fields used in production backend systems to track when records are created and updated.

The `Student` entity is used to demonstrate:

* `CreatedAt`
* `UpdatedAt`
* UTC-based timestamps
* Service-layer handling of audit fields
* Automatic timestamp assignment without asking the client to provide audit values

---

## Objectives

By completing this drill, the API should:

* Store the creation time of each student.
* Store the last update time of each student.
* Set `CreatedAt` automatically during creation.
* Set `UpdatedAt` automatically during update.
* Use UTC time.
* Prevent the client from manually controlling audit timestamps.
* Keep audit logic inside the service layer.

---

## Entity Changes

The `Student` entity contains the following audit fields:

```csharp
public DateTime CreatedAt { get; set; }

public DateTime? UpdatedAt { get; set; }
```

### CreatedAt

`CreatedAt` stores the date and time when the student was created.

It is set automatically using:

```csharp
student.CreatedAt = DateTime.UtcNow;
```

### UpdatedAt

`UpdatedAt` stores the date and time of the student's latest update.

It is initially `null` and is set during an update:

```csharp
existingStudent.UpdatedAt = DateTime.UtcNow;
```

---

## Why UpdatedAt Is Nullable

`UpdatedAt` is nullable because a newly created student has not been updated yet.

Example:

```text
CreatedAt  = 2026-09-14 15:30:00
UpdatedAt  = NULL
```

After the first update:

```text
CreatedAt  = 2026-09-14 15:30:00
UpdatedAt  = 2026-09-14 16:10:00
```

`CreatedAt` remains unchanged.

---

## UTC Time

The application uses:

```csharp
DateTime.UtcNow
```

instead of:

```csharp
DateTime.Now
```

UTC is preferred for backend systems because it provides a consistent time reference regardless of the server's local timezone.

Both creation and update timestamps are therefore stored using UTC time.

---

## Service Layer

Audit logic is implemented inside the `StudentService`.

### IStudentService

```csharp
public interface IStudentService
{
    Task<Student> CreateAsync(Student student);

    Task<Student?> UpdateAsync(int id, Student student);
}
```

### StudentService

During creation:

```csharp
student.CreatedAt = DateTime.UtcNow;
student.UpdatedAt = null;
```

During update:

```csharp
existingStudent.UpdatedAt = DateTime.UtcNow;
```

The service keeps the audit logic away from the controller and ensures that clients do not need to provide timestamps manually.

---

## Dependency Injection

The service is registered in `Program.cs`:

```csharp
builder.Services.AddScoped<IStudentService, StudentService>();
```

The controller receives the service through dependency injection.

---

## Create Student

### Endpoint

```http
POST /api/Student
```

### Request Body

The client does not provide `CreatedAt` or `UpdatedAt`.

```json
{
  "fullName": "Ali Hassan",
  "email": "ali.hassan@example.com",
  "isActive": true
}
```

### Expected Behavior

The service automatically sets:

```text
CreatedAt = current UTC time
UpdatedAt = NULL
```

### Example Response

```json
{
  "id": 106,
  "fullName": "Ali Hassan",
  "email": "ali.hassan@example.com",
  "createdAt": "2026-09-14T15:30:00Z",
  "updatedAt": null,
  "isActive": true
}
```

---

## Update Student

### Endpoint

```http
PUT /api/Student/{id}
```

### Request Body

```json
{
  "fullName": "Ali Hassan Updated",
  "email": "ali.updated@example.com",
  "isActive": true
}
```

The client does not provide `UpdatedAt`.

### Expected Behavior

The service automatically sets:

```csharp
existingStudent.UpdatedAt = DateTime.UtcNow;
```

`CreatedAt` remains unchanged.

### Example Response

```json
{
  "id": 106,
  "fullName": "Ali Hassan Updated",
  "email": "ali.updated@example.com",
  "createdAt": "2026-09-14T15:30:00Z",
  "updatedAt": "2026-09-14T16:10:00Z",
  "isActive": true
}
```

---

## Audit Behavior

| Operation     | CreatedAt         | UpdatedAt         |
| ------------- | ----------------- | ----------------- |
| Create        | Set automatically | `NULL`            |
| First Update  | Remains unchanged | Set automatically |
| Second Update | Remains unchanged | Updated again     |

This demonstrates the difference between creation and modification timestamps.

---

## Migration

After adding the audit fields, create and apply the migration:

```powershell
dotnet build

dotnet ef migrations add AddAuditFields

dotnet ef database update
```

The database will contain the new `UpdatedAt` column.

---

## Database Verification

The audit fields can be verified using:

```sql
SELECT
    Id,
    FullName,
    CreatedAt,
    UpdatedAt
FROM Students;
```


---

## Expected Test Cases

### Test Case 1 - Create

Send:

```http
POST /api/Student
```

Expected:

* Student is created.
* `CreatedAt` is automatically populated.
* `UpdatedAt` is `NULL`.

### Test Case 2 - Update

Send:

```http
PUT /api/Student/{id}
```

Expected:

* Student is updated.
* `CreatedAt` remains unchanged.
* `UpdatedAt` is automatically populated.

### Test Case 3 - UTC

Verify that timestamps are generated using:

```csharp
DateTime.UtcNow
```

and not:

```csharp
DateTime.Now
```

---

## Important Design Decision

The audit fields are handled inside the service layer instead of requiring the client to send them.

This prevents clients from controlling values such as:

```text
CreatedAt
UpdatedAt
```

and keeps the audit behavior centralized in the backend.

For this drill, the service-layer approach was selected instead of overriding `SaveChangesAsync`, because the task specifically requires practicing audit handling through create and update service methods.

---

## Key Learning

This drill demonstrates how production APIs track record history using audit fields.

Main concepts learned:

* `CreatedAt`
* `UpdatedAt`
* Nullable `DateTime`
* UTC timestamps
* Service-layer business logic
* Dependency Injection
* EF Core migrations
* Database verification
* Automatic audit field assignment
* Keeping client input separate from system-managed fields

---

## Result

The Student API now supports production-style audit tracking:

```text
Create Student
      ↓
CreatedAt = UTC Now
UpdatedAt = NULL

Update Student
      ↓
CreatedAt = unchanged
UpdatedAt = UTC Now
```

The audit timestamps are generated by the backend automatically and are not entered manually by the API consumer.
