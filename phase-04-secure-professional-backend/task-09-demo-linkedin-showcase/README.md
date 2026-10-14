# TechMaster Academy – Phase 04: Secure Professional Backend

## Overview

Phase 04 upgrades the Training Center API from Phase 03 into a more secure and professional backend application.

The project focuses on authentication, authorization, role-based access control, database security, validation, error handling, logging, audit trails, and production deployment.

The main objective is to build a backend API that follows security best practices, protects sensitive data, handles errors consistently, and can be verified in a production hosting environment.

## Project Information

- **Project:** Training Center API
- **Training Program:** TechMaster Academy – ASP.NET Backend Career Training
- **Phase:** Phase 04 – Secure Professional Backend
- **Framework:** ASP.NET Core Web API
- **Language:** C#
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** JWT Bearer Authentication
- **API Documentation:** Swagger / OpenAPI
- **Hosting Provider:** MonsterASP.NET

## Technology Stack

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- Password Hashing
- Role-Based Authorization
- Refresh Tokens
- Dependency Injection
- Swagger / OpenAPI
- ASP.NET Core Middleware
- Git and GitHub
- MonsterASP.NET

## Phase 04 Tasks

### Task 00 – Sprint Setup and Project Preparation

**Objective:** Prepare the Phase 04 project and establish a separate workspace for security and backend improvements.

Key activities:

- Prepared the Phase 04 project based on the existing Training Center API from Phase 03.
- Organized the work into focused tasks covering authentication, authorization, security, architecture, validation, logging, auditing, and deployment.
- Established a workflow for implementing and testing changes incrementally.

### Task 01 – Authentication Foundation

**Objective:** Implement secure user authentication and token management.

Authentication features include:

- User registration.
- User login.
- Password hashing.
- JWT access token generation and validation.
- Refresh token management.
- Access to the authenticated user's profile.
- Password change.
- Refresh token renewal.
- Logout and token revocation.

#### Authentication Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Authenticate a user and return tokens |
| GET | `/api/auth/me` | Get the current authenticated user |
| POST | `/api/auth/change-password` | Change the user's password |
| POST | `/api/auth/refresh-token` | Renew authentication tokens |
| POST | `/api/auth/logout` | Revoke the user's refresh token |

**Security considerations:**

- Passwords must never be stored as plain text.
- Protected endpoints require a valid access token.
- Refresh tokens are stored as hashes rather than raw token values.
- Expired or revoked refresh tokens must not be accepted.
- JWT validation checks the configured issuer and audience.

### Task 02 – Role-Based Authorization

**Objective:** Restrict API access according to the authenticated user's role.

The application uses the following roles:

- **Admin:** Administrative operations and authorized management reports.
- **Instructor:** Instructor-related operations permitted by the application's authorization rules.
- **Student:** Student-related operations permitted by the application's authorization rules.

Authorization is enforced through ASP.NET Core authentication and authorization mechanisms.

#### Expected Authorization Behavior

| Scenario | Expected result |
|---|---|
| Request without a token to a protected endpoint | `401 Unauthorized` |
| Request with a valid token and permitted role | Successful response |
| Authenticated user without the required role | `403 Forbidden` |
| Student attempting to access an admin-only endpoint | `403 Forbidden` |

Role restrictions must be applied to the relevant controllers or actions and verified through live API requests.

### Task 03 – Secure Platform Upgrade

**Objective:** Strengthen the existing API's security and prepare it for more reliable operation.

Security-related improvements include:

- JWT Bearer authentication configuration.
- Validation of token issuer and audience.
- Dependency injection for application services.
- Secure password handling.
- Refresh token expiration and revocation.
- Configuration of Swagger to support Bearer token authentication.
- Database connection configuration through application settings.
- Startup initialization for the administrator account.

The application uses a configured administrator seed account for initial administrative access when the account does not already exist.

**Important:** Production credentials and signing keys must be stored securely and must not be published in source control or documentation.

### Task 04 – Architecture Refactoring

**Objective:** Improve code organization and maintainability while preserving existing application behavior.

The refactoring focuses on separating responsibilities and reducing unnecessary coupling between application components.

Key architectural concerns include:

