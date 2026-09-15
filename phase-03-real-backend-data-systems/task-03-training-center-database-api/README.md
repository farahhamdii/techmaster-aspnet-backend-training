# Task 03 - Training Center Database API 🎓

An enterprise-grade RESTful API built with ASP.NET Core, Entity Framework Core, and SQL Server. This project manages training center operations—including students, instructors, training tracks, enrollments, payments, and analytical business reports—leveraging clean layered architecture, robust business validation, DTO projections, soft deletes, and complex LINQ aggregations.

## 📋 Table of Contents

- [Overview](#-overview)
- [Architecture & Layers](#-architecture--layers)
- [Technologies & Tools](#-technologies--tools)
- [Database Model & ERD](#-database-model--erd)
- [Key Features](#-key-features)
- [Business Rules & Validation](#-business-rules--validation)
- [Reporting & Aggregations](#-reporting--aggregations)
- [API Response Structure](#-api-response-structure)
- [Getting Started & Installation](#-getting-started--installation)
- [Database Setup & Migrations](#-database-setup--migrations)
- [Testing & Verification](#-testing--verification)

## 🎯 Overview

The Training Center API handles end-to-end workflows for an IT educational institute:

- **Student & Instructor Management**: Full lifecycle management with duplicate prevention and soft-delete capabilities.
- **Track Administration**: Capacity tracking, status filters, and schedule validation.
- **Enrollments**: Safe registration system preventing double-booking and capacity overruns.
- **Payment Tracking**: Multi-status payment logging linked directly to enrollments.
- **Business Intelligence & Reports**: Aggregated financial metrics, seat availability, and unpaid enrollment metrics using optimized LINQ queries.

## 🏗 Architecture & Layers

The project strictly separates concerns using a clear 4-tier design pattern:

```
               ┌──────────────────────────────┐
               │         Client / UI          │
               │   (Swagger / Postman App)    │
               └──────────────┬───────────────┘
                              │ HTTP Requests
                              ▼
               ┌──────────────────────────────┐
               │      Controllers Layer       │
               │  (Routing & HTTP Responses)  │
               └──────────────┬───────────────┘
                              │ DTOs
                              ▼
               ┌──────────────────────────────┐
               │        Services Layer        │
               │  (Business Rules & Logic)    │
               └──────────────┬───────────────┘
                              │ LINQ Projections
                              ▼
               ┌──────────────────────────────┐
               │  Data / DbContext (EF Core)  │
               │    (ORM & Entity Mappings)   │
               └──────────────┬───────────────┘
                              │ ADO.NET / SQL
                              ▼
               ┌──────────────────────────────┐
               │     SQL Server Database      │
               └──────────────────────────────┘
```

### Folder Structure

```
TrainingCenter/
│
├── Controllers/
│   ├── StudentsController.cs
│   ├── InstructorsController.cs
│   ├── TrainingTracksController.cs
│   ├── EnrollmentsController.cs
│   ├── PaymentsController.cs
│   └── ReportsController.cs
│
├── Data/
│   └── TrainingCenterDbContext.cs
│
├── Entities/
│   ├── Student.cs
│   ├── Instructor.cs
│   ├── TrainingTrack.cs
│   ├── Enrollment.cs
│   └── Payment.cs
│
├── DTOs/
│   ├── Student DTOs
│   ├── Instructor DTOs
│   ├── Training Track DTOs
│   ├── Enrollment DTOs
│   ├── Payment DTOs
│   └── Report DTOs
│
├── Services/
│   ├── StudentService.cs
│   ├── InstructorService.cs
│   ├── TrainingTrackService.cs
│   ├── EnrollmentService.cs
│   ├── PaymentService.cs
│   └── ReportService.cs
│
├── Common/
│   └── ApiResponse.cs
│
└── Migrations/
```

## 🛠 Technologies & Tools

- **Language**: C# (.NET 8/9)
- **Framework**: ASP.NET Core Web API
- **ORM**: Entity Framework Core
- **Database**: Microsoft SQL Server
- **Query Language**: LINQ (Language Integrated Query)
- **API Documentation**: Swagger / OpenAPI
- **API Testing**: Postman & Swagger UI

## 🗄 Database Model & ERD

The relational schema consists of 5 core domain entities designed with strict foreign key constraints, indexes, and deletion behaviors:

```
┌──────────────┐       1 : N       ┌──────────────────┐       1 : N       ┌────────────────┐
│  Instructor  │ ────────────────> │  TrainingTrack   │ ────────────────> │   Enrollment   │
└──────────────┘                   └──────────────────┘                   └───────┬────────┘
                                                                                    │
                                                                             1 : N  │  N : 1
                                                                                    │
┌──────────────┐                          ┌──────────────────┐                     │
│   Payment    │ <─────────────────────── │    Student       │ <───────────────────┘
└──────────────┘          1 : N           └──────────────────┘
```

### Entity Relationships Summary

| Primary Entity | Target Entity | Cardinality | Key Relationship Description |
|---|---|---|---|
| Instructor | TrainingTrack | 1 : N | An instructor can mentor multiple training tracks. |
| Student | Enrollment | 1 : N | A student can hold multiple course enrollments. |
| TrainingTrack | Enrollment | 1 : N | A track can host multiple student enrollments. |
| Enrollment | Payment | 1 : N | An enrollment can have multiple payment records. |

## ✨ Key Features

### 👤 Students
- Fetch paginated/filtered lists of students.
- Fetch detailed student history (enrolled tracks, payment records).
- Create & update student profiles.
- **Soft Deletion**: Marks `IsDeleted = true` and updates `DeletedAt` timestamp without destroying historical audit data.

### 👨‍🏫 Instructors
- Instructor management with active status flags.
- Fetch all tracks assigned to a specific instructor.
- Email uniqueness validation.

### 📚 Training Tracks
- Track catalog management filtered by Keyword, Level, Status, and Instructor.
- Automatic calculation of available seats vs maximum capacity.
- Schedule constraints enforcement (Start Date must precede End Date).

### 📝 Enrollments
- Multi-criteria filtering by Status, Student ID, Track ID, and Payment Status.
- Duplicate registration checks per student-track pair.
- Controlled status transitions (Active, Completed, Cancelled).

### 💳 Payments
- Multi-status financial transaction logging (Pending, Paid, Failed, Refunded).
- Unique reference tracking per transaction.
- Date-range filtering for revenue auditing.

## 🛡 Business Rules & Validation

Business logic validation takes place inside the dedicated Service Layer:

- **Email Uniqueness**: Both Student and Instructor email addresses must be unique across the system.
- **Active Status Mandates**: Only active instructors can be assigned to new training tracks.
- **Track Capacity Limits**:
  - Track capacity must be greater than zero.
  - Track capacity cannot be reduced below the count of active enrollments.
  - Registrations are blocked automatically if Enrolled Students == Capacity.
- **Enrollment Uniqueness**: A student cannot enroll in the same training track more than once.
- **Track Schedule Rules**: Track `EndDate` must strictly occur after `StartDate`.
- **Safe Deletion Shield**: A track with active student enrollments cannot be deleted.
- **Financial Integrity**: Payment amounts must be strictly greater than $0.00.

## 📊 Reporting & Aggregations

The `ReportService` leverages EF Core LINQ projections (`Select`, `GroupBy`, `SumAsync`, `CountAsync`) to construct high-level business analytics:

- **Dashboard Summary**:
  - Total & Active Student count
  - Total & Active Tracks
  - Total Enrollments & Active registrations
  - Cumulative paid revenue
- **Track Capacity Report**:
  - Total Capacity vs Enrolled Seats per track.
  - Dynamic calculation of `AvailableSeats` and boolean `IsFull`.
- **Revenue Analytics**:
  - Revenue broken down by Track (Total Revenue, Number of Paid Transactions).
  - Revenue summary filtered by custom date ranges.
- **Unpaid Enrollments Tracking**:
  - Groups enrollments by payment completion status (Unpaid vs Partially Paid vs Fully Paid).

### Payment Reporting Assumption

Since `TrainingTrack` does not currently define a static course fee, payment status classification is determined as follows:

- No Paid Payments: **Unpaid**
- ≥ 1 Paid Payment: **Partially Paid / Active**
- Revenue calculations aggregate only transactions where `PaymentStatus == Paid`.

## 📩 API Response Structure

All endpoints return a uniform REST response wrapper `ApiResponse<T>`:

```json
{
  "success": true,
  "message": "Student retrieved successfully.",
  "data": {
    "id": 1,
    "fullName": "Student Name",
    "email": "student@example.com",
    "isActive": true
  }
}
```

## 🚀 Getting Started & Installation

### Prerequisites

- .NET 8.0 SDK or higher
- SQL Server Express / LocalDB / Enterprise
- Visual Studio 2022 OR VS Code

### Step-by-Step Setup

**1. Clone the Repository**

```bash
git clone https://github.com/your-username/TrainingCenter.git
cd TrainingCenter
```

**2. Configure Database Connection**

Update `appsettings.json` with your local SQL Server instance connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=TechMasterTrainingCenterDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**3. Restore Dependencies**

```bash
dotnet restore
```

**4. Build Solution**

```bash
dotnet build
```

## 🗄 Database Setup & Migrations

Execute EF Core migrations to automatically generate the database schema:

```bash
# Add a new migration (if modifying entities)
dotnet ef migrations add InitialCreate

# Apply migrations to SQL Server
dotnet ef database update
```

## 🧪 Testing & Verification

**1. Run the API Application**

```bash
dotnet run
```

**2. Access Interactive Swagger UI**

Open your browser and navigate to:

```
https://localhost:<port>/swagger
```

**3. Postman Collection**

Import the API routes into Postman to test:
- CRUD flow for Students, Instructors, and Tracks.
- Validation triggers (e.g., duplicate email creation attempt).
- Report generation metrics under `/api/reports/dashboard`.

---

## 🏁 Task Status: Completed ✅

All core requirements, entity relationships, business validation rules, soft deletion mechanics, LINQ aggregations, and OpenAPI documentation have been fully implemented and verified.
