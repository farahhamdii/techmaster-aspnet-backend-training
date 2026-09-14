# Drill 02 - One-to-One StudentProfile

## Concept

This drill demonstrates how to model and configure a **One-to-One relationship** using Entity Framework Core.

The relationship connects a `Student` with one `StudentProfile`.

```text
Student 1 ───────── 1 StudentProfile
```

Each student can have one profile, and each profile belongs to one student.

---

## Scenario

Extend the existing EF Core model by creating a `StudentProfile` entity and connecting it to the existing `Student` entity using a One-to-One relationship.

The relationship must be explicitly configured using EF Core Fluent API.

---

## Requirements

* Create a `StudentProfile` entity.
* Add profile information such as:

  * `Id`
  * `PhoneNumber`
  * `Address`
  * `DateOfBirth`
  * `StudentId`
* Add a navigation property from `Student` to `StudentProfile`.
* Add a navigation property from `StudentProfile` to `Student`.
* Add `StudentProfiles` to `AppDbContext`.
* Configure the One-to-One relationship using Fluent API.
* Define `StudentId` as the foreign key.
* Create a new migration.
* Apply the migration to SQL Server.
* Verify that the `StudentProfiles` table exists.
* Verify the foreign key relationship between `Students` and `StudentProfiles`.

---

## Project Structure

```text
EFCoreModelingDrills/
├── Data/
│   └── AppDbContext.cs
├── Entities/
│   ├── Student.cs
│   └── StudentProfile.cs
├── Configurations/
│   └── StudentConfiguration.cs
├── Migrations/
├── Program.cs
├── appsettings.json
└── EFCoreModelingDrills.csproj
```

---

## StudentProfile Entity

The `StudentProfile` entity contains additional information related to a student.

| Property    | Type     | Purpose                |
| ----------- | -------- | ---------------------- |
| Id          | int      | Primary key            |
| PhoneNumber | string   | Student phone number   |
| Address     | string   | Student address        |
| DateOfBirth | DateTime | Student date of birth  |
| StudentId   | int      | Foreign key to Student |
| Student     | Student  | Navigation property    |

---

## Navigation Properties

The `Student` entity contains a navigation property for its profile:

```csharp
public StudentProfile? Profile { get; set; }
```

The `StudentProfile` entity contains a navigation property back to the student:

```csharp
public Student Student { get; set; } = null!;
```

This creates navigation in both directions:

```text
Student
   │
   │ Profile
   ▼
StudentProfile
   │
   │ Student
   ▼
Student
```

---

## DbContext

The `AppDbContext` exposes the `StudentProfile` entity through a `DbSet`.

```csharp
public DbSet<StudentProfile> StudentProfiles { get; set; }
```

The context also loads entity configurations from the assembly:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(
        typeof(AppDbContext).Assembly);

    base.OnModelCreating(modelBuilder);
}
```

---

## One-to-One Configuration

The relationship is configured using EF Core Fluent API.

```csharp
builder.HasOne(s => s.Profile)
       .WithOne(p => p.Student)
       .HasForeignKey<StudentProfile>(p => p.StudentId)
       .OnDelete(DeleteBehavior.Cascade);
```

### Relationship Explanation

```text
Student
   │
   │ HasOne
   ▼
StudentProfile
   │
   │ WithOne
   ▼
Student
```

### Foreign Key

The foreign key is stored in the `StudentProfile` table:

```text
StudentProfile.StudentId
        ↓
Students.Id
```

This means each profile references its associated student.

---

## Delete Behavior

The relationship uses:

```csharp
.OnDelete(DeleteBehavior.Cascade);
```

This means that when a student is deleted, the related student profile is also deleted.

```text
Delete Student
      ↓
Delete StudentProfile
```

---

## Migration

After adding the `StudentProfile` entity and configuring the relationship, a new migration is created:

```bash
dotnet ef migrations add AddStudentProfile
```

The migration records the schema changes required to add the new entity and its relationship.

---

## Database Update

The migration is applied to SQL Server using:

```bash
dotnet ef database update
```

This updates the existing database with the new `StudentProfiles` table and its foreign key relationship.

---

## Expected Database Structure

After applying the migration, the database should contain:

```text
TechMasterTrainingCenterDb
│
├── Students
│
└── StudentProfiles
```

The `StudentProfiles` table should contain:

```text
StudentProfiles
├── Id
├── PhoneNumber
├── Address
├── DateOfBirth
└── StudentId (FK)
```

---

## Relationship Diagram

```text
┌─────────────────────┐
│      Students       │
├─────────────────────┤
│ PK Id               │
│ FullName            │
│ Email               │
│ CreatedAt           │
│ IsActive            │
└──────────┬──────────┘
           │
           │ 1 : 1
           │
           ▼
┌─────────────────────┐
│   StudentProfiles   │
├─────────────────────┤
│ PK Id               │
│ PhoneNumber         │
│ Address             │
│ DateOfBirth         │
│ FK StudentId        │
└─────────────────────┘
```

---

## Verification

The following checks were performed:

1. `StudentProfile` entity was created successfully.
2. `Student` contains the profile navigation property.
3. `StudentProfile` contains the student navigation property.
4. `StudentProfiles` was added to `AppDbContext`.
5. The One-to-One relationship was configured using Fluent API.
6. `StudentId` was configured as the foreign key.
7. The migration `AddStudentProfile` was created successfully.
8. The migration was applied successfully.
9. The `StudentProfiles` table exists in SQL Server.
10. The foreign key connects `StudentProfiles.StudentId` to `Students.Id`.

---

## Test Cases

| Test Case                | Expected Result                                 | Status |
| ------------------------ | ----------------------------------------------- | ------ |
| StudentProfile entity    | Entity exists with required properties          | Passed |
| Navigation properties    | Student and StudentProfile reference each other | Passed |
| DbSet                    | `StudentProfiles` exists in AppDbContext        | Passed |
| One-to-One configuration | Relationship is configured correctly            | Passed |
| Migration                | `AddStudentProfile` migration exists            | Passed |
| Database update          | StudentProfiles table is created                | Passed |
| Foreign key              | StudentId references Students.Id                | Passed |
| Delete behavior          | Cascade delete is configured                    | Passed |

---

Shows the foreign key relationship between:

```text
StudentProfiles.StudentId
          ↓
Students.Id
```

---

## Result

Drill 02 successfully demonstrates how to create and configure a **One-to-One relationship** in Entity Framework Core.

The completed workflow is:

```text
Student Entity
      ↓
StudentProfile Entity
      ↓
Navigation Properties
      ↓
Fluent API Configuration
      ↓
EF Core Migration
      ↓
Database Update
      ↓
One-to-One SQL Relationship
```

The project is now ready to continue with **Drill 03 - One-to-Many InstructorTracks**.