- Keeping controllers focused on HTTP requests and responses.
- Moving application logic into appropriate services.
- Using dependency injection to manage service dependencies.
- Separating database access from business logic where appropriate.
- Keeping entity models, request DTOs, and response DTOs organized.
- Avoiding unnecessary architectural abstractions.

The refactoring should preserve existing endpoint behavior and authentication rules.

**Verification:** Build the solution and test the affected endpoints after each structural change.

### Task 05 – Validation, Error Handling, and Logging

**Objective:** Make API behavior more predictable, consistent, and easier to troubleshoot.

The task covers:

- Validating incoming request data.
- Returning appropriate HTTP status codes.
- Handling unexpected exceptions through centralized exception-handling middleware.
- Providing consistent API response structures where applicable.
- Logging application errors for troubleshooting.
- Preventing internal exception details and sensitive information from being exposed to clients.

The application includes a reusable API response structure:

- `Success`
- `Message`
- `Data`

The exact response format depends on the endpoint and its implementation.

**Expected behavior:**

- Invalid requests should return meaningful validation errors.
- Unauthorized requests should return `401`.
- Forbidden requests should return `403`.
- Unexpected server errors should be handled consistently.
- Sensitive information must not be included in error responses or logs.

### Task 06 – Production Redeployment and Live API Verification

**Objective:** Deploy the upgraded API and verify its behavior against a remote SQL Server database.

#### Hosting

The existing hosting environment is MonsterASP.NET.

- **Live API:** `https://trainingcenterr.runasp.net`
- **Live Swagger:** `https://trainingcenterr.runasp.net/swagger`

The live URLs must be checked to confirm that the deployed application is accessible and running the latest version.

#### Production Verification Checklist

- [ ] Live Swagger opens successfully.
- [ ] The deployed application connects to the intended remote database.
- [ ] Registration works against the live API.
- [ ] Login returns a valid access token.
- [ ] Protected endpoints reject requests without a token.
- [ ] Authorized requests succeed when a valid token is supplied.
- [ ] A Student cannot access an Admin-only endpoint.
- [ ] At least one protected report returns the expected data.
- [ ] Production configuration does not expose secrets.
- [ ] Required evidence screenshots are collected.

#### Production Configuration

Production connection strings and JWT signing keys must be configured through secure hosting settings or environment variables whenever supported.

Real production secrets must not be committed to GitHub, included in this README, or exposed in screenshots.

#### Current Deployment Limitation

The remote database connection encountered a SQL Server connection error (`Error 53`) during the database update attempt.

The remote database connection, migration status, and live data retrieval must be verified before the production deployment can be considered fully successful.

If deployment remains blocked, the blocker and the evidence of local testing and remote connection attempts should be documented.

### Task 07 – Audit Logging

**Objective:** Record important application activities and make audit records available to authorized administrators.

The audit functionality is intended to help administrators review application activity and investigate changes.

#### Admin Activity Log Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/admin/activity-logs` | Retrieve activity logs |
| GET | `/api/admin/activity-logs?userId=5` | Filter logs by user |
| GET | `/api/admin/activity-logs?entityName=Payment` | Filter logs by entity |
| GET | `/api/admin/activity-logs?from=...&to=...` | Filter logs by date range |

The activity log endpoint is intended to return recent records with pagination and support administrative filtering.

**Access control:** Activity logs must be restricted to Admin users.

Audit logging should be handled so that a failure to record an audit event does not unintentionally break the primary business operation, where that behavior is appropriate to the requirements.

Audit implementation and filtering behavior should be verified against the actual application before being marked complete.

### Task 08 – Authentication Cleanup and Refactoring

**Objective:** Remove ambiguity from the authentication implementation and keep the active API routes unambiguous.

During Swagger verification, a duplicate route was identified:

`POST /api/auth/register`

The route was associated with both `AuthController.Register` and `OriginalBadAuthController.Register`, causing a Swagger generation conflict.

The required cleanup is to ensure that the legacy authentication implementation is not exposed as a duplicate active API route.

Possible approaches include:

- Keeping legacy code as a non-controller reference file.
- Excluding the legacy controller from runtime controller discovery.
- Assigning a separate route if the legacy endpoint must remain available for testing.

The active authentication controller should remain the authoritative implementation.

**Verification checklist:**

