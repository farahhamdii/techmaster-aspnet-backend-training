# Drill 04 - Many-to-Many Enrollment

## Concept

This drill demonstrates how to model a **Many-to-Many relationship** using an explicit join entity in Entity Framework Core.

The relationship connects `Student` and `Track` through an `Enrollment` entity.

```text
Student * ─────── * Track
        \         /
         Enrollment
```

A student can enroll in multiple tracks, and a track can contain multiple students.

---

## Scenario

Extend the existing EF Core model by creating an `Enrollment` join entity between `Student` and `Track`.

The join entity represents the enrollment itself and stores the relationship between a student and a track.

Using an explicit join entity also allows additional information about the enrollment to be stored, such as the enrollment date and status.

---

## Requirements

* Create an `Enrollment` entity.
* Add a primary key to the enrollment.
* Add `StudentId` as a foreign key.
* Add `TrackId` as a foreign key.
* Add navigation properties to `Student` and `Track`.
* Add enrollment-specific information.
* Add `DbSet<Enrollment>` to `AppDbContext`.
* Configure the relationships using Fluent API.
* Configure the foreign keys.
* Create a migration.
* Apply the migration to SQL Server.
* Verify the `Enrollments` table.
* Verify both foreign key relationships.

---

## Project Structure

```text
EFCoreModelingDrills/
├── Data/
│   └── AppDbContext.cs
├── Entities/
│   ├── Student.cs
│   ├── StudentProfile.cs
│   ├── Instructor.cs
│   ├── Track.cs
│   └── Enrollment.cs
├── Configurations/
│   ├── StudentConfiguration.cs
│   ├── InstructorConfiguration.cs
│   └── EnrollmentConfiguration.cs
├── Migrations/
├── Program.cs
├── appsettings.json
└── EFCoreModelingDrills.csproj
```

---

## Enrollment Entity

The `Enrollment` entity acts as the join entity between `Student` and `Track`.

| Property       | Type     | Purpose                     |
| -------------- | -------- | --------------------------- |
| Id             | int      | Primary key                 |
| StudentId      | int      | Foreign key to Student      |
| TrackId        | int      | Foreign key to Track        |
| EnrollmentDate | DateTime | Date of enrollment          |
| Status         | string   | Enrollment status           |
| Student        | Student  | Student navigation property |
| Track          | Track    | Track navigation property   |

---

## Why Use a Join Entity?

Instead of connecting `Student` and `Track` directly, the relationship is represented through:

```text
Student
   │
   ▼
Enrollment
   ▲
   │
Track
```

This approach is useful because the relationship itself can contain additional data.

For example:

```text
Enrollment
├── StudentId
├── TrackId
├── EnrollmentDate
└── Status
```

The enrollment date and status belong to the enrollment relationship, not directly to the student or track.

---

## Relationship

The final relationship is:

```text
Student 1 ───── * Enrollment * ───── 1 Track
```

This produces a Many-to-Many relationship:

```text
Student 1 ─── Enrollment ─── Track 1
Student 1 ─── Enrollment ─── Track 2
Student 2 ─── Enrollment ─── Track 1
Student 3 ─── Enrollment ─── Track 2
```

Therefore:

```text
One Student → Many Tracks
One Track   → Many Students
```

---

## Navigation Properties

The `Enrollment` entity references both sides:

```csharp
public Student Student { get; set; } = null!;

public Track Track { get; set; } = null!;
```

The `Student` entity can contain:

```csharp
public ICollection<Enrollment> Enrollments { get; set; }
    = new List<Enrollment>();
```

The `Track` entity can contain:

```csharp
public ICollection<Enrollment> Enrollments { get; set; }
    = new List<Enrollment>();
```

---

## DbContext

Add the enrollment DbSet:

```csharp
public DbSet<Enrollment> Enrollments { get; set; }
```

This allows EF Core to work with enrollment records.

---

## Fluent API Configuration

The join entity is configured using Fluent API.

```csharp
builder.HasOne(e => e.Student)
       .WithMany(s => s.Enrollments)
       .HasForeignKey(e => e.StudentId)
       .OnDelete(DeleteBehavior.Cascade);

builder.HasOne(e => e.Track)
       .WithMany(t => t.Enrollments)
       .HasForeignKey(e => e.TrackId)
       .OnDelete(DeleteBehavior.Cascade);
```

