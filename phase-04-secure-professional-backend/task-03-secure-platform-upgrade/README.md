# Task 03 – Secure Platform Upgrade

## Overview

Upgraded the Training Center API security by applying authentication, role-based authorization, ownership checks, and protected access to sensitive platform features.

## Security Upgrades

* Protected sensitive endpoints from anonymous access.
* Applied role-based authorization.
* Added ownership checks for Student and Instructor resources.
* Used JWT claims to identify the current user.
* Restricted private data to the correct user.
* Protected payment and administrative operations.

## Implemented Features

### Admin

* Create and manage training tracks.
* Assign instructors through track management.
* View all enrollments.
* Approve/update enrollment status.
* Update payment status.
* Access revenue reports.

### Instructor

* View assigned tracks:
  `GET /api/instructor/my-tracks`
* View students belonging to own tracks.
* Cannot access another instructor's tracks.

### Student

* View own enrollments:
  `GET /api/student/my-enrollments`
* View own payment history:
  `GET /api/student/my-payments`
* Access is based on the Student ID stored in the JWT claims.

### Training Tracks

* Added available tracks endpoint for students.
* Available tracks are:

  * Active
  * Not deleted
  * Before capacity limit
  * Upcoming

## Protected Endpoints

| Feature                    | Authorization      |
| -------------------------- | ------------------ |
| Student private data       | Student / Owner    |
| Instructor private data    | Instructor / Owner |
| Track management           | Admin              |
| Enrollment management      | Admin              |
| Payment status             | Admin              |
| Revenue reports            | Admin              |
| Student enrollments        | Student            |
| Student payments           | Student            |
| Instructor assigned tracks | Instructor         |

## Validation & Business Rules

* Inactive instructors cannot be assigned to tracks.
* Students cannot access other students' private data.
* Instructors cannot access another instructor's tracks.
* Payment status updates are restricted to Admin.
* Enrollment status transitions are validated.
* Deleted tracks are excluded from active operations.
* Track capacity is checked before enrollment-related operations.

## Testing

Security behavior was verified using Swagger/Postman with different user roles.

Expected results:

* No token → `401 Unauthorized`
* Authenticated user with wrong role → `403 Forbidden`
* Authorized user → `200 OK`
