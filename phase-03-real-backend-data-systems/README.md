# Phase 03 - Real Backend Data Systems

## Overview

Phase 03 focuses on transforming ASP.NET Core Web APIs into real, database-driven backend systems.

The phase introduces Entity Framework Core, SQL Server, database modeling, requirements analysis, querying, filtering, reporting, business rules, data integrity, remote databases, production hosting, API refactoring, and technical interview preparation.

Unlike the previous phases, the final output is not a local-only or in-memory API.

The goal is to build a production-like backend system that starts from business requirements and ends with a live API connected to a real SQL Server database.

---

## Phase Goal

The main goal of Phase 03 is to develop the ability to:

**Read Requirements → Design Database → Build EF Core Models → Build API → Query Real Data → Apply Business Rules → Connect Remote Database → Deploy API → Refactor → Explain & Demo the System**

The phase is designed to simulate a real backend development workflow rather than isolated coding exercises.

---

## Phase Tasks

1. Task 00 - Workspace & Environment Setup
2. Task 01 - EF Core Modeling Drills
3. Task 02 - Requirements to ERD
4. Task 03 - Training Center Database API
5. Task 04 - Querying, Filtering & Reporting
6. Task 05 - Business Rules & Data Integrity
7. Task 06 - Production Hosting & Remote Database
8. Task 07 - EF Core API Refactor Pack
9. Task 08 - Interview & Demo Pack

---

## Main Project

### Training Center Registration API

The main project of Phase 03 is a database-driven Training Center Registration API.

The project is built from business requirements and requires students to:

* Analyze the requirements
* Identify entities and relationships
* Design the database
* Create EF Core models
* Build a real SQL Server database
* Implement the Web API
* Query and filter real data
* Generate reports
* Enforce business rules
* Maintain data integrity
* Connect to a remote database
* Deploy the API
* Refactor poor EF Core code
* Prepare technical explanations
* Demonstrate the completed system
* Provide evidence that the system works

---

## Main Learning Areas

### Entity Framework Core

* EF Core fundamentals
* DbContext
* DbSet
* Entity configuration
* Code First
* Migrations
* Database creation
* Database updates
* Relationships
* Navigation properties
* Foreign keys
* Constraints
* Data seeding
* Tracking and No-Tracking queries
* Loading related data

### SQL Server

* Relational database concepts
* Tables
* Primary keys
* Foreign keys
* Relationships
* Constraints
* SQL Server database management
* Local SQL Server
* Remote SQL Server
* Connection strings
* Database deployment

### Database Modeling

* Requirement analysis
* Entity identification
* Relationship analysis
* ERD
* One-to-One relationships
* One-to-Many relationships
* Many-to-Many relationships
* Database normalization
* Data integrity
* Foreign key constraints

### API Development

* ASP.NET Core Web API
* Controllers
* DTOs
* Services
* Dependency Injection
* Routing
* HTTP methods
* HTTP status codes
* Swagger / OpenAPI
* Postman
* API testing

### Querying & Reporting

* LINQ
* Filtering
* Searching
* Sorting
* Pagination
* Projection
* Aggregation
* Grouping
* Reporting queries
* Query specifications
* Efficient database queries
* Database-side filtering

### Business Rules

* Requirement-based validation
* Business rule enforcement
* Preventing invalid operations
* Data consistency
* Validation before database operations
* Domain-specific rules
* Conflict handling
* Data integrity

### Production & Deployment

* Remote SQL Server
* Production connection strings
* Environment configuration
* Hosting
* Live Swagger
* Database deployment
* API deployment
* Secrets management
* Production troubleshooting
* Verifying the deployed API

### Refactoring

* Identifying bad EF Core code
* Improving query efficiency
* Avoiding unnecessary database calls
* Avoiding EF entities in API responses
* Proper DTO usage
* Improving service structure
* Improving maintainability
* Applying production-minded practices
* Separating responsibilities

### Interview & Demo Preparation

* Explaining the project architecture
* Explaining database relationships
* Explaining EF Core decisions
* Explaining LINQ queries
* Explaining business rules
* Explaining API endpoints
* Explaining deployment
* Explaining common backend decisions
* Demonstrating the API using Swagger or Postman
* Answering technical interview questions
* Explaining problems and solutions
* Presenting the project clearly

---

## Production Mindset

Throughout this phase, the following principles should be followed:

