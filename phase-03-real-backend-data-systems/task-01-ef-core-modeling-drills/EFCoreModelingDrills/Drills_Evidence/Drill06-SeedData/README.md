# Drill 06 - Seed Data Drill

## Overview

This drill introduces EF Core seed data using `HasData`.

The purpose is to provide realistic initial data that allows reviewers and mentors to test the API immediately without manually creating all required records.

The seed data includes students, instructors, tracks, and enrollments while maintaining the required foreign key relationships.

---

## Objectives

* Seed at least 5 students.
* Seed at least 2 instructors.
* Seed at least 3 tracks.
* Seed at least 5 enrollments.
* Use EF Core `HasData`.
* Keep seed data repeatable.
* Prevent duplicate seed records when the application restarts.
* Document known sample IDs for API testing.
* Verify seeded records in SQL Server and Swagger.

---

## Seed Strategy

EF Core `HasData` is used for the seed implementation.

Seed data is configured inside:

```text
Data/AppDbContext.cs
```

Example:

```csharp
modelBuilder.Entity<Student>().HasData(
    ...
);

modelBuilder.Entity<Instructor>().HasData(
    ...
);

modelBuilder.Entity<Track>().HasData(
    ...
);

modelBuilder.Entity<Enrollment>().HasData(
    ...
);
```

The data is inserted through EF Core migrations rather than being inserted every time the application starts.

---

## Seeded Data

### Students

At least 5 students are seeded.

|  ID | Name            |
| --: | --------------- |
| 101 | Ahmed Ali       |
| 102 | Mona Hassan     |
| 103 | Omar Mohamed    |
| 104 | Sara Mahmoud    |
| 105 | Youssef Ibrahim |

---

### Instructors

At least 2 instructors are seeded.

|  ID | Name        |
| --: | ----------- |
| 101 | Ahmed Samir |
| 102 | Nour Khaled |

---

### Tracks

At least 3 tracks are seeded.

|  ID | Track                | Instructor |
| --: | -------------------- | ---------: |
| 101 | ASP.NET Core Backend |        101 |
| 102 | Database & EF Core   |        101 |
| 103 | Web API Development  |        102 |

---

### Enrollments

At least 5 enrollments are seeded.

|  ID | Student | Track | Status    | Final Grade |
| --: | ------: | ----: | --------- | ----------: |
| 101 |     101 |   101 | Active    |           - |
| 102 |     101 |   102 | Completed |       92.50 |
| 103 |     102 |   101 | Active    |           - |
| 104 |     103 |   103 | Completed |       88.00 |
| 105 |     104 |   102 | Active    |           - |

---

## Known Sample IDs

These IDs are intentionally fixed so that mentors and reviewers can test the API using known records.

```text
Students:
101 - Ahmed Ali
102 - Mona Hassan
103 - Omar Mohamed
104 - Sara Mahmoud
105 - Youssef Ibrahim

Instructors:
101 - Ahmed Samir
102 - Nour Khaled

Tracks:
101 - ASP.NET Core Backend
102 - Database & EF Core
103 - Web API Development

Enrollments:
101 - Student 101 -> Track 101
102 - Student 101 -> Track 102
103 - Student 102 -> Track 101
104 - Student 103 -> Track 103
105 - Student 104 -> Track 102
```

---

## Seed Relationships

The seeded records respect the existing database relationships:

```text
Instructor 1
    │
    ├── Track 101
    └── Track 102

Instructor 2
    │
    └── Track 103

Student 101
    │
    ├── Enrollment 101 → Track 101
    └── Enrollment 102 → Track 102

Student 102
    │
    └── Enrollment 103 → Track 101

Student 103
    │
    └── Enrollment 104 → Track 103

Student 104
    │
    └── Enrollment 105 → Track 102
```

---

## Migration

A migration was created to apply the seed data:

```text
AddSeedData
```

Commands:

```powershell
dotnet build
dotnet ef migrations add AddSeedData
dotnet ef database update
```

---

## Repeatability

The seed data is repeatable because it is managed by EF Core migrations.

Restarting the application does not execute a new set of INSERT statements.

Therefore, restarting the application does not create duplicate seed rows.

The seeded records use fixed primary keys, allowing EF Core to track the seed data consistently.

---

## Database Verification

Seeded students can be checked using:

```sql
SELECT *
FROM Students
WHERE Id BETWEEN 101 AND 105;
```

Instructors:

```sql
SELECT *
FROM Instructors
WHERE Id BETWEEN 101 AND 102;
```

Tracks:

```sql
SELECT *
FROM Tracks
WHERE Id BETWEEN 101 AND 103;
```

Enrollments:

```sql
SELECT *
FROM Enrollments
WHERE Id BETWEEN 101 AND 105;
```

---

## Duplicate Verification

After restarting the application, the number of seeded students should remain:

```sql
SELECT COUNT(*) AS SeedStudents
FROM Students
WHERE Id BETWEEN 101 AND 105;
```

Expected result:

```text
5
```

Seeded enrollments:

```sql
SELECT COUNT(*) AS SeedEnrollments
FROM Enrollments
WHERE Id BETWEEN 101 AND 105;
```

Expected result:

```text
5
```

The same records should remain without additional duplicate rows.

---

## API Testing

The existing API endpoints can be tested using the known seed IDs.

### Student Tracks

```http
GET /api/Student/101/tracks
```

Expected behavior:

Returns the tracks in which student `101` is enrolled.

### Track Students

```http
GET /api/Track/101/students
```

Expected behavior:

Returns students enrolled in track `101`.

---

## Expected Output

The seeded data should be available through the existing GET endpoints without requiring manual record creation.

For example:

```text
GET /api/Student/101/tracks
```

should return enrollment-related track data for Ahmed Ali.

---

---

## Key Learning

This drill demonstrates how to:

* Use EF Core `HasData`.
* Create repeatable database seed data.
* Work with fixed IDs for predictable testing.
* Respect foreign key relationships during seeding.
* Apply seed data through EF Core migrations.
* Verify seeded records in SQL Server.
* Test seeded records through Swagger.
* Avoid duplicate records when restarting the application.
