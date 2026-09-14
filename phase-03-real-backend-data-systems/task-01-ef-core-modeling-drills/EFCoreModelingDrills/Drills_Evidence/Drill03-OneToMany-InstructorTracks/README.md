# Drill 03 - One-to-Many InstructorTracks

## Concept

This drill demonstrates how to model and configure a **One-to-Many relationship** using Entity Framework Core.

The relationship connects an `Instructor` with multiple `Tracks`.

```text
Instructor 1 ───────── * Tracks
```

One instructor can teach multiple tracks, while each track belongs to one instructor.

---

## Scenario

Extend the EF Core model by creating `Instructor` and `Track` entities and configuring a One-to-Many relationship between them.

The relationship must be explicitly configured using EF Core Fluent API.

---

## Requirements

* Create an `Instructor` entity.
* Create a `Track` entity.
* Add an instructor primary key.
* Add a track primary key.
* Add a foreign key from `Track` to `Instructor`.
* Add navigation properties on both entities.
* Add `DbSet<Instructor>` and `DbSet<Track>` to `AppDbContext`.
* Configure the One-to-Many relationship using Fluent API.
* Create a migration.
* Apply the migration to SQL Server.
* Verify that both tables exist.
* Verify the foreign key relationship.

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
│   └── Track.cs
├── Configurations/
│   ├── StudentConfiguration.cs
│   └── InstructorConfiguration.cs
├── Migrations/
├── Program.cs
├── appsettings.json
└── EFCoreModelingDrills.csproj
```

---

## Instructor Entity

The `Instructor` entity represents an instructor who can be responsible for multiple tracks.

| Property | Type               | Purpose                         |
| -------- | ------------------ | ------------------------------- |
| Id       | int                | Primary key                     |
| FullName | string             | Instructor name                 |
| Tracks   | ICollection<Track> | Tracks taught by the instructor |

---

## Track Entity

The `Track` entity represents a training track.

| Property     | Type       | Purpose             |
| ------------ | ---------- | ------------------- |
| Id           | int        | Primary key         |
| Name         | string     | Track name          |
| InstructorId | int        | Foreign key         |
| Instructor   | Instructor | Navigation property |

---

## Navigation Properties

The `Instructor` entity contains a collection of tracks:

```csharp
public ICollection<Track> Tracks { get; set; } = new List<Track>();
```

The `Track` entity contains a reference to its instructor:

```csharp
public Instructor Instructor { get; set; } = null!;
```

This creates the following relationship:

```text
Instructor
    │
    │ Tracks
    ▼
Track
    │
    │ Instructor
    ▼
Instructor
```

---

## One-to-Many Configuration

The relationship is configured using Fluent API:

```csharp
builder.HasOne(i => i.Instructor)
       .WithMany(i => i.Tracks)
       .HasForeignKey(i => i.InstructorId)
       .OnDelete(DeleteBehavior.Restrict);
```

The relationship means:

```text
Instructor 1
     │
     ├──── Track 1
     ├──── Track 2
     └──── Track 3
```

Each track has exactly one instructor.

---

## Foreign Key

The foreign key is stored in the `Tracks` table:

```text
Tracks.InstructorId
        ↓
Instructors.Id
```

This establishes the database relationship between the two tables.

---

## Delete Behavior

The relationship uses:

```csharp
.OnDelete(DeleteBehavior.Restrict);
```

This prevents deleting an instructor while tracks are still associated with that instructor.

```text
Instructor
    │
    ├── Track 1
    └── Track 2

Delete Instructor
       ↓
Blocked while related Tracks exist
```

This protects the related training data from accidental deletion.

---

## DbContext

The following DbSets are added to `AppDbContext`:

```csharp
public DbSet<Instructor> Instructors { get; set; }

public DbSet<Track> Tracks { get; set; }
```

The existing configuration loading mechanism is used:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(
        typeof(AppDbContext).Assembly);

    base.OnModelCreating(modelBuilder);
}
```

---

## Migration

After creating the entities and configuring the relationship, create the migration:

```bash
dotnet ef migrations add AddInstructorTracks
```

The migration records the schema changes required for the `Instructors` and `Tracks` tables.

---

## Database Update

Apply the migration:

```bash
dotnet ef database update
```

This creates the required tables and foreign key relationship in SQL Server.

---

## Expected Database Structure

```text
TechMasterTrainingCenterDb
│
├── Instructors
│
├── Tracks
│
├── Students
│
└── StudentProfiles
```

The `Tracks` table should contain:

```text
Tracks
├── Id
├── Name
└── InstructorId (FK)
```

---

## Relationship Diagram

```text
┌─────────────────────┐
│     Instructors     │
├─────────────────────┤
│ PK Id               │
│ FullName            │
└──────────┬──────────┘
           │
           │ 1 : Many
           │
           ▼
┌─────────────────────┐
│       Tracks        │
├─────────────────────┤
│ PK Id               │
│ Name                │
│ FK InstructorId     │
└─────────────────────┘
```

---

## Verification

The following checks were performed:

1. `Instructor` entity was created.
2. `Track` entity was created.
3. Instructor-to-track navigation properties were added.
4. `Instructors` and `Tracks` DbSets were added.
5. The One-to-Many relationship was configured.
6. `InstructorId` was configured as the foreign key.
7. The `AddInstructorTracks` migration was created.
8. The migration was applied successfully.
9. Both database tables exist.
10. The foreign key connects `Tracks.InstructorId` to `Instructors.Id`.

---

## Test Cases

| Test Case                 | Expected Result                           | Status |
| ------------------------- | ----------------------------------------- | ------ |
| Instructor entity         | Entity created successfully               | Passed |
| Track entity              | Entity created successfully               | Passed |
| Navigation properties     | Relationship represented in both entities | Passed |
| DbSets                    | Instructors and Tracks are registered     | Passed |
| One-to-Many configuration | One instructor can have many tracks       | Passed |
| Foreign key               | Track references Instructor               | Passed |
| Migration                 | AddInstructorTracks exists                | Passed |
| Database update           | Both tables are created                   | Passed |
| Delete behavior           | Related instructor deletion is restricted | Passed |

---

## Evidence

### 1. Instructor and Track Entities

![Instructor and Track Entities](evidence/01-instructor-track-entities.png)

### 2. One-to-Many Configuration

![One to Many Configuration](evidence/02-one-to-many-configuration.png)

### 3. Migration

![Migration](evidence/03-migration.png)

### 4. Database Tables

![Database Tables](evidence/04-database-tables.png)

### 5. Foreign Key Relationship

![Foreign Key](evidence/05-foreign-key.png)

---

## Result

Drill 03 successfully demonstrates how to create a **One-to-Many relationship** using Entity Framework Core.

The completed workflow is:

```text
Instructor Entity
      ↓
Track Entity
      ↓
Navigation Properties
      ↓
Fluent API Configuration
      ↓
Migration
      ↓
Database Update
      ↓
One Instructor → Many Tracks
```

The project is now ready for **Drill 04 - Many-to-Many Enrollment**.
