# 🎓 Student Management API — Task 02

A clean, in-memory RESTful CRUD API built with **ASP.NET Core** to simulate a training center system for managing students, tracks, and student statistics.

The project focuses on building a structured Web API with clear separation of concerns, input validation, pagination, filtering, and standard HTTP status codes.

---

## 📌 Project Overview

The **Student Management API** provides a complete set of RESTful endpoints for managing student profiles in a training center.

The API supports:

* Creating new students
* Retrieving students
* Searching and filtering
* Pagination
* Updating student information
* Activating and deactivating students
* Soft deletion
* Filtering students by track
* Generating real-time student statistics

The application uses an **in-memory data store**, so no external database is required.

---

## 🏗️ Architecture & Design

The project follows an **N-Tier / Clean Architecture-inspired structure** to keep responsibilities separated and make the application easier to maintain and extend.

### Controllers Layer

**Location:** `Controllers/`

Responsible for:

* Handling HTTP requests
* Defining API routes
* Receiving request DTOs
* Calling the service layer
* Returning appropriate HTTP status codes
* Handling API responses

The controller does **not** contain the main business logic.

---

### Services Layer

**Location:** `Services/`

Contains the application's core business logic.

Responsibilities include:

* Managing in-memory student data
* Creating, updating, and deleting students
* Searching and filtering
* Pagination
* Email uniqueness validation
* Student statistics
* Data transformations
* Thread-safe state management

The service layer is accessed through the `IStudentService` interface to keep the controller decoupled from the implementation.

---

### DTOs Layer

**Location:** `DTOs/`

DTOs are used to control the data entering and leaving the API.

They provide:

* Request validation using **DataAnnotations**
* Separation between API contracts and internal models
* Controlled response shapes
* Protection of internal domain model details

---

### Domain Models

**Location:** `Models/`

Contains the internal domain entities used by the application.

Currently, the main domain model is:

* `Student`

---

## 📁 Project Structure

```text
task-02-student-management-api/
└── StudentManagementApi/
    │
    ├── Controllers/
    │   └── StudentsController.cs
    │
    ├── DTOs/
    │   ├── CreateStudentRequest.cs
    │   ├── UpdateStudentRequest.cs
    │   ├── UpdateStudentStatusRequest.cs
    │   ├── StudentResponse.cs
    │   ├── PagedResultResponse.cs
    │   └── StudentStatsResponse.cs
    │
    ├── Models/
    │   └── Student.cs
    │
    ├── Services/
    │   ├── IStudentService.cs
    │   └── StudentService.cs
    │
    ├── Program.cs
    └── README.md
```

---

# 🛠️ Features

## 1. Create Student

### `POST /api/students`

Creates a new student profile.

### Validation

The API validates the required student information:

* Full Name
* Email
* Phone Number
* Track Name

It also verifies that the email address is unique before creating the student.

### Response

Returns:

* `201 Created` when the student is successfully created
* `400 Bad Request` when validation fails or the email already exists

The response includes a **Location header** pointing to the newly created student's resource.

---

## 2. Get All Students

### `GET /api/students`

Returns a paginated list of students.

### Supported Query Parameters

| Parameter    | Description                              |
| ------------ | ---------------------------------------- |
| `search`     | Searches by student name or email        |
| `trackName`  | Filters students by track                |
| `isActive`   | Filters active/inactive students         |
| `pageNumber` | Specifies the requested page             |
| `pageSize`   | Specifies the number of records per page |

### Example

```http
GET /api/students?search=ahmed&trackName=.NET&isActive=true&pageNumber=1&pageSize=10
```

### Response

The endpoint returns a `PagedResultResponse<StudentResponse>` containing:

* Student data
* Total count
* Current page number
* Page size
* Total pages

### Status Codes

* `200 OK`
* `400 Bad Request` for invalid pagination parameters

---

## 3. Get Student By ID

### `GET /api/students/{id}`

Retrieves a specific student using their unique identifier.

### Example

```http
GET /api/students/1
```

### Status Codes

* `200 OK` — Student found
* `404 Not Found` — Student does not exist

The API never returns `null` with a `200 OK` response when the requested student is missing.

---

## 4. Get Students By Track

### `GET /api/students/by-track/{trackName}`

Returns students belonging to a specific training track.

### Example

```http
GET /api/students/by-track/.NET
```

### Response

Returns the matching students.

### Status Code

* `200 OK`

---

## 5. Update Student

### `PUT /api/students/{id}`

Updates the student's core profile information.

### Example

```http
PUT /api/students/1
```

The request body contains the fields defined in `UpdateStudentRequest`.

### Business Rules

* `StudentId` cannot be modified.
* `EnrollmentDate` cannot be modified.
* Email uniqueness is validated.
* The existing student must exist before updating.

### Status Codes

* `200 OK`
* `400 Bad Request`
* `404 Not Found`

---

## 6. Update Student Status

### `PATCH /api/students/{id}/status`

Updates only the student's active status without modifying the rest of the student profile.

### Example

```http
PATCH /api/students/1/status
```

### Request Body

```json
{
  "isActive": false
}
```

### Status Codes

* `200 OK`
* `404 Not Found`

---

## 7. Soft Delete Student

### `DELETE /api/students/{id}`

Performs a **soft delete** instead of permanently removing the student.

The student's:

```text
IsActive = false
```