This creates two One-to-Many relationships:

```text
Student
   │
   │ 1 : Many
   ▼
Enrollment
   ▲
   │ Many : 1
   │
Track
```

Together, these relationships form a Many-to-Many relationship between Student and Track.

---

## Foreign Keys

The `Enrollments` table contains two foreign keys:

```text
Enrollments.StudentId
        ↓
Students.Id
```

and:

```text
Enrollments.TrackId
        ↓
Tracks.Id
```

The enrollment record therefore identifies both the student and the track.

---

## Optional Unique Constraint

To prevent the same student from being enrolled in the same track more than once, a unique composite index can be configured:

```csharp
builder.HasIndex(e => new { e.StudentId, e.TrackId })
       .IsUnique();
```

This ensures:

```text
Student 1 + Track 1
```

cannot be inserted twice.

---

## Migration

Create the migration:

```bash
dotnet ef migrations add AddEnrollmentManyToMany
```

The migration creates the `Enrollments` table and its foreign key relationships.

---

## Database Update

Apply the migration:

```bash
dotnet ef database update
```

The database is updated with the new enrollment structure.

---

## Expected Database Structure

```text
TechMasterTrainingCenterDb
│
├── Students
├── StudentProfiles
├── Instructors
├── Tracks
└── Enrollments
```

The `Enrollments` table should contain:

```text
Enrollments
├── Id
├── StudentId (FK)
├── TrackId (FK)
├── EnrollmentDate
└── Status
```

---

## Relationship Diagram

```text
┌─────────────────┐
│    Students     │
├─────────────────┤
│ PK Id           │
│ FullName        │
│ Email           │
└────────┬────────┘
         │
         │ 1 : Many
         ▼
┌─────────────────────┐
│     Enrollments     │
├─────────────────────┤
│ PK Id               │
│ FK StudentId        │
│ FK TrackId          │
│ EnrollmentDate      │
│ Status              │
└──────────┬──────────┘
           │
           │ Many : 1
           ▼
┌─────────────────┐
│      Tracks     │
├─────────────────┤
│ PK Id           │
│ Name            │
│ InstructorId    │
└─────────────────┘
```

---

## Verification

The following checks were performed:

1. `Enrollment` entity was created.
2. `StudentId` and `TrackId` were added.
3. Navigation properties were added.
4. Enrollment-specific fields were added.
5. `Enrollments` was added to `AppDbContext`.
6. Student-to-Enrollment relationship was configured.
7. Track-to-Enrollment relationship was configured.
8. The migration `AddEnrollmentManyToMany` was created.
9. The migration was applied successfully.
10. The `Enrollments` table exists.
11. Both foreign keys exist.
12. The Student-Track Many-to-Many relationship works through the join entity.

---

## Test Cases

| Test Case                 | Expected Result                           | Status |
| ------------------------- | ----------------------------------------- | ------ |
| Enrollment entity         | Entity created successfully               | Passed |
| StudentId                 | Foreign key to Student                    | Passed |
| TrackId                   | Foreign key to Track                      | Passed |
| Navigation properties     | Both relationships are represented        | Passed |
| DbSet                     | Enrollments is registered                 | Passed |
| Many-to-Many relationship | Students can enroll in multiple tracks    | Passed |
| Migration                 | AddEnrollmentManyToMany exists            | Passed |
| Database update           | Enrollments table is created              | Passed |
| Foreign keys              | Both references are configured            | Passed |
| Duplicate enrollment      | Prevented when unique index is configured | Passed |

---


---

## Result

Drill 04 successfully demonstrates how to model a **Many-to-Many relationship using an explicit join entity** in Entity Framework Core.

The completed workflow is:

```text
Student
   ↓
Enrollment Join Entity
   ↑
Track
   ↓
Fluent API Configuration
   ↓
Migration
   ↓
Database Update
   ↓
Many-to-Many Relationship
```

The project is now ready to continue with **Drill 05 - One-to-One PaymentSummary**.
