# 🛒 Products & Categories API — Task 03

A clean, in-memory RESTful CRUD API built with **ASP.NET Core** to simulate a small store management system for handling products, categories, stock visibility, filtering, and stock reports.

---

## 📌 Project Overview

This project is part of **TechMaster Academy – ASP.NET Backend Career Training, Phase 02**.

The API manages:

* Product categories.
* Store products.
* Product-category relationships.
* Product search and filtering.
* Stock management.
* Low-stock monitoring.
* Stock value reports.
* Category-based product statistics.

The project uses **in-memory data storage** instead of a database to focus on API design, business logic, validation, LINQ, and service-layer architecture.

---

# 🏗️ Architecture & Design

The project follows a simple **N-Tier / Clean Architecture-inspired structure** to keep responsibilities separated.

```text
Client
   │
   ▼
Controllers
   │
   ▼
Services
   │
   ▼
InMemoryStore
   │
   ├── Categories
   └── Products
```

### Controllers

Responsible for:

* Receiving HTTP requests.
* Route mapping.
* Reading query parameters and request bodies.
* Calling the appropriate service.
* Returning appropriate HTTP status codes.

Controllers do not contain business logic.

### Services

Responsible for:

* Business rules.
* Product and category operations.
* Category validation.
* Searching and filtering.
* Stock calculations.
* LINQ-based reporting.
* Mapping Models to Response DTOs.

### DTOs

DTOs are used to control the data entering and leaving the API.

Examples:

* `CreateProductRequest`
* `UpdateProductRequest`
* `UpdateProductStockRequest`
* `CreateCategoryRequest`
* `ProductResponse`
* `CategoryResponse`
* `StockReportResponse`

### Models

Represent the internal domain data:

* `Product`
* `Category`

### InMemoryStore

Provides shared in-memory storage for:

* Categories.
* Products.
* Auto-incrementing IDs.
* Seed data.

---

# 📁 Project Structure

```text
task-03-products-categories-api/
│
├── README.md
│
└── ProductsCategoriesApi/
    │
    ├── Controllers/
    │   ├── ProductsController.cs
    │   └── CategoriesController.cs
    │
    ├── Data/
    │   └── InMemoryStore.cs
    │
    ├── DTOs/
    │   ├── CreateProductRequest.cs
    │   ├── UpdateProductRequest.cs
    │   ├── UpdateProductStockRequest.cs
    │   ├── ProductResponse.cs
    │   ├── CreateCategoryRequest.cs
    │   ├── CategoryResponse.cs
    │   └── StockReportResponse.cs
    │
    ├── Models/
    │   ├── Product.cs
    │   └── Category.cs
    │
    ├── Services/
    │   ├── IProductService.cs
    │   ├── ProductService.cs
    │   ├── ICategoryService.cs
    │   └── CategoryService.cs
    │
    ├── Program.cs
    └── ProductsCategoriesApi.csproj
```

---

# 🗂️ Domain Models

## Category

A category represents a group of products.

| Property    | Type     | Description                              |
| ----------- | -------- | ---------------------------------------- |
| CategoryId  | int      | Unique category identifier               |
| Name        | string   | Category name                            |
| Description | string   | Category description                     |
| IsActive    | bool     | Indicates whether the category is active |
| CreatedAt   | DateTime | Category creation date                   |

---

## Product

A product belongs to a category.

| Property      | Type     | Description                 |
| ------------- | -------- | --------------------------- |
| ProductId     | int      | Unique product identifier   |
| Name          | string   | Product name                |
| CategoryId    | int      | Related category identifier |
| Price         | decimal  | Product price               |
| StockQuantity | int      | Available stock quantity    |
| IsAvailable   | bool     | Product availability status |
| SupplierName  | string   | Product supplier            |
| CreatedAt     | DateTime | Product creation date       |

### Relationship

```text
Category 1 ─────────── * Product
```

Each Product belongs to one Category, while a Category can contain multiple Products.

---

# ⚙️ Business Rules

## Category Rules

* Category name is required.
* Category name must be unique.
* Inactive categories should not appear by default.
* Products cannot be created under an inactive category.
* A category with products should not be physically deleted; it should be blocked or deactivated.

## Product Rules

* Product name is required.
* Price must be greater than zero.
* Stock quantity cannot be negative.
* Category must exist before creating a product.
* Product must reference an active category.
* Missing products return `404 Not Found`.
* Invalid input returns `400 Bad Request`.

---

# 🚀 API Endpoints

## Categories