This allows the record to remain available while treating the student as inactive.

### Status Codes

* `200 OK`
* `404 Not Found`

---

## 8. Student Statistics

### `GET /api/students/stats`

Returns real-time aggregate statistics about students.

The endpoint provides:

* Total students count
* Active students count
* Inactive students count
* Student count per track

### Example Response

```json
{
  "totalStudents": 50,
  "activeStudents": 42,
  "inactiveStudents": 8,
  "countByTrack": {
    ".NET": 20,
    "Frontend": 15,
    "Flutter": 10,
    "AI": 5
  }
}
```

The statistics are calculated dynamically from the current in-memory data.

---

# 📋 API Endpoints

| Method   | Endpoint                             | Description                              | Status Codes        |
| -------- | ------------------------------------ | ---------------------------------------- | ------------------- |
| `GET`    | `/api/students`                      | Get students with pagination and filters | `200`, `400`        |
| `GET`    | `/api/students/{id}`                 | Get student by ID                        | `200`, `404`        |
| `GET`    | `/api/students/by-track/{trackName}` | Get students by track                    | `200`               |
| `GET`    | `/api/students/stats`                | Get student statistics                   | `200`               |
| `POST`   | `/api/students`                      | Create a new student                     | `201`, `400`        |
| `PUT`    | `/api/students/{id}`                 | Update student information               | `200`, `400`, `404` |
| `PATCH`  | `/api/students/{id}/status`          | Activate/deactivate student              | `200`, `404`        |
| `DELETE` | `/api/students/{id}`                 | Soft-delete student                      | `200`, `404`        |

---

# 🔍 Filtering & Searching

The API supports flexible student searching and filtering.

### Search

The `search` parameter performs a case-insensitive search across:

* `FullName`
* `Email`

Example:

```http
GET /api/students?search=farah
```

### Track Filtering

```http
GET /api/students?trackName=.NET
```

### Active Status Filtering

```http
GET /api/students?isActive=true
```

### Combining Filters

Multiple filters can be used together:

```http
GET /api/students?search=ahmed&trackName=.NET&isActive=true&pageNumber=1&pageSize=10
```

---

# 📄 Pagination

The API uses pagination to avoid returning all records at once.

Supported parameters:

```text
pageNumber
pageSize
```

The response contains pagination metadata:

```json
{
  "data": [],
  "totalCount": 50,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

This structure makes the API easier to consume from frontend applications.

---

# 🛡️ Validation & Business Rules

The API applies validation at the request level using **DataAnnotations**.

Examples include:

* Required fields
* Valid email format
* Valid request data
* Unique email constraint
* Valid pagination values
* Student existence checks

Invalid requests return:

```http
400 Bad Request
```

with validation information instead of allowing invalid data to enter the system.

---

# 🔄 HTTP Status Codes

The API follows standard RESTful HTTP status codes.

| Status Code       | Meaning                               |
| ----------------- | ------------------------------------- |
| `200 OK`          | Request completed successfully        |
| `201 Created`     | Resource created successfully         |
| `400 Bad Request` | Invalid request or validation failure |
| `404 Not Found`   | Requested resource does not exist     |

---

# 🧵 Thread-Safe In-Memory Storage

Because the application does not use a database, student records are stored in memory.

The service layer handles access to the shared in-memory state in a **thread-safe** manner to prevent inconsistent data when multiple requests access or modify the collection simultaneously.

---

# 🧪 Testing the API

## 1. Clone the Repository

Clone the project and navigate to the task directory:

```bash
cd task-02-student-management-api/StudentManagementApi
```

---

## 2. Run the Application

Use the following command:

```bash
dotnet run
```

The application will start on the configured HTTP/HTTPS ports.

---

## 3. Open Swagger

After starting the application, open Swagger UI:

```text
https://localhost:<port>/swagger
```

Swagger provides an interactive interface for testing all API endpoints.

---

## 4. Postman

The API can also be tested using **Postman**.

You can manually create requests using the endpoint routes listed above and send them to the local application URL.

Example:

```text
GET https://localhost:<port>/api/students
```

---

# 🧰 Technologies Used

* **C#**
* **ASP.NET Core Web API**
* **.NET**
* **RESTful API**
* **Swagger / OpenAPI**
* **DataAnnotations**
* **In-Memory Data Storage**
* **Dependency Injection**
* **DTO Pattern**
* **Service Layer Pattern**

---

# 🎯 Learning Objectives

This task demonstrates practical understanding of:

* ASP.NET Core Web API fundamentals
* RESTful API design
* CRUD operations
* Controllers and routing
* Dependency Injection
* Service layer architecture
* DTOs
* Model validation
* HTTP status codes
* Query parameters
* Filtering and searching
* Pagination
* Soft deletion
* Aggregation and statistics
* Thread-safe in-memory state management
* Swagger API testing

---

# 🚀 Future Improvements

The current implementation intentionally uses in-memory storage. It can be extended in the future with:

* SQL Server database
* Entity Framework Core
* Repository Pattern
* Authentication and Authorization
* JWT Authentication
* Global Exception Handling Middleware
* Logging
* FluentValidation
* AutoMapper
* Advanced pagination
* Sorting
* Unit and Integration Testing
* Caching

---

# 👩‍💻 Author

**Farah Hamdy**

Student Management API — Task 02
ASP.NET Core Backend Training