- [ ] Only one active registration endpoint uses `POST /api/auth/register`.
- [ ] Swagger loads without conflicting method/path errors.
- [ ] Login and registration still work after the cleanup.
- [ ] JWT authentication and role authorization remain functional.

### Task 09 – Final Documentation and Demonstration

**Objective:** Prepare the project for review, demonstration, and submission.

Final deliverables include:

- A project README.
- A summary of the implemented security features.
- Authentication and authorization endpoint documentation.
- Deployment notes.
- A list of known limitations.
- Evidence screenshots for the required verification scenarios.
- A demonstration of hosted authentication and role-based access control.

The final submission should clearly distinguish verified functionality from features that still require production testing.

## Database and Entity Framework Core

The project uses Entity Framework Core with SQL Server.

Database changes should be managed through EF Core migrations where applicable.

Typical commands:

```powershell
Add-Migration MigrationName -Context TrainingCenterDbContext
Update-Database -Context TrainingCenterDbContext
```

For production deployments, migrations must target the intended remote database and use the correct production configuration.

**Warning:** Review migrations before applying them to production. Changes that drop tables, columns, or data may cause irreversible data loss.

The local database and hosted database are separate environments. Local records are not automatically copied to the remote database during application publishing.

## Running the Project Locally

### Prerequisites

- .NET SDK compatible with the project target framework.
- SQL Server or an accessible SQL Server instance.
- Visual Studio or another compatible .NET development environment.

### Setup

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Configure the development connection string in local configuration.
4. Configure the required JWT settings.
5. Restore NuGet packages.
6. Apply database migrations to the intended development database.
7. Build and run the API.
8. Open Swagger and test the endpoints.

Example commands:

```bash
dotnet restore
dotnet build
dotnet run
```

Do not copy production credentials into a public configuration file.

## Authentication Testing with Swagger

1. Open the Swagger UI.
2. Register a test account or use an existing test account.
3. Log in through `/api/auth/login`.
4. Copy the returned access token.
5. Click **Authorize** in Swagger.
6. Enter the token using the Bearer authentication scheme.
7. Execute a protected endpoint.
8. Repeat without a token to verify authentication enforcement.
9. Test an Admin-only endpoint using a Student account to verify role restrictions.

Never include real passwords or reusable access tokens in public screenshots.

## Production Evidence

The following evidence should be included in the final submission:

| Evidence | Required demonstration |
|---|---|
| Live Swagger | Publicly or mentor-accessible API documentation |
| Remote database | Tables and records with credentials hidden |
| Registration | Successful registration against the hosted API |
| Login | Live login returning an access token |
| Protected endpoint | Successful request using a Bearer token |
| Authentication rejection | Protected endpoint rejects a missing token |
| Role rejection | Student receives `403 Forbidden` from an Admin-only endpoint |
| Protected report | At least one report returns expected data |
| Deployment notes | Hosting steps and known limitations |

## Security Best Practices

- Never commit production connection strings.
- Never publish database passwords.
- Never include JWT signing keys in the README.
- Never upload production `appsettings.json` containing real secrets.
- Use secure hosting configuration or environment variables for production secrets.
- Use HTTPS for production API traffic.
- Hash passwords and refresh tokens as required by the authentication design.
- Avoid exposing internal exception details to clients.
- Restrict administrative endpoints using explicit authorization rules.
- Review database migrations before applying them to production.

## Known Limitations

The following items must be updated according to the latest test results:

- Remote database connectivity and migration status require confirmation.
- The presence of existing production records must be verified against the intended database.
- The latest deployment must be checked to ensure the hosted application contains the latest code.
- Role-based rejection and protected report responses must be demonstrated through actual API requests.
- Audit log filtering and pagination must be verified.
- Any remaining Swagger conflicts must be resolved before the final demonstration.


## Conclusion

Phase 04 extends the Training Center API with a stronger focus on security, authentication, authorization, maintainability, error handling, auditing, and production readiness.

The final goal is to demonstrate a secure ASP.NET Core backend that works with a remote SQL Server database and enforces its access-control rules in a deployed environment.

Production completion should only be claimed after the hosted API, remote database, authentication flow, and role-based restrictions have been verified successfully.

**Training Program:** TechMaster Academy – ASP.NET Backend Career Training  
**Phase:** Phase 04 – Secure Professional Backend
