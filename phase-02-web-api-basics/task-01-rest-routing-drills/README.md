# Task 01: REST & Routing Drills

This repository contains the implementation of **Task 01: REST & Routing Drills** as part of the TechMaster ASP.NET Core Backend Training Program[cite: 1].

## 📌 Project Overview
The objective of this task is to master HTTP verbs, RESTful routing conventions, parameter binding (`[FromRoute]`, `[FromQuery]`, `[FromBody]`), and status codes in ASP.NET Core Web API[cite: 1].

---
## 🛠 Project Structure
```text
task-01-rest-routing-drills/
  ├── README.md
  └── ApiRoutingDrills/
      ├── Controllers/
      │   ├── HealthController.cs
      │   ├── ToolsController.cs
      │   ├── CalculatorController.cs
      │   └── NotesController.cs
      ├── DTOs/
      │   ├── CreateNoteRequest.cs
      │   └── UpdateNoteRequest.cs
      ├── Services/
      │   └── ConverterService.cs
      └── Program.cs
```[cite: 1]
This project contains 15 API drills demonstrating foundational Web API concepts in ASP.NET Core, including controllers, route parameters, query strings, DTOs, response shapes, and HTTP status codes.

## Drills Progress Table

| Drill No. | Endpoint | Concept | Status | Evidence |
| :--- | :--- | :--- | :---: | :--- |
| **Drill 01** | `GET /api/health` | Basic endpoint / controller action | Done | Swagger / Postman Screenshot |
| **Drill 02** | `GET /api/tools/echo/{name}` | Route parameter | Done | Swagger / Postman Screenshot |
| **Drill 03** | `GET /api/calculator/add` | Query parameters (`?a=10&b=5`) | Done | Swagger / Postman Screenshot |
| **Drill 04** | `GET /api/converter/celsius-to-fahrenheit` | Business calculation endpoint | Done | Swagger / Postman Screenshot |
| **Drill 05** | `GET /api/grades/calculate` | Validation + Range conditions | Done | Swagger / Postman Screenshot |
| **Drill 06** | `POST /api/notes` | Request body DTO (`[FromBody]`) | Done | Swagger / Postman Screenshot |
| **Drill 07** | `GET /api/notes` | Collection response | Done | Swagger / Postman Screenshot |
| **Drill 08** | `GET /api/notes/{id}` | Route parameters + 404 handling | Done | Swagger / Postman Screenshot |
| **Drill 09** | `PUT /api/notes/{id}` | Resource update via PUT | Done | Swagger / Postman Screenshot |
| **Drill 10** | `DELETE /api/notes/{id}` | Resource deletion + status codes | Done | Swagger / Postman Screenshot |
| **Drill 11** | `GET /api/notes/search` | Search query filter (`?keyword=...`) | Done | Swagger / Postman Screenshot |
| **Drill 12** | `GET /api/notes?pageNumber=1&pageSize=5` | Skip / Take Pagination | Done | Swagger / Postman Screenshot |
| **Drill 13** | `GET /api/request-info` | Request Headers (`X-Student-Name`) | Done | Swagger / Postman Screenshot |
| **Drill 14** | Various (`GET` / `POST`) | HTTP Status Codes practice (200, 201, 204, 400, 404) | Done | Swagger / Postman Screenshot |
| **Drill 15** | `GET /api/errors/demo` | Standard Error Response Format | Done | Swagger / Postman Screenshot |

---

## How to Run & Test
1. Clone the repository and navigate to `phase-02-web-api-basics/task-01-rest-routing-drills`.
2. Run `dotnet restore` and `dotnet run`.
3. Open Swagger UI at `https://localhost:{port}/swagger`.
4. Test the endpoints using Swagger UI or Postman.
## 🧪 Testing & Verification
All endpoints were tested and verified using **Postman** and **Swagger UI**[cite: 1]. 
Evidence screenshots for headers, request parameters, and JSON payloads are saved in the dedicated Google Drive folder[cite: 1].

---

## 💻 Tech Stack
* **Framework:** .NET 8 / ASP.NET Core Web API[cite: 1]
* **Language:** C#[cite: 1]
* **Documentation:** Swagger (Swashbuckle)[cite: 1]
