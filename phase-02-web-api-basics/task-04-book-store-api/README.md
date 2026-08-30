# 📚 Book Store API

A RESTful Web API mini-project built with **ASP.NET Core** as part of **TechMaster Academy – Phase 02**.

The project simulates a simple Book Store system that manages **Books, Authors, and Categories** using in-memory data instead of a database.

The API is structured with separation of concerns using **Controllers, DTOs, Services, and Models**, making it easier to migrate to **EF Core and a relational database in Phase 03**.

---

## 📌 Project Overview

The Book Store API provides endpoints to:

* Manage authors.
* Manage categories.
* Manage books.
* Search and filter books.
* Paginate book results.
* Validate business rules.
* Generate management summary reports.
* Return appropriate HTTP status codes.
* Document and test the API using Swagger and Postman.

---

## 🛠️ Technologies

* **C#**
* **ASP.NET Core Web API**
* **.NET**
* **RESTful API**
* **Swagger / OpenAPI**
* **Postman**
* **In-Memory Data Storage**
* **Data Annotations Validation**
* **Dependency Injection**
* **LINQ**

---

## 🏗️ Project Structure

```text
task-04-book-store-api/
│
├── README.md
│
└── BookStoreApi/
    │
    ├── Controllers/
    │   ├── BooksController.cs
    │   ├── AuthorsController.cs
    │   └── CategoriesController.cs
    │
    ├── Data/
    │   └── InMemoryStore.cs
    │
    ├── Models/
    │   ├── Book.cs
    │   ├── Author.cs
    │   └── Category.cs
    │
    ├── DTOs/
    │   ├── CreateAuthorRequest.cs
    │   ├── CreateCategoryRequest.cs
    │   ├── CreateBookRequest.cs
    │   ├── UpdateBookRequest.cs
    │   ├── AuthorResponse.cs
    │   ├── CategoryResponse.cs
    │   ├── BookResponse.cs
    │   └── BookSummaryResponse.cs
    │
    ├── Services/
    │   ├── IAuthorService.cs
    │   ├── AuthorService.cs
    │   ├── ICategoryService.cs
    │   ├── CategoryService.cs
    │   ├── IBookService.cs
    │   └── BookService.cs
    │
    └── Program.cs
```

---

# 🧩 Architecture

The project follows a simple layered structure:

```text
Client
   ↓
Controllers
   ↓
Services
   ↓
InMemoryStore
   ↓
Models
```

### Controllers

Controllers are responsible for:

* Receiving HTTP requests.
* Calling the appropriate service.
* Returning HTTP responses.
* Handling HTTP status codes.

Business logic is kept outside the controllers to prevent them from becoming too large.

### Services

Services contain the application's business logic, including:

* Validation of related entities.
* Duplicate checking.
* Search and filtering.
* Pagination.
* Book availability logic.
* Report calculations.

### DTOs

DTOs are used to separate the API contract from the domain models.

Different DTOs are used for different operations:

* Create requests.
* Update requests.
* Response objects.
* Report responses.

This prevents clients from directly controlling internal model properties such as `BookId` and `CreatedAt`.

### Models

Models represent the main domain entities:

* Book
* Author
* Category

---

# 📦 Domain Models

## Book

```text
BookId
Title
ISBN
PublishedYear
Price
StockQuantity
AuthorId
CategoryId
IsAvailable
CreatedAt
```

## Author

```text
AuthorId
FullName
Country
BirthDate
CreatedAt
```

## Category

```text
CategoryId
Name
Description
IsActive
```

### Relationships

```text
Author 1 ───────── * Book

Category 1 ─────── * Book
```

A single author can have multiple books, and a category can contain multiple books.

The relationship structure is designed to make the transition to EF Core easier in Phase 03.

---

# 🔌 API Endpoints

## Authors

| Method | Endpoint            | Description         |
| ------ | ------------------- | ------------------- |
| GET    | `/api/authors`      | Get all authors     |
| GET    | `/api/authors/{id}` | Get author by ID    |
| POST   | `/api/authors`      | Create a new author |

### Create Author

```http
POST /api/authors
```

Request:

```json
{
  "fullName": "Andrew Hunt",
  "country": "USA",
  "birthDate": "1964-02-02"
}
```

Response:

```text
201 Created
```

---

# 🗂️ Categories

| Method | Endpoint               | Description           |
| ------ | ---------------------- | --------------------- |
| GET    | `/api/categories`      | Get all categories    |
| GET    | `/api/categories/{id}` | Get category by ID    |
| POST   | `/api/categories`      | Create a new category |

### Create Category

```http
POST /api/categories
```

Request:

```json
{
  "name": "Database",
  "description": "Books about databases and SQL.",
  "isActive": true
}
```