| Method | Route             | Description               |
| ------ | ----------------- | ------------------------- |
| GET    | `/api/categories` | Get all active categories |
| POST   | `/api/categories` | Create a new category     |

---

## Products

| Method | Route                               | Description                 |
| ------ | ----------------------------------- | --------------------------- |
| GET    | `/api/products`                     | Get products with filtering |
| GET    | `/api/products/{id}`                | Get product by ID           |
| POST   | `/api/products`                     | Create a product            |
| PUT    | `/api/products/{id}`                | Update a product            |
| PATCH  | `/api/products/{id}/stock`          | Update product stock        |
| DELETE | `/api/products/{id}`                | Mark product as unavailable |
| GET    | `/api/products/low-stock`           | Get low-stock products      |
| GET    | `/api/products/reports/stock-value` | Get stock reports           |

---

# 🔎 Product Search & Filtering

The main products endpoint supports multiple filters:

```http
GET /api/products
```

### Available Query Parameters

| Parameter     | Description                    |
| ------------- | ------------------------------ |
| `search`      | Search products by name        |
| `categoryId`  | Filter by category             |
| `minPrice`    | Minimum product price          |
| `maxPrice`    | Maximum product price          |
| `isAvailable` | Filter by availability         |
| `lowStock`    | Return products with low stock |

### Example

```http
GET /api/products?search=laptop
```

### Combined Filters

Multiple filters can be used together:

```http
GET /api/products?categoryId=1&minPrice=1000&maxPrice=10000&isAvailable=true
```

The filters are applied together to produce the final result.

---

# 📦 Create Product

### Request

```http
POST /api/products
```

Example body:

```json
{
  "name": "Gaming Laptop",
  "categoryId": 1,
  "price": 55000,
  "stockQuantity": 10,
  "isAvailable": true,
  "supplierName": "Tech Supplier"
}
```

Before creating the product, the service verifies that the requested category:

1. Exists.
2. Is active.

If the category does not exist or is inactive:

```http
400 Bad Request
```

Example response:

```json
{
  "message": "Category does not exist or is inactive."
}
```

If successful:

```http
201 Created
```

---

# 🔍 Get Product By ID

```http
GET /api/products/{id}
```

Example:

```http
GET /api/products/1
```

If the product exists:

```http
200 OK
```

If it does not exist:

```http
404 Not Found
```

Example:

```json
{
  "message": "Product with id 99 was not found."
}
```

---

# ✏️ Update Product

```http
PUT /api/products/{id}
```

Example:

```json
{
  "name": "Gaming Laptop",
  "categoryId": 1,
  "price": 60000,
  "stockQuantity": 8,
  "isAvailable": true,
  "supplierName": "Updated Supplier"
}
```

The update validates that the selected category exists and is active.

Immutable information such as `ProductId` and `CreatedAt` is not modified.

---

# 📊 Update Stock

Stock can be updated independently without sending the entire product object.

```http
PATCH /api/products/{id}/stock
```

Example:

```json
{
  "stockQuantity": 25
}
```

Negative stock values are rejected through DTO validation.

---

# 🗑️ Delete Product

```http
DELETE /api/products/{id}
```

Instead of physically removing the product from memory, the API marks it as unavailable:

```text
IsAvailable = false
```

This keeps the product data while preventing it from being considered available for sale.

---

# ⚠️ Low Stock

```http
GET /api/products/low-stock
```

The application considers a product to be low stock when:

```text
StockQuantity <= 5
```

This threshold is defined in the service:

```csharp
private const int LowStockThreshold = 5;
```

The same rule is also available through the filtering endpoint:

```http
GET /api/products?lowStock=true
```

---

# 📈 Stock Reports

```http
GET /api/products/reports/stock-value
```

The report provides five important business metrics.

### 1. Total Stock Value

Calculated using:

```text
Price × StockQuantity
```

for all products.

### 2. Stock Value By Category

Calculates the total inventory value for each category.

### 3. Low Stock Products

Returns products where:

```text
StockQuantity <= 5
```

### 4. Out Of Stock Products

Returns products where:

```text
StockQuantity = 0
```

### 5. Products Count By Category

Returns the number of products belonging to each category.

The reports are calculated using **LINQ**.

---

# 📊 Example Stock Report

```json
{
  "totalStockValue": 500000,
  "stockValueByCategory": [
    {
      "categoryId": 1,
      "categoryName": "Electronics",
      "stockValue": 350000
    },
    {
      "categoryId": 2,
      "categoryName": "Furniture",
      "stockValue": 100000
    }
  ],
  "lowStockProducts": [],
  "outOfStockProducts": [],
  "productsCountByCategory": [
    {
      "categoryId": 1,
      "categoryName": "Electronics",
      "productCount": 5
    }
  ]
}
```

