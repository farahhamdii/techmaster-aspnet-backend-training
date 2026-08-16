# Task 06 - SQL & ERD Starter

## Selected Scenario

**Library Management System**

The system is designed for a small library that needs to manage books, authors, categories, members, and borrowing operations.

Members can borrow books, and each borrowing operation stores the borrow date, due date, return date, and current status.

---

## Main Entities

* Authors
* Categories
* Books
* Members
* BorrowRecords

---

## Tables and Fields

### 1. Authors

| Field     | Type          | Key |
| --------- | ------------- | --- |
| AuthorId  | INT           | PK  |
| FullName  | NVARCHAR(150) |     |
| BirthDate | DATE          |     |
| Country   | NVARCHAR(100) |     |

---

### 2. Categories

| Field       | Type          | Key |
| ----------- | ------------- | --- |
| CategoryId  | INT           | PK  |
| Name        | NVARCHAR(100) |     |
| Description | NVARCHAR(500) |     |

---

### 3. Books

| Field           | Type          | Key |
| --------------- | ------------- | --- |
| BookId          | INT           | PK  |
| Title           | NVARCHAR(200) |     |
| ISBN            | NVARCHAR(20)  |     |
| PublishedYear   | INT           |     |
| AvailableCopies | INT           |     |
| AuthorId        | INT           | FK  |
| CategoryId      | INT           | FK  |

---

### 4. Members

| Field       | Type          | Key |
| ----------- | ------------- | --- |
| MemberId    | INT           | PK  |
| FullName    | NVARCHAR(150) |     |
| Email       | NVARCHAR(150) |     |
| PhoneNumber | NVARCHAR(30)  |     |
| JoinDate    | DATE          |     |
| IsActive    | BIT           |     |

---

### 5. BorrowRecords

| Field          | Type         | Key |
| -------------- | ------------ | --- |
| BorrowRecordId | INT          | PK  |
| BookId         | INT          | FK  |
| MemberId       | INT          | FK  |
| BorrowDate     | DATE         |     |
| DueDate        | DATE         |     |
| ReturnDate     | DATE         |     |
| Status         | NVARCHAR(30) |     |

---

## Primary Keys

The database uses a primary key in every table to uniquely identify each record.

* `Authors.AuthorId`
* `Categories.CategoryId`
* `Books.BookId`
* `Members.MemberId`
* `BorrowRecords.BorrowRecordId`

---

## Foreign Keys

The `Books` table contains two foreign keys:

* `AuthorId` references `Authors.AuthorId`
* `CategoryId` references `Categories.CategoryId`

The `BorrowRecords` table contains two foreign keys:

* `BookId` references `Books.BookId`
* `MemberId` references `Members.MemberId`

---

## Relationships

### Author → Books

One author can have many books, while each book belongs to one author.

**Relationship:**

`Author 1 → Many Books`

---

### Category → Books

One category can contain many books, while each book belongs to one category.

**Relationship:**

`Category 1 → Many Books`

---

### Member → BorrowRecords

One member can have many borrowing records over time, while each borrowing record belongs to one member.

**Relationship:**

`Member 1 → Many BorrowRecords`

---

### Book → BorrowRecords

One book can appear in many borrowing records over time, while each borrowing record refers to one book.

**Relationship:**

`Book 1 → Many BorrowRecords`

---

## Why I Designed It This Way

The database is divided into separate tables to avoid storing repeated information and to keep the data organized. Authors and categories are stored independently because they can be associated with many books. The Books table stores the information specific to each book and uses foreign keys to connect it to its author and category. Members are stored separately because one member can borrow books multiple times. BorrowRecords represents each borrowing operation and connects a member with a book while storing the borrowing dates, return date, and status. This design follows relational database principles and makes it easier to query and maintain the library data.

---

## ERD

The Entity Relationship Diagram represents the five main entities and their relationships.

### Relationships Summary

```text
Authors      1 ───────────< Books
Categories   1 ───────────< Books
Books        1 ───────────< BorrowRecords
Members      1 ───────────< BorrowRecords
```

The ERD file is available in:

`ERD/Library-Management-ERD.png`

---

# SQL Queries

## 1. Select All Books

```sql
SELECT *
FROM Books;
```

This query returns all books stored in the library.

---

## 2. Select All Active Members

```sql
SELECT *
FROM Members
WHERE IsActive = 1;
```