Response:

```text
201 Created
```

---

# 📚 Books

| Method | Endpoint                     | Description                                     |
| ------ | ---------------------------- | ----------------------------------------------- |
| GET    | `/api/books`                 | Get books with search, filtering and pagination |
| GET    | `/api/books/{id}`            | Get book by ID                                  |
| POST   | `/api/books`                 | Create a book                                   |
| PUT    | `/api/books/{id}`            | Update a book                                   |
| DELETE | `/api/books/{id}`            | Mark a book as unavailable                      |
| GET    | `/api/books/reports/summary` | Get book store summary                          |

---

## 🔎 Search

Books can be searched by:

* Title
* ISBN

Example:

```http
GET /api/books?search=clean
```

or:

```http
GET /api/books?search=9780132350884
```

---

## 🎯 Filtering

Books can be filtered by:

### Category

```http
GET /api/books?categoryId=2
```

### Author

```http
GET /api/books?authorId=1
```

### Availability

```http
GET /api/books?isAvailable=true
```

---

## 📄 Pagination

Pagination is supported using:

* `pageNumber`
* `pageSize`

Example:

```http
GET /api/books?pageNumber=1&pageSize=5
```

Maximum page size is limited to **100**.

Invalid pagination values return:

```text
400 Bad Request
```

---

## 🔍 Combined Search, Filtering and Pagination

Multiple query parameters can be combined.

Example:

```http
GET /api/books?search=clean&categoryId=1&authorId=1&isAvailable=true&pageNumber=1&pageSize=5
```

---

# ➕ Create Book

```http
POST /api/books
```

Request:

```json
{
  "title": "The Pragmatic Programmer",
  "isbn": "9780135957059",
  "publishedYear": 2019,
  "price": 50,
  "stockQuantity": 8,
  "authorId": 1,
  "categoryId": 3
}
```

If creation succeeds:

```text
201 Created
```

The API automatically determines availability based on stock quantity.

---

# ✏️ Update Book

```http
PUT /api/books/{id}
```

Example:

```json
{
  "title": "Clean Code Updated",
  "isbn": "9780132350884",
  "publishedYear": 2008,
  "price": 49.99,
  "stockQuantity": 15,
  "authorId": 1,
  "categoryId": 1,
  "isAvailable": true
}
```

---

# 🗑️ Delete Book

```http
DELETE /api/books/{id}
```

The API uses a **soft-delete approach** by marking the book as unavailable instead of physically removing it from the in-memory collection.

Successful operation:

```text
204 No Content
```

This approach keeps the book record available for future database implementation and reporting.

---

# 📊 Reports Summary

Endpoint:

```http
GET /api/books/reports/summary
```

The report provides:

* Total books.
* Available books.
* Out-of-stock books.
* Books per category.
* Books per author.
* Total inventory value.

Example response:

```json
{
  "totalBooks": 3,
  "availableBooks": 2,
  "outOfStockBooks": 1,
  "totalInventoryValue": 696.89,
  "booksPerCategory": [
    {
      "categoryId": 1,
      "categoryName": "Software Engineering",
      "bookCount": 1
    },
    {
      "categoryId": 2,
      "categoryName": "Architecture",
      "bookCount": 2
    }
  ],
  "booksPerAuthor": [
    {
      "authorId": 1,
      "authorName": "Robert C. Martin",
      "bookCount": 1
    },
    {
      "authorId": 2,
      "authorName": "Martin Fowler",
      "bookCount": 1
    }
  ]
}
```

### Inventory Value

The total inventory value is calculated as:

```text
Price × StockQuantity
```

for each book.

---

# ✅ Validation & Business Rules

## Authors

* `FullName` is required.
* Author IDs are unique.
* Authors are represented separately from books.

## Categories

* Category name is required.
* Category names must be unique.
* Category names are checked case-insensitively.
* Inactive categories cannot be assigned to new books.

## Books

* Title is required.
* ISBN is required.
* ISBN must be unique.
* Price must be greater than zero.
* Stock quantity cannot be negative.
* Author must exist.
* Category must exist.
* Category must be active when assigning it to a book.

---

# 🌐 HTTP Status Codes

The API uses appropriate HTTP status codes:

| Status Code       | Usage                                        |
| ----------------- | -------------------------------------------- |
| `200 OK`          | Successful GET / UPDATE                      |
| `201 Created`     | Resource successfully created                |
| `204 No Content`  | Book successfully marked unavailable         |
| `400 Bad Request` | Invalid request or validation error          |
| `404 Not Found`   | Requested resource does not exist            |
| `409 Conflict`    | Duplicate ISBN/category or inactive category |

---

# 💉 Dependency Injection

