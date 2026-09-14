# Drill 09 – Projection DTO Drill

## 📌 Concept

**Projection with LINQ**

This drill demonstrates how to use LINQ `Select()` to project EF Core entities into DTOs before returning data from the API.

The goal is to return only the required fields instead of exposing the complete EF Core entity directly.

---

## 🎯 Objective

* Understand DTO projection using LINQ.
* Use `Select()` to map an Entity to a DTO.
* Return API responses using DTOs.
* Avoid exposing EF Core entities directly.
* Select only the required properties from the database.

---

## 🏗️ Scenario

The API contains a `Student` entity.

Instead of returning the complete `Student` entity directly, the API should return a `StudentDto` containing only the required information.

### Student Entity

The entity contains the student's database properties.

### Student DTO

The DTO contains only the fields that should be exposed through the API.



---

## 🔧 Implementation

The projection is performed using LINQ `Select()`.

```csharp
var students = await _context.Students
    .Select(s => new StudentlistItemDto
    {
        Id = s.Id,
        FullName = s.FullName,
         Email = s.Email,
        IsActive = s.IsActive
    })
    .ToListAsync();
```


## 🌐 API Response

The endpoint returns the projected DTO instead of the complete EF Core entity.

Example response:

```json
[
 {
    "id": 101,
    "fullName": "Ahmed Ali",
    "email": "ahmed.ali@techmaster.com",
    "isActive": true
  },
  {
    "id": 102,
    "fullName": "Mona Hassan",
    "email": "mona.hassan@techmaster.com",
    "isActive": true
  },
]
```

---



## 💡 Why Projection?

Projection is useful because the API does not always need all entity properties.

Instead of:

```csharp
_context.Students.ToListAsync();
```

we use:

```csharp
_context.Students
    .Select(s => new StudentlistItemDto
    {
        Id = s.Id,
       FullName = s.FullName,
        Email = s.Email,
        IsActive = s.IsActive
    })
    .ToListAsync();
```

This allows the application to request only the required data.

### Benefits

* Prevents exposing unnecessary entity properties.
* Reduces the amount of data returned by the API.
* Makes API responses cleaner.
* Improves separation between database entities and API contracts.
* Helps control exactly what the client receives.



---

## 📝 Key Learning

This drill demonstrates the difference between **returning an Entity** and **returning a DTO using projection**.

### Entity

```text
Student
   ↓
Database Model
```

### DTO Projection

```text
Student Entity
      ↓
    Select()
      ↓
 StudentlistItemDto
      ↓
   API Response
```

The API controls the response shape by selecting only the required properties.

---

---

## 📚 Key Takeaway

> **Projection means selecting the data you need and creating a DTO instead of returning the entire Entity.**

Using `Select()` with DTOs provides a clean and controlled API response while keeping database entities separated from the API contract.
