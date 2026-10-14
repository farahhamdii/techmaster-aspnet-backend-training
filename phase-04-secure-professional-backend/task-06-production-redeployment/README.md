# Task 06 – Production Redeployment & Live API Verification

## Overview

This task focuses on redeploying the upgraded Training Center Secure API to a production hosting environment and verifying that it works correctly with a remote SQL Server database.

The goal is to validate the deployed API, authentication, authorization, role-based access control, and protected endpoints using the live API URL rather than relying only on the local development environment.

## Objectives

- Deploy the upgraded ASP.NET Core API to a hosting provider.
- Connect the deployed application to a remote SQL Server database.
- Configure production connection strings and secrets securely.
- Verify the live Swagger interface.
- Test registration, login, and JWT authentication.
- Verify protected endpoints and role-based authorization.
- Collect screenshots and deployment evidence.
- Document deployment steps, verification results, and known limitations.

## Technology Stack

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- Role-Based Authorization
- Swagger / OpenAPI
- MonsterASP.NET Hosting

## Production Deployment

### 1. Hosting Configuration

The API is deployed to MonsterASP.NET.

**Live API URL:**  
`https://trainingcenterr.runasp.net`

**Live Swagger URL:**  
`https://trainingcenterr.runasp.net/swagger`

These URLs should be verified after deployment.

### 2. Remote Database Configuration

The deployed API uses a remote SQL Server database instead of relying exclusively on a local database.

The production connection string must be configured through the hosting provider's environment settings or secure configuration options whenever possible.

Production credentials must not be committed to source control or included in public screenshots.

### 3. Database Verification

The remote database should be checked to confirm that:

- The required tables exist.
- Entity Framework Core migrations have been applied.
- Existing records can be retrieved.
- New records can be created through the live API.
- The deployed application is connected to the intended database.

## Live API Verification

### 1. Swagger Availability

**Endpoint:** `GET /swagger`

Verification:
- The live Swagger page opens successfully.
- API endpoints are listed correctly.
- Requests can be executed against the hosted API.

### 2. User Registration

**Endpoint:** `POST /api/auth/register`

Verification:
- Submit a valid registration request.
- Confirm that the API returns the expected response.
- Verify that the new user is stored in the remote database.

### 3. User Login

**Endpoint:** `POST /api/auth/login`

Verification:
- Submit valid login credentials.
- Confirm that the API returns a JWT access token.
- Use the token to access protected endpoints.

### 4. Authentication Verification

Verification:
- Send a request to a protected endpoint without a token.
- Confirm that the request is rejected with `401 Unauthorized`.
- Send the same request with a valid Bearer token.
- Confirm that an authenticated user can access the endpoint when authorized.

### 5. Role-Based Authorization

Verification:
- Sign in using a user with the required role.
- Access an endpoint restricted to that role.
- Confirm that an authorized request succeeds.
- Attempt to access an admin-only endpoint using a Student account.
- Confirm that the request returns `403 Forbidden` when the user is authenticated but lacks the required role.

### 6. Protected Reports

Verification:
- Call at least one protected report endpoint.
- Include a valid Bearer token.
- Confirm that the endpoint returns the expected data from the remote database.
- Verify that unauthorized requests are rejected.


Successful completion requires a publicly accessible or mentor-accessible API, working authentication and authorization, verified role restrictions, and sufficient evidence to demonstrate the deployed system's behavior.

