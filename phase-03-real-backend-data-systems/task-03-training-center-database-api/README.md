# Task 03 - Training Center Database API

## Overview

This task implements a data-driven Training Center Registration API using ASP.NET Core Web API, Entity Framework Core, and SQL Server.

The API manages:

- Students
- Instructors
- Training Tracks
- Enrollments
- Payments
- Reports

The task focuses on building a real relational backend with EF Core, SQL Server, business rules, DTOs, LINQ queries, and reporting endpoints.

---

## Technologies

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- LINQ
- Swagger / OpenAPI
- Postman

---

## Project Structure

```text
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
Database Model

The database contains five main entities:

Student
Instructor
TrainingTrack
Enrollment
Payment
Relationships
Instructor → TrainingTracks: One-to-Many
Student → Enrollments: One-to-Many
TrainingTrack → Enrollments: One-to-Many
Enrollment → Payments: One-to-Many
Enrollment Relationship

Enrollment represents the relationship between Student and TrainingTrack.

This allows the system to store information specific to each registration, such as:

Enrollment Date
Status
Progress Percentage
Final Result
API Features
Students
Get all students
Get student by ID
Create student
Update student
Soft delete student
Instructors
Get all instructors
Get instructor by ID
Create instructor
Update instructor
Get instructor tracks
Training Tracks
Get all tracks
Filter by keyword
Filter by level
Filter by status
Filter by instructor
Get track details
Create track
Update track
Soft delete track
Enrollments
Get all enrollments
Filter by status
Filter by student
Filter by track
Filter by payment status
Get enrollment details
Create enrollment
Update enrollment status
Get student enrollments
Get track students
Payments
Get all payments
Filter by date range
Filter by payment status
Get payment by ID
Create payment
Update payment status
Get enrollment payments
Reports
Dashboard summary
Unpaid enrollments
Track capacity
Revenue summary
Revenue by track
Business Rules

The application implements several business rules:

Students
Student email must be unique.
Deleted students are excluded from normal operations.
Student deletion is implemented as soft delete.
Instructors
Instructor email must be unique.
Only active instructors can be assigned to training tracks.
Training Tracks
Track code must be unique.
Capacity must be greater than zero.
End date must be after start date.
Track capacity cannot be reduced below the number of active enrollments.
A track with active enrollments cannot be deleted.
Track deletion is implemented as soft delete.
Enrollments
Student must exist and be active.
Training track must exist and be active.
A student cannot enroll in the same track more than once.
A student cannot enroll if the track has reached its capacity.
Enrollment status supports controlled values such as:
Active
Completed
Cancelled
Payments
Payment amount must be greater than zero.
Payment reference number must be unique.
Payment must belong to an existing enrollment.
Payment status supports:
Pending
Paid
Failed
Refunded
EF Core

Entity Framework Core is used for:

Entity modeling
Database relationships
Foreign keys
Unique indexes
Constraints
LINQ queries
Projections
Aggregations
Migrations
SQL Server integration

The API uses DTO projections instead of exposing EF Core entities directly.

DTOs

DTOs are used to control the data exchanged between the API and clients.

Examples include:

CreateStudentRequest
UpdateStudentRequest
StudentListItemResponse
StudentDetailsResponse
CreateInstructorRequest
UpdateInstructorRequest
CreateTrackRequest
UpdateTrackRequest
TrackListItemResponse
TrackDetailsResponse
CreateEnrollmentRequest
EnrollmentDetailsResponse
CreatePaymentRequest
PaymentResponse
Report DTOs

This keeps API responses separated from the database entities.

API Response Format

The API uses a common response wrapper:

{
  "success": true,
  "message": "Student retrieved successfully.",
  "data": {}
}

This provides a consistent response structure across the API.

Validation

Business validation is implemented in the service layer.

Examples include:

Duplicate email validation
Duplicate track code validation
Capacity validation
Date validation
Student and track existence validation
Duplicate enrollment validation
Payment amount validation
Payment reference validation
Payment status validation
Enrollment status validation
LINQ and Queries

The project uses LINQ extensively for querying and reporting data.

Examples include:

Where
Any
FirstOrDefaultAsync
CountAsync
SumAsync
Select
GroupBy

Examples of implemented queries include:

Filtering tracks
Checking whether a student is already enrolled
Checking track capacity
Counting active enrollments
Calculating total paid revenue
Calculating revenue by track
Filtering enrollments by payment status
Reports and Aggregations

The reporting layer provides business-level information using EF Core and LINQ.

Dashboard Summary

Returns:

Total students
Active students
Total instructors
Total tracks
Active tracks
Total enrollments
Active enrollments
Total revenue
Track Capacity

Returns:

Track
Capacity
Enrolled students
Available seats
Whether the track is full
Revenue Summary

Returns:

Total revenue
Total payments
Paid payments
Revenue By Track

Returns:

Track
Total paid revenue
Number of paid payments
Unpaid Enrollments

Returns enrollments based on their payment information.

Important Payment Report Assumption

The current TrainingTrack entity does not contain a course fee.

Because of this, the system cannot calculate:

Remaining amount
Exact partially paid amount
Fully paid status based on a required fee

For the current implementation:

No paid payment → Unpaid
At least one paid payment → Partially Paid

Revenue is calculated by summing payments where:

PaymentStatus = Paid
Database Setup

Example connection string:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=TechMasterTrainingCenterDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
Create Migration
dotnet ef migrations add InitialCreate
Update Database
dotnet ef database update
Running the Project

Restore dependencies:

dotnet restore

Build the project:

dotnet build

Run the API:

dotnet run

Open Swagger:

https://localhost:<port>/swagger
Testing

The API was tested using:

Swagger
Postman
SQL Server Management Studio

Test data was added to:

Students
Instructors
Training Tracks
Enrollments
Payments

The test data covers different scenarios such as:

Active students
Completed enrollments
Paid payments
Pending payments
Unpaid enrollments
Available track capacity
Revenue calculations
Testing Scenarios

The API can be tested for:

CRUD Operations
Create
Read
Update
Delete
Filtering
Track keyword
Track level
Track status
Instructor
Enrollment status
Payment status
Payment date range
Business Rules
Duplicate student email
Duplicate instructor email
Duplicate track code
Duplicate enrollment
Full track enrollment
Invalid track dates
Invalid capacity
Invalid payment amount
Invalid payment status
Invalid enrollment status
Reports
Dashboard summary
Unpaid enrollments
Track capacity
Revenue summary
Revenue by track
Soft Delete

Soft delete is used instead of permanently removing certain records.

For example, a Student can be marked as:

IsDeleted = true

and the deletion time is stored in:

DeletedAt

Training Tracks also support soft delete through:

IsDeleted

This preserves historical data while preventing deleted records from appearing in normal operations.

Architecture

The project follows a simple layered architecture:

Controller
    ↓
Service
    ↓
DbContext
    ↓
SQL Server
Controllers

Controllers handle:

HTTP requests
HTTP responses
Status codes
Routing
Services

Services contain:

Business rules
Validation
LINQ queries
Data processing
DbContext

The DbContext handles:

Entity configuration
Relationships
Database communication
EF Core operations
Key Learning Outcomes

This task demonstrates practical backend development skills including:

Designing relational databases
Modeling relationships with EF Core
Creating SQL Server databases using migrations
Building RESTful APIs
Using DTOs
Implementing service layers
Writing LINQ queries
Using projections
Applying business rules
Implementing soft delete
Filtering and reporting data
Handling database relationships
Working with aggregations
Testing APIs using Swagger and Postman
Debugging EF Core and SQL Server queries
Task Status

Completed

The Training Center API includes:

Entity modeling
SQL Server database
EF Core configuration
Migrations
CRUD operations
DTOs
Service layer
Business validation
Filtering
Enrollments
Payments
Reports
Soft delete
Swagger testing
Postman testing
