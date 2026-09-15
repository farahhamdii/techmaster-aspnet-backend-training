# Task 02 - Requirements to ERD

## Overview

This task translates a company-style business requirement into a relational database design for TechMaster Academy.

The system manages:

* Students
* Instructors
* Training Tracks
* Enrollments
* Payments

The ERD represents the database structure that will be implemented later using EF Core.

## Main Entities

### Student

Stores student information and account/activity status.

### Instructor

Stores instructor information and specialization.

### TrainingTrack

Represents a training track, including its capacity, level, dates, status, and main instructor.

### Enrollment

Connects students with training tracks and stores enrollment-specific information such as status, progress, and final result.

### Payment

Stores payments related to a specific enrollment.

## Relationships

* One Student can have many Enrollments.
* One TrainingTrack can have many Enrollments.
* One Instructor can teach many TrainingTracks.
* One Enrollment can have many Payments.
* Student and TrainingTrack have a many-to-many relationship through Enrollment.

## Primary Keys

* StudentId
* InstructorId
* TrainingTrackId
* EnrollmentId
* PaymentId

## Foreign Keys

* TrainingTrack.InstructorId → Instructor.InstructorId
* Enrollment.StudentId → Student.StudentId
* Enrollment.TrainingTrackId → TrainingTrack.TrainingTrackId
* Payment.EnrollmentId → Enrollment.EnrollmentId

## Important Business Rules

* Student email must be unique.
* Instructor email must be unique.
* Training track code must be unique.
* A track cannot exceed its capacity.
* Progress percentage must be between 0 and 100.
* Payment amount must be greater than zero.
* Training track end date should be after its start date.
* Every enrollment belongs to one student and one training track.
* Every payment belongs to one enrollment.
* System-generated creation dates use UTC.
* Students and tracks support soft deletion where applicable.

## Business Questions

The database design should support answering:

1. Which students are enrolled in a specific track?
2. Which tracks have available seats?
3. Which enrollments are unpaid?
4. How much revenue did each track generate?
5. Which instructor has the highest workload?
6. Which students have active enrollments?
7. Which tracks start this month?
8. What is the payment history for an enrollment?
9. Which tracks are full?
10. How many enrollments exist by status?

## Design Decisions

### Enrollment as a Separate Entity

Student and TrainingTrack have a many-to-many relationship. Enrollment is used as a separate entity because the relationship itself contains important data:

* EnrollmentDate
* Status
* ProgressPercentage
* FinalResult

Therefore, Enrollment is not just a junction table; it represents a real business concept.

### Payments Belong to Enrollments

Payments are linked directly to Enrollment rather than Student because payment information is related to a student's specific registration in a training track.

This also allows one enrollment to have multiple payments.

### Capacity

TrainingTrack stores its maximum capacity. The number of active enrollments can be compared with Capacity to determine available seats and whether a track is full.

### Audit Fields

CreatedAt, UpdatedAt, IsDeleted, and DeletedAt are included where required to support tracking and soft deletion.

## ERD

The ERD diagram is included in this folder as:

`ERD.png`

## Next Step

The database design created in this task will be used as the foundation for implementing the entities and relationships with Entity Framework Core in the following tasks.
