# Task 05 - Business Rules & Data Integrity

## Overview

This task focuses on applying business rules and data integrity checks to the Training Center Registration API.

The goal is to move beyond simple CRUD operations and ensure that invalid business operations are rejected with clear error messages.

Business rules are implemented mainly inside the Service Layer.

---

## Business Rules Implemented

### 1. Student Rules

* Student Full Name is required.
* Student Email must be unique.
* Students are soft deleted instead of being permanently deleted.
* Deleted students are marked as inactive.
* Deleted students are excluded from normal student listing.
* Inactive or deleted students cannot create new enrollments.

### 2. Training Track Rules

* Track Title is required.
* Track Code must be unique.
* Track Capacity must be greater than zero.
* Start Date must be before End Date.
* Track must have a valid active instructor.
* Track capacity cannot be reduced below the current number of active enrollments.
* Inactive/closed tracks cannot accept new enrollments.
* Tracks with active enrollments cannot be deleted.

### 3. Enrollment Rules

* Only active and non-deleted students can enroll.
* Students cannot have another active or pending enrollment in the same track.
* New enrollments start with `Pending` status.
* Active enrollment count is used when checking track capacity.
* Cancelled enrollments do not count toward track capacity.
* Completed enrollments cannot be cancelled.
* Completed or cancelled enrollments cannot become active again.

### 4. Payment Rules

* Payment amount must be greater than zero.
* Payment method is required.
* Reference number is required and must be unique.
* Payment status must be one of:

  * `Pending`
  * `Paid`
  * `Failed`
  * `Refunded`
* Payment amount cannot exceed the remaining amount.
* Only `Paid` payments are considered valid payments for revenue calculations.
* A failed payment does not activate the enrollment.
* A successful `Paid` payment can activate the related pending enrollment.

---

## Error Handling

Invalid business operations throw `InvalidOperationException` from the Service Layer.

Controllers catch these exceptions and return:

```http
400 Bad Request
```

with a clear error message.

Example:

```json
{
  "success": false,
  "message": "Capacity must be greater than zero."
}
```

---

## Business Rules by Service

| Service              | Main Rules                                                                                   |
| -------------------- | -------------------------------------------------------------------------------------------- |
| StudentService       | Required name, unique email, soft delete, active/deleted validation                          |
| TrainingTrackService | Required title, unique code, capacity, dates, instructor, active enrollment capacity         |
| EnrollmentService    | Student/track validation, duplicate enrollment, capacity, status rules                       |
| PaymentService       | Amount validation, unique reference, payment status, remaining amount, enrollment activation |

---

## Validation Scenarios

### Student

| Scenario                            | Expected Result                 |
| ----------------------------------- | ------------------------------- |
| Create student with duplicate email | `400 Bad Request`               |
| Create student without full name    | `400 Bad Request`               |
| Delete student                      | Student is soft deleted         |
| Get students after deletion         | Deleted student is not returned |
| Enroll inactive/deleted student     | `400 Bad Request`               |

### Training Track

| Scenario                                 | Expected Result   |
| ---------------------------------------- | ----------------- |
| Create track with capacity `0`           | `400 Bad Request` |
| Create track without title               | `400 Bad Request` |
| Create track with duplicate code         | `400 Bad Request` |
| Create track without valid instructor    | `400 Bad Request` |
| Create track with invalid dates          | `400 Bad Request` |
| Reduce capacity below active enrollments | `400 Bad Request` |
| Enroll into inactive/closed track        | `400 Bad Request` |
| Delete track with active enrollments     | `400 Bad Request` |

### Enrollment

| Scenario                                   | Expected Result                |
| ------------------------------------------ | ------------------------------ |
| Create duplicate active/pending enrollment | `400 Bad Request`              |
| Create enrollment for inactive student     | `400 Bad Request`              |
| Create enrollment for inactive track       | `400 Bad Request`              |
| Enroll when track is full                  | `400 Bad Request`              |
| Create new enrollment                      | Status is `Pending`            |
| Cancelled enrollment                       | Does not count toward capacity |
| Cancel completed enrollment                | `400 Bad Request`              |

### Payment

| Scenario                                | Expected Result                  |
| --------------------------------------- | -------------------------------- |
| Create payment with amount `0`          | `400 Bad Request`                |
| Create payment with negative amount     | `400 Bad Request`                |
| Create payment with duplicate reference | `400 Bad Request`                |
| Create payment with invalid status      | `400 Bad Request`                |
| Create payment above remaining amount   | `400 Bad Request`                |
| Create failed payment                   | Enrollment remains `Pending`     |
| Create paid payment                     | Enrollment can become `Active`   |
| Revenue calculation                     | Only `Paid` payments are counted |

---

## API Response Behavior

Successful operations return normal success responses such as:

```http
200 OK
201 Created
```

Invalid business operations return:

```http
400 Bad Request
```

Missing resources return:

```http
404 Not Found
```

---

## Testing

Business rules are tested using Swagger/Postman by sending both valid and invalid requests.

The tests verify that:

* Valid requests are accepted.
* Invalid requests are rejected.
* Correct HTTP status codes are returned.
* Error messages clearly explain the violated business rule.
* Database state remains consistent after invalid operations.

---

## Key Concepts Practiced

* Business Rules
* Data Integrity
* Service Layer Validation
* Soft Delete
* Unique Constraints
* Capacity Validation
* Status Transitions
* Entity Relationships
* Payment Validation
* Exception Handling
* HTTP Status Codes
* API Testing

---

## Expected Outcome

By completing this task, the API is no longer limited to basic CRUD operations.

The Service Layer now protects important business rules and prevents invalid operations from creating inconsistent data.

The API validates business scenarios before modifying the database and provides clear feedback when a rule is violated.