---

# 🌱 Seed Data

The application contains predefined in-memory data for testing search, filters, low-stock scenarios, and reports.

### Categories

At least four categories are seeded:

```text
1. Electronics
2. Furniture
3. Stationery
4. Accessories
```

### Products

At least 15 products are seeded.

Examples:

### Electronics

```text
Laptop
Mouse
Keyboard
Monitor
USB-C Hub
```

### Furniture

```text
Office Chair
Desk
Desk Lamp
```

### Stationery

```text
Notebook
Pen Set
Marker
Paper Pack
```

### Accessories

```text
Backpack
Mouse Pad
Laptop Sleeve
```

The seed data intentionally contains different prices, stock quantities, availability states, and categories to make filtering and reporting easy to test.

---

# 🧪 Testing

## 1. Run the Application

Navigate to the project directory:

```bash
cd task-03-products-categories-api/ProductsCategoriesApi
```

Run the application:

```bash
dotnet run
```

---

## 2. Swagger

Open:

```text
https://localhost:<port>/swagger
```

Swagger can be used to test all available endpoints.

---

## 3. Postman

The API can also be tested using Postman.

Recommended test cases:

### Categories

* Get all categories.
* Create a new category.
* Try creating a duplicate category.

### Products

* Get all products.
* Search by product name.
* Filter by category.
* Filter by price range.
* Filter by availability.
* Combine multiple filters.
* Get product by ID.
* Create a product with a valid category.
* Create a product with an invalid category.
* Update a product.
* Update stock.
* Try negative stock.
* Delete/mark a product unavailable.
* Get low-stock products.
* Get stock reports.

---

# ✅ Expected HTTP Status Codes

| Scenario           | Status Code       |
| ------------------ | ----------------- |
| Successful GET     | `200 OK`          |
| Successful POST    | `201 Created`     |
| Successful PUT     | `200 OK`          |
| Successful PATCH   | `200 OK`          |
| Successful DELETE  | `200 OK`          |
| Invalid request    | `400 Bad Request` |
| Resource not found | `404 Not Found`   |

The API avoids returning `200 OK` for resources that do not exist.

---

# 🧠 LINQ Usage

LINQ is used throughout the service layer for:

* Searching.
* Filtering.
* Counting.
* Grouping.
* Calculating stock values.
* Finding low-stock products.
* Finding out-of-stock products.
* Generating category-based statistics.

Examples:

```csharp
products.Where(...)
```

```csharp
products.FirstOrDefault(...)
```

```csharp
products.Count(...)
```

```csharp
products.Sum(...)
```

```csharp
products.Select(...)
```

---

# 🛡️ Validation

The API uses **Data Annotations** inside request DTOs.

Examples:

```csharp
[Required]
public string Name { get; set; }
```

```csharp
[Range(0.01, double.MaxValue)]
public decimal Price { get; set; }
```

```csharp
[Range(0, int.MaxValue)]
public int StockQuantity { get; set; }
```

ASP.NET Core automatically validates the request models through `[ApiController]`.

---

# 🎯 Acceptance Criteria

The implementation satisfies the main Task 03 requirements:

* [x] At least 4 categories exist as seed data.
* [x] At least 15 products exist as seed data.
* [x] Category validation exists.
* [x] Product CRUD operations are implemented.
* [x] Product-category relationship is implemented.
* [x] Product search is supported.
* [x] Category filtering is supported.
* [x] Minimum and maximum price filtering is supported.
* [x] Availability filtering is supported.
* [x] Low-stock filtering is supported.
* [x] Low-stock endpoint is implemented.
* [x] Stock update endpoint is implemented.
* [x] Stock reports are implemented.
* [x] Reports use LINQ.
* [x] DTOs are used for API requests and responses.
* [x] Business logic is kept inside services.
* [x] Appropriate HTTP status codes are returned.
* [x] Swagger testing is supported.
* [x] Postman testing is supported.

---

# 🏁 Task Status

**Task 03 – Products & Categories API: Completed**

The project demonstrates a stronger ASP.NET Core API with:

```text
Controllers
    ↓
Services
    ↓
Business Rules
    ↓
In-Memory Data
```

It also introduces related resources, cross-resource validation, advanced filtering, stock management, and LINQ-based business reporting before moving to database-backed APIs.