* Do not hardcode secrets.
* Do not expose connection strings publicly.
* Do not return EF Core entities directly from API endpoints.
* Use DTOs for API contracts.
* Do not skip migrations.
* Do not rely on fake or in-memory data for the final system.
* Do not submit untested endpoints.
* Validate business rules before modifying data.
* Keep database access organized and maintainable.
* Test the API locally before deployment.
* Verify the deployed API against the remote database.
* Understand the code instead of copying implementations.
* Be able to explain important technical decisions during the final demo.

---

## Definition of Done

Phase 03 is considered complete when:

* The local API works with SQL Server.
* EF Core models and relationships are correctly implemented.
* Database migrations are created and applied.
* The required business rules are enforced.
* Queries, filtering, and reports work correctly.
* The remote database is configured.
* The API is deployed online.
* The live Swagger URL works.
* Sensitive configuration is not exposed.
* Required endpoints have been tested.
* Evidence screenshots are available.
* A demo video is available when required.
* The repository contains the required documentation.
* The student can explain the main technical decisions.
* The student can demonstrate the completed API.
* The student can answer common interview questions related to the phase.

---

## Required Repository Structure

```text
techmaster-aspnet-backend-training/
│
├── README.md
│
├── phase-01-backend-foundations/
│
├── phase-02-web-api-basics/
│
└── phase-03-real-backend-data-systems/
    │
    ├── README.md
    │
    ├── task-00-workspace-environment-setup/
    │
    ├── task-01-ef-core-modeling-drills/
    │
    ├── task-02-requirements-to-erd/
    │
    ├── task-03-training-center-database-api/
    │
    ├── task-04-querying-filtering-reporting/
    │
    ├── task-05-business-rules-data-integrity/
    │
    ├── task-06-production-hosting-remote-database/
    │
    ├── task-07-ef-core-api-refactor-pack/
    │
    └── task-08-interview-demo-pack/
```

---

## Evidence & Documentation

The phase requires proof of implementation rather than code alone.

Evidence may include:

* Swagger screenshots
* Postman screenshots
* SQL Server screenshots
* Database tables and relationships
* ERD
* Migration evidence
* API responses
* Query results
* Filtering and reporting results
* Business-rule validation results
* Remote database configuration evidence
* Live Swagger URL
* Deployment evidence
* Refactoring evidence
* Demo video
* Interview preparation answers

All evidence should clearly demonstrate that the implemented functionality actually works.

---

## Development Workflow

The recommended workflow for the phase is:

```text
Business Requirements
        ↓
Requirements Analysis
        ↓
ERD / Database Design
        ↓
EF Core Models
        ↓
DbContext & Relationships
        ↓
Migrations
        ↓
SQL Server Database
        ↓
DTOs & Services
        ↓
ASP.NET Core Web API
        ↓
LINQ Queries
        ↓
Filtering & Reporting
        ↓
Business Rules
        ↓
Data Integrity
        ↓
Testing
        ↓
Remote Database
        ↓
Production Hosting
        ↓
Live API
        ↓
EF Core Refactoring
        ↓
Interview Preparation
        ↓
Final Demo
```

---

## Tools & Technologies

* C#
* .NET
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* LINQ
* REST APIs
* Swagger / OpenAPI
* Postman
* Git
* GitHub
* Remote SQL Database
* API Hosting Platform

---

## Phase 03 Progress

| Task                                           | Status        |
| ---------------------------------------------- | -----------   |
| Task 00 - Workspace & Environment Setup        | Completed     |
| Task 01 - EF Core Modeling Drills              | Completed     |
| Task 02 - Requirements to ERD                  | Completed     |
| Task 03 - Training Center Database API         | Completed     |
| Task 04 - Querying, Filtering & Reporting      | Completed     |
| Task 05 - Business Rules & Data Integrity      | Completed     |
| Task 06 - Production Hosting & Remote Database | Completed     |
| Task 07 - EF Core API Refactor Pack            | Completed     |
| Task 08 - Interview & Demo Pack                | Completed     |

---

## Overall Progress

Phase 03 represents the transition from Web API fundamentals to real-world, database-driven backend development.

Phase 01 established C#, OOP, and backend programming fundamentals.

Phase 02 introduced ASP.NET Core Web API, REST principles, routing, DTOs, validation, services, Swagger, Postman, and API standards.

Phase 03 builds on these foundations by introducing persistent data, Entity Framework Core, SQL Server, database modeling, advanced querying, business rules, remote databases, production hosting, EF Core refactoring, and technical interview preparation.

The final objective is to demonstrate the ability to take a backend system from:

**Business Requirements → Database Design → Implementation → Testing → Deployment → Refactoring → Technical Explanation → Live Demo**

This phase is designed to develop both **backend engineering skills** and the ability to explain, defend, and demonstrate the implemented system in a professional environment.
