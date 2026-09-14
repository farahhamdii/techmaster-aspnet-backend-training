# Drill 01 - DbContext & First Migration

## Concept

This drill introduces the basic EF Core database workflow using `DbContext`, `DbSet`, SQL Server, and EF Core Migrations.

The goal is to prove that a simple C# entity can be mapped to a real SQL Server database table.

---

## Scenario

Create the first EF Core project workspace and prove that a simple `Student` entity can generate a real database table.

---

## Requirements

* Create a `Student` entity.
* Add `Id`, `FullName`, `Email`, `CreatedAt`, and `IsActive`.
* Create `AppDbContext`.
* Add `DbSet<Student>`.
* Configure a SQL Server connection string.
* Register `AppDbContext` in the application.
* Create a migration named `InitialStudentSchema`.
* Apply the migration to SQL Server.
* Verify that the `Students` table exists.
* Run the API successfully with Swagger.

---

## Project Structure

```text
EFCoreModelingDrills/
├── Data/
│   └── AppDbContext.cs
├── Entities/
│   └── Student.cs
├── Migrations/
├── Program.cs
├── appsettings.json
└── EFCoreModelingDrills.csproj
```

---

## Student Entity

The `Student` entity contains:

| Property  | Type     | Purpose               |
| --------- | -------- | --------------------- |
| Id        | int      | Primary key           |
| FullName  | string   | Student full name     |
| Email     | string   | Student email         |
| CreatedAt | DateTime | Creation timestamp    |
| IsActive  | bool     | Student active status |

---

## DbContext

`AppDbContext` inherits from EF Core's `DbContext`.

It represents the session between the application and the database and is responsible for querying and saving entity data.

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
}
```

### DbSet

`DbSet<Student>` represents the collection of `Student` entities that EF Core maps to the `Students` database table.

The basic mapping is:

```text
Student Entity
      ↓
DbSet<Student>
      ↓
EF Core
      ↓
Students Table
```

---

## EF Core Configuration

The SQL Server provider is configured through the application's connection string.

`AppDbContext` is registered using dependency injection:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
```

The connection string is stored in local development configuration and production credentials are not committed to the repository.

---

## Migration

The following command was used to create the first migration:

```bash
dotnet ef migrations add InitialStudentSchema
```

The migration represents the database schema generated from the current EF Core model.

---

## Database Update

The migration was applied using:

```bash
dotnet ef database update
```

This created the `TechMasterTrainingCenterDb` database and the `Students` table in SQL Server.

---

## Verification

The generated database table contains the expected columns:

```text
Students
├── Id
├── FullName
├── Email
├── CreatedAt
└── IsActive
```

The API was also started successfully and Swagger loaded without DbContext or database configuration errors.

---

## Test Cases

| Test Case        | Expected Result                                        | Status |
| ---------------- | ------------------------------------------------------ | ------ |
| Migration exists | `InitialStudentSchema` exists in the Migrations folder | Passed |
| Database update  | `Students` table exists in SQL Server                  | Passed |
| Swagger running  | API starts without DbContext errors                    | Passed |

---


## Result

Drill 01 successfully demonstrates the basic EF Core workflow:

```text
Entity
  ↓
DbContext / DbSet
  ↓
Migration
  ↓
Database Update
  ↓
SQL Server Table
```

The project is now ready to continue with the next EF Core modeling drill.
