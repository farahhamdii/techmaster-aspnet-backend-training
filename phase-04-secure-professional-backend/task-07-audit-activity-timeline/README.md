# Task: Activity Logs & Audit Trail

## Overview
Implemented an audit logging feature in the Training Center API to record important system activities and allow administrators to review activity logs securely.

## Features
- Records important actions performed in the system.
- Stores the current user ID and role.
- Records action details, entity information, timestamps, and IP addresses.
- Supports metadata as a JSON string.
- Stores a correlation ID to help trace requests.
- Provides an admin-only endpoint to view activity logs.
- Supports pagination and filtering.

## API Endpoint

### Get Activity Logs
`GET /api/admin/activity-logs`

**Access:** Admin only

**Supported Query Parameters:**
- `userId`: Filter logs by user ID.
- `entityName`: Filter logs by entity type.
- `from`: Filter logs starting from a specific date.
- `to`: Filter logs up to a specific date.
- `pageNumber`: Select the page number.
- `pageSize`: Specify the number of logs per page.

**Examples:**
```http
GET /api/admin/activity-logs
GET /api/admin/activity-logs?userId=5
GET /api/admin/activity-logs?entityName=Payment
GET /api/admin/activity-logs?from=2026-10-01&to=2026-10-12
GET /api/admin/activity-logs?pageNumber=1&pageSize=10
```

## Security
- Only users with the Admin role can access activity logs.
- Student users are not allowed to access the endpoint.

## Pagination Response
The response includes:
- `data`: Activity logs for the requested page.
- `pageNumber`: Current page number.
- `pageSize`: Number of records per page.
- `totalCount`: Total matching records.
- `totalPages`: Total number of pages.

## Technologies
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication and Role-Based Authorization

## Notes
Audit logging failures are recorded using application logging and are handled without interrupting the main operation.
