# Task 08 – Bad Auth Refactor Pack

## Overview

This task focuses on refactoring authentication code to improve security, maintainability, and reliability in the Training Center API.

The original authentication implementation contained several security issues, including plain-text password comparison, fake token generation, unsafe user responses, and direct database access inside the controller.

## Implemented Fixes

### 1. Secure Refresh Token Storage
Refresh tokens are generated using cryptographically secure random bytes. Instead of storing the original refresh token, the system stores its SHA-256 hash.

### 2. Unique Refresh Token Hashes
A unique database index is configured on `RefreshToken.TokenHash` to prevent duplicate token hashes from being stored.

### 3. Real JWT Generation
A dedicated `JwtService` generates signed JWT access tokens using the configured signing key and the HMAC-SHA256 algorithm.

### 4. JWT Claims
The access token includes the user's identifier, email, and role. It also includes `StudentId` or `InstructorId` when applicable.

### 5. Configurable Access Token Expiration
The access token lifetime is configurable through `Jwt:AccessTokenMinutes`, with a default of 15 minutes when the configured value is invalid.

### 6. Configurable Refresh Token Expiration
The refresh token lifetime is configurable through `Jwt:RefreshTokenDays`, with a default of 7 days when the configured value is invalid.

### 7. Refresh Token Rotation
When a valid refresh token is used, the service creates a replacement refresh token and records the replacement hash in `ReplacedByTokenHash`.

### 8. Refresh Token Revocation
The previous refresh token is revoked by setting `RevokedAt`. Revoked tokens are rejected when submitted for refresh.

### 9. Refresh Token Expiration Validation
Expired refresh tokens are rejected before a new token pair is issued.

### 10. Inactive User Protection During Refresh
The refresh service checks `ApplicationUser.IsActive` and rejects refresh requests for inactive accounts.

### 11. Safe Refresh Token Validation
Empty, unknown, revoked, and expired refresh tokens are rejected instead of being used to issue new tokens.

### 12. User–Refresh Token Relationship
A foreign key connects each refresh token to its associated application user. Cascade delete is configured for this relationship.

### 13. Asynchronous Database Operations
The refresh token service uses asynchronous Entity Framework Core operations, including `FirstOrDefaultAsync` and `SaveChangesAsync`.

## Database Configuration

The `TrainingCenterDbContext` configures:

- A unique index on `RefreshToken.TokenHash`.
- A relationship between `RefreshToken` and `ApplicationUser`.
- A unique index on `ApplicationUser.Email`.

These constraints help maintain data integrity and prevent duplicate values where uniqueness is required.

## Configuration

The JWT settings are read from configuration:

```json
{
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE",
    "AccessTokenMinutes": 15,
    "RefreshTokenDays": 7
  }
}
```

**Security note:** Use a strong secret key and store production secrets in secure configuration or environment variables. Do not commit real credentials or secret keys to source control.

## Testing Notes

The following scenarios should be tested through Swagger:

1. Submit a valid refresh token and verify that a new token pair is returned.
2. Reuse the old refresh token and verify that it is rejected.
3. Submit an empty refresh token and verify that it is rejected.
4. Submit an unknown refresh token and verify that it is rejected.
5. Submit an expired refresh token and verify that it is rejected.
6. Submit a revoked refresh token and verify that it is rejected.
7. Attempt to refresh a token belonging to an inactive user and verify that the request is rejected.

Record the actual HTTP status codes and responses after running these tests.

## Notes and Limitations

- Concurrent refresh requests have not yet been verified through dedicated tests.
- Login, registration, password hashing, role restrictions, authentication response DTOs, and audit logging must be reviewed separately against the complete Task 08 requirements.
- The JWT validation middleware and production configuration should be verified before deployment.

## Conclusion

The reviewed implementation includes secure refresh token generation and hashing, JWT access token generation, refresh token rotation and revocation, expiration checks, inactive-user checks during refresh, and database integrity constraints.

Further verification is required before considering the entire authentication refactoring task complete.