This query returns only members whose accounts are currently active.

---

## 3. Select Books by Category

```sql
SELECT
    b.BookId,
    b.Title,
    b.ISBN,
    b.PublishedYear,
    b.AvailableCopies,
    c.Name AS CategoryName
FROM Books b
INNER JOIN Categories c
    ON b.CategoryId = c.CategoryId
WHERE c.Name = 'Programming';
```

This query uses a JOIN to find books that belong to a specific category.

---

## 4. Count Books per Category

```sql
SELECT
    c.CategoryId,
    c.Name AS CategoryName,
    COUNT(b.BookId) AS BookCount
FROM Categories c
LEFT JOIN Books b
    ON c.CategoryId = b.CategoryId
GROUP BY
    c.CategoryId,
    c.Name;
```

This query counts the number of books in every category.

---

## 5. Borrow Records with Member Name and Book Title

```sql
SELECT
    br.BorrowRecordId,
    m.FullName AS MemberName,
    b.Title AS BookTitle,
    br.BorrowDate,
    br.DueDate,
    br.ReturnDate,
    br.Status
FROM BorrowRecords br
INNER JOIN Members m
    ON br.MemberId = m.MemberId
INNER JOIN Books b
    ON br.BookId = b.BookId;
```

This query demonstrates multiple JOIN operations to display meaningful borrowing information instead of only foreign key IDs.

---

## 6. Select Overdue Books

```sql
SELECT
    b.BookId,
    b.Title,
    m.FullName AS MemberName,
    br.DueDate,
    br.Status
FROM BorrowRecords br
INNER JOIN Books b
    ON br.BookId = b.BookId
INNER JOIN Members m
    ON br.MemberId = m.MemberId
WHERE br.DueDate < CAST(GETDATE() AS DATE)
  AND br.ReturnDate IS NULL;
```

This query finds books whose due date has passed and that have not been returned yet.

---

## 7. Borrowing History for One Member

```sql
SELECT
    br.BorrowRecordId,
    b.Title AS BookTitle,
    br.BorrowDate,
    br.DueDate,
    br.ReturnDate,
    br.Status
FROM BorrowRecords br
INNER JOIN Books b
    ON br.BookId = b.BookId
WHERE br.MemberId = 1;
```

This query returns the borrowing history of a specific member.

---

## 8. Select Available Books

```sql
SELECT
    BookId,
    Title,
    AvailableCopies
FROM Books
WHERE AvailableCopies > 0;
```

This query returns books that currently have at least one available copy.

---

## 9. Count Books per Author

```sql
SELECT
    a.AuthorId,
    a.FullName AS AuthorName,
    COUNT(b.BookId) AS BookCount
FROM Authors a
LEFT JOIN Books b
    ON a.AuthorId = b.AuthorId
GROUP BY
    a.AuthorId,
    a.FullName;
```

This query calculates how many books are associated with each author.

---

## 10. Top 5 Most Borrowed Books

```sql
SELECT TOP 5
    b.BookId,
    b.Title,
    COUNT(br.BorrowRecordId) AS BorrowCount
FROM Books b
INNER JOIN BorrowRecords br
    ON b.BookId = br.BookId
GROUP BY
    b.BookId,
    b.Title
ORDER BY
    BorrowCount DESC;
```

This query returns the five books with the highest number of borrowing records.

---

## Design Decisions

The database follows a normalized relational structure where each entity has its own table. Primary keys uniquely identify records, while foreign keys maintain relationships between related entities.

The BorrowRecords table was introduced as a separate table because borrowing is an operation that can happen many times for the same book and member. This also allows the system to keep a complete borrowing history.

The `AvailableCopies` field is stored in the Books table because it represents the current inventory available for borrowing.

The `ReturnDate` can be NULL because a book may still be borrowed and not yet returned.

The `IsActive` field allows the library to deactivate a member without deleting their historical borrowing records.

---

## Required Deliverables

* [x] ERD
* [x] List of tables and fields
* [x] Primary keys
* [x] Foreign keys
* [x] Relationship explanation
* [x] SQL file
* [x] Required SQL queries
* [x] JOIN query
* [x] README
* [x] Design explanation

---

## Files

```text
Task-06-SQL-ERD/
│
├── ERD/
│   └── Library-Management-ERD.png
│
├── SQL/
│   └── Task06_Library_SQL.sql
│
└── README.md
```
