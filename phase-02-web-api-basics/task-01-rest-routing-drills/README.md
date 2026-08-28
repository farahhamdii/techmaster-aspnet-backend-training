# Task 01: REST & Routing Drills

This repository contains the implementation of **Task 01: REST & Routing Drills** as part of the TechMaster ASP.NET Core Backend Training Program[cite: 1].

## 📌 Project Overview
The objective of this task is to master HTTP verbs, RESTful routing conventions, parameter binding (`[FromRoute]`, `[FromQuery]`, `[FromBody]`), and status codes in ASP.NET Core Web API[cite: 1].

---

## 🛠 Project Structure

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


---

## 🚀 Implemented Drills & Endpoints

### 1. Health Check (`HealthController`)
* **`GET /api/health`**[cite: 1]
  * **Description:** Returns the operational status of the API with server timestamp[cite: 1].
  * **Response:** `200 OK`[cite: 1]

---

### 2. Tools & Conversions (`ToolsController`)
* **`GET /api/tools/echo/{name}`**[cite: 1]
  * **Description:** Echoes back the provided name via route parameter[cite: 1].
  * **Binding:** `[FromRoute]`[cite: 1]
  * **Response:** `200 OK` or `400 BadRequest` if empty[cite: 1].

* **`GET /api/tools/convert?celsius={value}`**[cite: 1]
  * **Description:** Converts Celsius temperature to Fahrenheit[cite: 1].
  * **Binding:** `[FromQuery]`[cite: 1]
  * **Response:** `200 OK`[cite: 1]

---

### 3. Mathematical Operations (`CalculatorController`)
* **`GET /api/calculator/add?a={num1}&b={num2}`**[cite: 1]
  * **Description:** Performs addition of two numbers[cite: 1].
* **`GET /api/calculator/subtract?a={num1}&b={num2}`**[cite: 1]
  * **Description:** Performs subtraction[cite: 1].
* **`GET /api/calculator/multiply?a={num1}&b={num2}`**[cite: 1]
  * **Description:** Performs multiplication[cite: 1].
* **`GET /api/calculator/divide?a={num1}&b={num2}`**[cite: 1]
  * **Description:** Performs division with validation for division by zero[cite: 1].
  * **Response:** `200 OK` or `400 BadRequest` (on division by zero)[cite: 1].

---

### 4. Notes Resource (`NotesController`)
* **`POST /api/notes`**[cite: 1]
  * **Description:** Creates a new note from JSON body payload[cite: 1].
  * **Binding:** `[FromBody]` via `CreateNoteRequest` DTO[cite: 1].
  * **Response:** `201 Created` or `400 BadRequest`[cite: 1].

---

## 🧪 Testing & Verification
All endpoints were tested and verified using **Postman** and **Swagger UI**[cite: 1]. 
Evidence screenshots for headers, request parameters, and JSON payloads are saved in the dedicated Google Drive folder[cite: 1].

---

## 💻 Tech Stack
* **Framework:** .NET 8 / ASP.NET Core Web API[cite: 1]
* **Language:** C#[cite: 1]
* **Documentation:** Swagger (Swashbuckle)[cite: 1]
