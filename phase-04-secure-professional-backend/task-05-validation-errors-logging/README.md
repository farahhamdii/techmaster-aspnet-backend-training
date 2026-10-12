# Task 05 — Validation, Errors & Logging

## Overview
This task improves the reliability and security of the Training Center API through request validation, business rule enforcement, centralized exception handling, and structured application logging.

## Validation
The API validates incoming requests and enforces business rules, including:

- Required fields and valid user input.
- Unique email addresses and password strength during registration.
- Valid training track capacity and status.
- Student and training track existence.
- Duplicate enrollment prevention.
- Enrollment capacity restrictions.
- Positive payment amounts and valid payment statuses.
- Unique payment reference numbers.
- Valid enrollment status transitions.
- Authorization and ownership restrictions where applicable.

## Error Handling
- Global exception middleware handles unexpected exceptions.
- Safe error responses avoid exposing internal exception details.
- Missing resources return appropriate not-found responses.
- Invalid requests return appropriate client error responses.
- Authentication and authorization errors are handled using `401 Unauthorized` and `403 Forbidden` where applicable.
- API responses use a consistent response structure where implemented.

## Logging
Application logging uses ASP.NET Core `ILogger<T>`.

Logged events include:
- Successful and failed login attempts.
- Payment creation and payment status updates.
- Enrollment creation and status updates.
- Enrollment soft deletion.
- Unexpected exceptions through global exception middleware.

Passwords, access tokens, refresh tokens, and sensitive payment reference details are not intentionally written to application logs.

Logs are written through the configured logging providers, such as Console and Debug. No separate database logging table is required.

## Testing Evidence
The following evidence should be captured using Swagger or the relevant application output:

- Request validation error.
- Unauthorized request (`401`).
- Forbidden request (`403`).
- Global exception middleware implementation.
- Logging output for authentication, payment, and enrollment events.

## Technologies
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Global Exception Middleware
- ASP.NET Core Logging (`ILogger<T>`)
