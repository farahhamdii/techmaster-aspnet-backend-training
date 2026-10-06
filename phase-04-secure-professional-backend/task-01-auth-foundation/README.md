# Task 01 - Authentication Foundation

## Overview

This task introduces a real authentication system to the Training Center API.

The goal is to allow users to:

* Register an account
* Login securely
* Receive JWT access tokens
* Access protected endpoints
* Retrieve the currently authenticated user
* Refresh expired access tokens
* Change their password
* Logout and revoke their refresh token

The authentication system is designed as a foundation for the upcoming role-based access control and security tasks.

---

## Objectives

* Implement user registration.
* Implement secure password hashing.
* Implement user login.
* Generate JWT access tokens.
* Protect API endpoints using JWT authentication.
* Identify the authenticated user from JWT claims.
* Implement refresh tokens.
* Implement refresh token rotation.
* Implement password change.
* Revoke refresh tokens during password changes.
* Implement logout.
* Prevent users from registering as Admin.
* Prevent inactive users from logging in.
* Store refresh token hashes instead of raw refresh tokens.
* Never expose passwords or password hashes through API responses.

---

## Technologies

* ASP.NET Core Web API
* .NET 8
* Entity Framework Core
* SQL Server
* JWT Bearer Authentication
* `Microsoft.AspNetCore.Authentication.JwtBearer`
* `Microsoft.Extensions.Identity.Core`
* `PasswordHasher<ApplicationUser>`
* Swagger / OpenAPI

---

# Authentication Flow

## Registration

A user registers using:

```http
POST /api/auth/register
```

Example request:

```json
{
  "fullName": "Farah Test",
  "email": "farah.test@example.com",
  "password": "Test@1234",
  "confirmPassword": "Test@1234",
  "role": "Student"
}
```

The system:

1. Validates the input.
2. Validates password strength.
3. Normalizes the email.
4. Checks email uniqueness.
5. Prevents Admin self-registration.
6. Hashes the password.
7. Creates the user.
8. Generates an access token.
9. Generates a refresh token.

---

# Login

```http
POST /api/auth/login
```

Example:

```json
{
  "email": "farah.test@example.com",
  "password": "Test@1234"
}
```

The system:

1. Finds the user using a case-insensitive normalized email.
2. Checks whether the account is active.
3. Verifies the hashed password.
4. Updates `LastLoginAt`.
5. Generates a JWT access token.
6. Generates a refresh token.

---

# JWT Access Token

The access token is a short-lived token used to access protected endpoints.

The token contains authentication-related claims such as:

* User ID
* Email
* Role
* Expiration
* Token ID (`jti`)

Sensitive information is never stored inside the JWT, including:

* Password
* Password hash
* Database connection strings
* API keys
* Secrets
* Payment information

Example protected request:

```http
GET /api/auth/me
Authorization: Bearer <access-token>
```

---

# Protected Endpoints

Endpoints marked with:

```csharp
[Authorize]
```

require a valid access token.

For example:

```http
GET /api/auth/me
```

Without a valid access token, the API returns:

```http
401 Unauthorized
```

The backend identifies the current user from the User ID stored in the access token.

---

# Refresh Tokens

Refresh tokens are used to obtain a new access token without requiring the user to login again.

Endpoint:

```http
POST /api/auth/refresh-token
```

Example:

```json
{
  "refreshToken": "<refresh-token>"
}
```

## Refresh Token Security

Refresh tokens are:

* Cryptographically generated.
* Stored as SHA-256 hashes in the database.
* Associated with a specific user.
* Expirable.
* Revocable.
* Rotated when used.

The raw refresh token is never stored in the database.

---

# Refresh Token Rotation

When a valid refresh token is used:

```text
Old Refresh Token
       ↓
     Revoked
       ↓
New Refresh Token
       +
New Access Token
```

The old refresh token cannot be used again.

This prevents a previously used refresh token from being reused.

---

# Change Password

Endpoint:

```http
POST /api/auth/change-password
```

This endpoint requires authentication.

Example:

```json
{
  "currentPassword": "Test@1234",
  "newPassword": "NewTest@1234",
  "confirmNewPassword": "NewTest@1234"
}
```

The system:

1. Verifies the current password.
2. Validates the new password.
3. Hashes the new password.
4. Updates the user's password.
5. Updates `UpdatedAt`.
6. Revokes the user's active refresh tokens.

Revoking refresh tokens helps terminate existing refreshable sessions after a password change.

---

# Logout

Endpoint:

```http
POST /api/auth/logout
```

Example:

```json
{
  "refreshToken": "<refresh-token>"
}
```

Logout revokes the supplied refresh token.

After logout, the same refresh token cannot be used to obtain a new access token.

### Important

Logout does not immediately invalidate an already-issued access token.

The access token remains valid until it expires.

The refresh token is revoked immediately, preventing the user from obtaining a new access token using that session.

---

# ApplicationUser