Services are registered using ASP.NET Core Dependency Injection:

```csharp
builder.Services.AddSingleton<InMemoryStore>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBookService, BookService>();
```

`InMemoryStore` is registered as a Singleton so all services work with the same in-memory data.

Services are registered as Scoped to keep the application structure ready for future database-based implementations.

---

# 🧪 Testing

The API can be tested using:

* Swagger UI
* Postman

Swagger is available when the application is running:

```text
/swagger
```

### Main test scenarios

The following scenarios were tested:

* Create author successfully.
* Create category successfully.
* Create book successfully.
* Create book with invalid author.
* Create book with invalid category.
* Create book using inactive category.
* Create book with duplicate ISBN.
* Search books by title.
* Search books by ISBN.
* Filter books by category.
* Filter books by author.
* Filter books by availability.
* Test pagination.
* Get book by ID.
* Update book.
* Mark book unavailable.
* Generate summary report.
* Validate invalid input.

---

# 📝 Postman

A Postman collection is used to demonstrate the API functionality.

The collection covers:

```text
Authors
Categories
Books
Search
Filtering
Pagination
Reports
Validation
Error cases
```

---

# 🖼️ Evidence

Screenshots can be added to this section to demonstrate the implemented features.

Recommended evidence:

### Swagger

```text
docs/screenshots/swagger.png
```

### Create Author

```text
docs/screenshots/create-author.png
```

### Create Category

```text
docs/screenshots/create-category.png
```

### Create Book

```text
docs/screenshots/create-book.png
```

### Validation Error

```text
docs/screenshots/validation-error.png
```

### Search / Filter / Pagination

```text
docs/screenshots/search-filter-pagination.png
```

### Reports

```text
docs/screenshots/report-summary.png
```

---

# 🚀 How to Run

### 1. Clone the repository

```bash
git clone <repository-url>
```

### 2. Navigate to the project

```bash
cd task-04-book-store-api/BookStoreApi
```

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Run the API

```bash
dotnet run
```

### 5. Open Swagger

Open the Swagger URL displayed by the application, for example:

```text
https://localhost:<port>/swagger
```

---

# 🔮 Future Improvements

The current version intentionally uses in-memory data because this is a **Phase 02 project**.

In Phase 03, the application can be extended with:

* Entity Framework Core.
* SQL Server.
* DbContext.
* EF Core relationships.
* Migrations.
* Repository pattern.
* Database indexes.
* Unique database constraints.
* Async database operations.
* Pagination at database level.
* Authentication and authorization.
* Global exception handling.
* Logging.

The existing separation between Controllers, Services, DTOs, and Models makes this migration easier without significantly changing the API contract.

---

# 🎯 Learning Objectives

This project demonstrates practical understanding of:

* RESTful API design.
* Route naming.
* HTTP methods.
* HTTP status codes.
* DTOs.
* Model validation.
* Dependency Injection.
* Service layer.
* Business rules.
* Entity relationships.
* Search and filtering.
* Pagination.
* LINQ.
* Reporting.
* Swagger documentation.
* Postman API testing.
* Preparing an API for future EF Core integration.

---

# 📋 Acceptance Criteria

| Requirement                     | Status |
| ------------------------------- | ------ |
| All required resources exist    | ✅      |
| DTOs are used                   | ✅      |
| Books CRUD implemented          | ✅      |
| Authors API implemented         | ✅      |
| Categories API implemented      | ✅      |
| Book relationships implemented  | ✅      |
| Book validation implemented     | ✅      |
| Author validation implemented   | ✅      |
| Category validation implemented | ✅      |
| Unique ISBN                     | ✅      |
| Unique category name            | ✅      |
| Inactive category protection    | ✅      |
| Search by title / ISBN          | ✅      |
| Category filtering              | ✅      |
| Author filtering                | ✅      |
| Availability filtering          | ✅      |
| Pagination                      | ✅      |
| Summary report                  | ✅      |
| Correct HTTP status codes       | ✅      |
| Swagger documentation           | ✅      |
| Postman testing                 | ✅      |
| Evidence screenshots            | ⏳      |
| Demo video                      | ⏳      |

---

# 🎥 Demo Video

The planned demo covers:

1. Run the API and open Swagger.
2. Create an author.
3. Create a category.
4. Create a book.
5. Demonstrate validation error.
6. Demonstrate search.
7. Demonstrate filtering.
8. Demonstrate pagination.
9. Show the summary report.
10. Explain the folder structure and the role of DTOs and Services.

**Target duration:** 3–6 minutes.

---

## 👩‍💻 Project

**TechMaster Academy – ASP.NET Backend Career Training**

**Phase 02 – Task 04: Book Store API Mini-Project**
