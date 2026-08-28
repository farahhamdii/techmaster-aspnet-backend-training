# 🎓 Student Management API (Task 02)

A clean, in-memory RESTful CRUD API built with **ASP.NET Core** simulating a training center system to manage students, tracks, and stats.

---

## 📌 Architecture & Design Guidelines

This project follows the **N-Tier/Clean Architecture pattern** to keep code maintainable, scalable, and fully decoupled:

- **Controllers Layer (`Controllers/`)**: Handles incoming HTTP requests, route mapping, and returns standard HTTP status codes (`200 OK`, `201 Created`, `400 Bad Request`, `404 Not Found`).
- **Services Layer (`Services/`)**: Contains all core business logic, thread-safe in-memory state management, data transformations, and filtering.
- **DTOs (`DTOs/`)**: Enforces input validation using `DataAnnotations` and shapes outgoing responses to hide internal Domain Model details.
- **Domain Models (`Models/`)**: Represents internal data structures (`Student`).

---

## 📁 Project Structure

```text
task-02-student-management-api/
└── StudentManagementApi/
    ├── Controllers/
    │   └── StudentsController.cs
    ├── DTOs/
    │   ├── CreateStudentRequest.cs
    │   ├── UpdateStudentRequest.cs
    │   ├── UpdateStudentStatusRequest.cs
    │   ├── StudentResponse.cs
    │   ├── PagedResultResponse.cs
    │   └── StudentStatsResponse.cs
    ├── Models/
    │   └── Student.cs
    ├── Services/
    │   ├── IStudentService.cs
    │   └── StudentService.cs
    ├── Program.cs
    └── README.md
🛠️ Features Breakdown & Endpoints
Method	Endpoint Route	Description	Query Parameters / Body	Expected Status Codes
GET	/api/students	Get all students with pagination & filters	search, trackName, isActive, pageNumber, pageSize	200 OK, 400 Bad Request
GET	/api/students/{id}	Get student by ID	Path variable: id	200 OK, 404 Not Found
GET	/api/students/by-track/{trackName}	Filter students by track name	Path variable: trackName	200 OK
GET	/api/students/stats	Aggregate stats (Total, Active, Inactive, Track Counts)	None	200 OK
POST	/api/students	Create new student profile	Body: CreateStudentRequest	201 Created, 400 Bad Request
PUT	/api/students/{id}	Update student core details	Path variable: id, Body: UpdateStudentRequest	200 OK, 400 Bad Request, 404 Not Found
PATCH	/api/students/{id}/status	Activate/Deactivate student profile	Path variable: id, Body: UpdateStudentStatusRequest	200 OK, 404 Not Found
DELETE	/api/students/{id}	Soft-delete student (sets IsActive = false)	Path variable: id	200 OK, 404 Not Found
⚡ Feature Implementation Details
1. Create Student (POST /api/students)
Validates required fields (FullName, Email, PhoneNumber, TrackName).

Verifies unique email constraint before creation.

Returns 201 Created with a Location header pointing to GET /api/students/{id}.

2. Get All Students with Pagination (GET /api/students)
Supports optional case-insensitive searching on FullName and Email.

Supports filtering by TrackName and IsActive state.

Returns data wrapped in PagedResultResponse<StudentResponse> containing metadata (totalCount, pageNumber, pageSize, totalPages).

3. Get Student By Id (GET /api/students/{id})
Fetches student by primary identifier.

Returns 404 Not Found with a structured message if missing (never returns null with 200 OK).

4. Update Student (PUT /api/students/{id})
Updates core student profile attributes.

Preserves immutable attributes (StudentId and EnrollmentDate).

Validates email uniqueness against other existing records.

5. Update Student Status (PATCH /api/students/{id}/status)
Allows toggling IsActive flag without overwriting full entity fields.

6. Student Statistics (GET /api/students/stats)
Computes real-time aggregate data:

Total students count.

Active vs. Inactive students count.

Dynamic grouping count per track (CountByTrack).

🧪 How to Test
Clone & Navigate:

Bash
cd task-02-student-management-api/StudentManagementApi
Run Application:

Bash
dotnet run
Open Swagger UI:
Navigate to https://localhost:<port>/swagger in your browser.

Postman Testing:
Import the endpoint routes or run requests through Postman against the local host URL.
