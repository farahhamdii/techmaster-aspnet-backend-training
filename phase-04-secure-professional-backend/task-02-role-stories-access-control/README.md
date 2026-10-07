# Task 02 – Role Stories & Access Control

## Overview

Implemented role-based authorization and ownership rules for the Training Center API.

## Roles

* **Admin** – Full administrative access
* **Instructor** – Access to own profile, assigned tracks, and related students
* **Student** – Access to own profile and enrollments

## Access Control

| Area        | Admin       | Instructor        | Student          |
| ----------- | ----------- | ----------------- | ---------------- |
| Students    | Full Access | Own Tracks        | Own Profile      |
| Instructors | Full Access | Own Profile       | No Access        |
| Tracks      | Full Access | Assigned Tracks   | Available Tracks |
| Enrollments | Full Access | Own Tracks        | Own Enrollments  |
| Payments    | Full Access | No Access         | Own Payments     |
| Reports     | Full Access | Own Track Reports | No Access        |
| Audit Logs  | Full Access | No Access         | No Access        |

## Security Rules

* Authentication is required for protected endpoints.
* Role-based authorization is applied using JWT claims.
* Ownership checks prevent users from accessing other users' private data.
* Students cannot access other students' profiles or enrollments.
* Instructors cannot access another instructor's tracks.
* Admin-only operations are protected from Student and Instructor roles.
* Sensitive operations use the authenticated user's identity from JWT claims.

## Implemented Endpoints

### Student

* `GET /api/students/{id}` – Own profile only
* `GET /api/student/my-enrollments` – Own enrollments

### Instructor

* `GET /api/instructors/{id}` – Own profile
* `GET /api/instructors/{id}/tracks` – Own assigned tracks
* `GET /api/instructor/my-tracks` – Current instructor's tracks

### Admin

* Student management
* Instructor management
* Track management
* Enrollment management
* Payment management
* Reports

## Authorization Tests

Verified:

* No token → `401 Unauthorized`
* Wrong role → `403 Forbidden`
* Student accessing another student → `403 Forbidden`
* Instructor accessing another instructor's track → `403 Forbidden`
* Student accessing own data → `200 OK`
* Instructor accessing own tracks → `200 OK`
* Admin accessing protected resources → `200 OK`
