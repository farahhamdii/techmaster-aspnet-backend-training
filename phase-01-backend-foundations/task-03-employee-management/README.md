# Task 03 - Employee Management Console App

## Overview

A simple HR console application built with C# to manage company employees.

The application allows HR employees to add, update, deactivate, search, filter, and sort employees. It also provides salary and employee statistics reports for management.

---

## Project Structure

```text
task-03-employee-management/
│
├── README.md
│
└── EmployeeManagement/
    │
    ├── Models/
    │   ├── Employee.cs
    │   └── Department.cs
    │
    ├── Services/
    │   ├── EmployeeService.cs
    │   └── EmployeeReportService.cs
    │
    ├── UI/
    │   └── ConsoleMenu.cs
    │
    ├── Program.cs
    └── EmployeeManagement.csproj
```

---

## Features

### 1. Add Employee

* Creates a new employee.
* Generates a unique `EmployeeId`.
* Validates required fields.
* Salary must be positive.
* Hire date cannot be in the future.
* New employees are automatically active.

### 2. Update Employee

Allows updating:

* Email
* Department
* Position
* Salary

The `EmployeeId` remains unchanged.

### 3. Deactivate Employee

* Employees are not deleted.
* `IsActive` is changed to `false`.
* This preserves employee records.

### 4. Search Employees

Employees can be searched by:

* Employee ID
* Full name
* Partial name

Search is case-insensitive.

### 5. Filter by Department

Employees can be filtered by:

* IT
* HR
* Sales
* Finance
* Marketing
* Support

Inactive employees are excluded by default.

### 6. Sort Employees

Employees can be sorted by:

* Salary ascending
* Salary descending
* Hire date ascending
* Hire date descending
* Name

### 7. Salary Reports

The application provides:

* Average salary
* Highest salary employee
* Lowest salary employee
* Total payroll
* Employee count by department
* Active employee count
* Inactive employee count

### 8. View All Employees

Displays all employees with their:

* Employee ID
* Full name
* Email
* Department
* Position
* Salary
* Hire date
* Active/Inactive status

---

## Seed Data

The application starts with 12 employees distributed across different departments.

The seed data includes both active and inactive employees to make searching, filtering, sorting, and reporting meaningful.

---

## Technologies

* C#
* .NET
* Object-Oriented Programming
* Collections
* LINQ
* Console Application

---

## Architecture

The application separates responsibilities into different layers:

### Models

Contains the employee data model and department enumeration.

### Services

`EmployeeService` handles employee operations such as:

* Add
* Update
* Deactivate
* Search
* Filter
* Sort

`EmployeeReportService` handles:

* Salary calculations
* Payroll reports
* Department statistics
* Active/inactive statistics

### UI

`ConsoleMenu` handles user interaction and the console menu.

`Program.cs` is responsible only for creating the required services and starting the application.

This keeps business logic outside `Main`.

---

## Validation

The application validates:

* Required employee information
* Unique employee IDs
* Positive salary when adding employees
* Non-negative salary when updating employees
* Hire date cannot be in the future
* Employee must exist before update/deactivation

Clear validation messages are displayed to the user.

---

## How to Run

Open the `EmployeeManagement` folder and run:

```bash
dotnet run
```

---

## Main Menu

```text
====== Employee Management System ======
1. Add Employee
2. Update Employee
3. Deactivate Employee
4. Search Employee
5. Filter by Department
6. Sort Employees
7. Show Salary Reports
8. View All Employees
9. Exit
========================================
```

---

## Task Acceptance Checklist

* EmployeeId is unique
* Search supports partial names
* Search is case-insensitive
* Deactivation uses `IsActive`
* Employees are not removed from memory
* Salary reports are implemented
* Filtering handles inactive employees
* Code is separated into Model, Service, and UI
* Seed data contains 12 employees
* README documentation is included