The authentication system introduces the `ApplicationUser` entity.

Main properties:

| Property       | Description                              |
| -------------- | ---------------------------------------- |
| `Id`           | Unique user identifier                   |
| `FullName`     | User's full name                         |
| `Email`        | Unique email address                     |
| `PasswordHash` | Hashed password                          |
| `Role`         | Student, Instructor, or Admin            |
| `IsActive`     | Determines whether the account can login |
| `CreatedAt`    | Account creation time                    |
| `UpdatedAt`    | Last account update                      |
| `LastLoginAt`  | Last successful login                    |
| `StudentId`    | Optional linked student                  |
| `InstructorId` | Optional linked instructor               |

---

# User Roles

The current role representation is:

```csharp
public enum UserRole
{
    Student,
    Instructor,
    Admin
}
```

During registration, users cannot register themselves as:

```text
Admin
```

Admin role management will be handled through the upcoming authorization and role-management tasks.

---

# RefreshToken Entity

The refresh token entity stores:

* Token hash
* User ID
* Creation date
* Expiration date
* Revocation date
* Replacement token hash

It also provides calculated states:

```text
IsRevoked
IsExpired
IsActive
```

The database contains only the hashed refresh token.

---

# API Endpoints

| Method | Endpoint                    | Authentication | Description                        |
| ------ | --------------------------- | -------------- | ---------------------------------- |
| POST   | `/api/auth/register`        | Anonymous      | Register a new user                |
| POST   | `/api/auth/login`           | Anonymous      | Login and receive tokens           |
| GET    | `/api/auth/me`              | Required       | Get current authenticated user     |
| POST   | `/api/auth/refresh-token`   | Anonymous      | Generate new access/refresh tokens |
| POST   | `/api/auth/change-password` | Required       | Change current password            |
| POST   | `/api/auth/logout`          | Anonymous      | Revoke refresh token               |

---

# Password Security

Passwords are never stored as plaintext.

The application uses:

```csharp
PasswordHasher<ApplicationUser>
```

Password validation requires:

* Minimum 8 characters
* At least one uppercase letter
* At least one lowercase letter
* At least one digit
* At least one special character

Example valid password:

```text
Test@1234
```

---

# JWT Configuration

JWT configuration contains:

```json
"Jwt": {
  "Issuer": "TrainingCenter",
  "Audience": "TrainingCenterClient",
  "AccessTokenMinutes": 15,
  "RefreshTokenDays": 7
}
```

The JWT signing key should not be committed to source control in a real production environment.

For production deployment, the signing key should be provided through a secure configuration mechanism such as:

* Environment variables
* .NET User Secrets for local development
* Secure deployment secrets

---

# Testing

The authentication flow was tested using Swagger.

## Test 1 - Register

```http
POST /api/auth/register
```

Expected:

```http
200 OK
```

The response contains:

* Access Token
* Refresh Token
* User information
* Access Token expiration

---

## Test 2 - Login

```http
POST /api/auth/login
```

Expected:

```http
200 OK
```

---

## Test 3 - Current User

Authorize Swagger using:

```text
Bearer <access-token>
```

Then call:

```http
GET /api/auth/me
```

Expected:

```http
200 OK
```

The API returns the authenticated user's information.

---

## Test 4 - Refresh Token

```http
POST /api/auth/refresh-token
```

Expected:

```http
200 OK
```

A new access token and refresh token are returned.

---

## Test 5 - Refresh Token Rotation

After refreshing, the old refresh token is revoked.

Trying to use the old refresh token again should return:

```http
401 Unauthorized
```

---

## Test 6 - Change Password

```http
POST /api/auth/change-password
```

Expected:

```http
200 OK
```

The user's active refresh tokens are revoked.

---

## Test 7 - Logout

```http
POST /api/auth/logout
```

Expected:

```http
204 No Content
```

The supplied refresh token becomes revoked.

Trying to use the same refresh token again should return:

```http
401 Unauthorized
```

---

# Security Rules Implemented

* Passwords are hashed.
* Password hashes are never returned.
* Emails are normalized.
* Emails are unique.
* Inactive accounts cannot login.
* Admin self-registration is blocked.
* JWT access tokens have expiration.
* JWT issuer and audience are validated.
* Invalid or expired access tokens are rejected.
* Refresh tokens are hashed before database storage.
* Refresh tokens expire.
* Refresh tokens can be revoked.
* Refresh tokens are rotated after successful refresh.
* Password changes revoke active refresh tokens.
* Logout revokes the supplied refresh token.
* Sensitive data is not included in JWT claims.

---

# Task Result

Task 01 establishes the authentication foundation required for the rest of the secure backend.

The API now supports:

```text
Register
   ↓
Login
   ↓
Access Token + Refresh Token
   ↓
Protected API Access
   ↓
Refresh Token Rotation
   ↓
Change Password / Logout
```

The next task will build on this authentication foundation to introduce **role-based access control and authorization rules**.
