# Task 06 - API Standards & Refactor Pack

## Overview

This task focuses on refactoring a poorly designed ASP.NET Core Web API into a cleaner and more professional structure.

The original API had business logic, validation, and data storage directly inside the controller. It also used incorrect status codes, poor route naming, and public fields instead of properties.

The goal of this refactoring is to improve the API structure while keeping the original functionality.

---

## Original Problems

The original API had several design and implementation problems:

* The controller contained storage and business logic.
* The POST endpoint used query/string parameters instead of a request body DTO.
* Invalid data returned `200 OK` with error text.
* Missing products also returned `200 OK`.
* The `Product` model used public fields instead of properties.
* There was no service layer.
* There were no DTOs for requests and responses.
* The routes were not RESTful.
* The controller was responsible for creating and searching products.
* The API did not have a clear and consistent response structure.

---

## Original Bad Code

The original implementation was kept inside:

```text
OriginalBadCode/
└── ProductsController.cs
```

The original controller handled:

* Product storage
* Product creation
* Validation
* Product searching
* HTTP responses

This made the controller tightly coupled to the business logic and difficult to maintain.

---

# Refactoring Improvements

## 1. Product Model with Properties

The original model used public fields:

```csharp
public string Name;
public decimal Price;
public int Stock;
```

These were replaced with properties:

```csharp
public string Name { get; set; } = string.Empty;
public decimal Price { get; set; }
public int Stock { get; set; }
```

This follows the standard C# model structure used in ASP.NET Core applications.

---

## 2. Added CreateProductRequest DTO

A dedicated request DTO was created:

```text
DTOs/CreateProductRequest.cs
```

It is used to receive product creation data from the client.

Example request:

```json
{
  "name": "Book",
  "price": 100,
  "stock": 5
}
```

This is cleaner than passing multiple parameters directly to the controller action.

---

## 3. Added ProductResponse DTO

A separate response DTO was created:

```text
DTOs/ProductResponse.cs
```

The API returns `ProductResponse` instead of exposing the internal model directly.

This creates a clear API response contract and makes the API easier to change in the future.

---

## 4. Added Service Layer

The business logic was moved from the controller into:

```text
Services/ProductService.cs
```

The service is responsible for:

* Creating products
* Validating product data
* Storing products
* Searching for products
* Mapping products to response DTOs

This keeps the controller focused on handling HTTP requests and responses.

---

## 5. Added IProductService

An interface was created:

```text
Services/IProductService.cs
```

It defines the operations available for products:

```csharp
CreateProduct()
GetAll()
GetById()
```

The controller depends on the abstraction `IProductService` instead of directly depending on `ProductService`.

---

## 6. Moved Validation Out of the Controller

Validation and business rules are handled inside the service.

For example:

```csharp
if (string.IsNullOrWhiteSpace(request.Name))
{
    throw new ArgumentException("Product name is required.");
}
```

And:

```csharp
if (request.Price < 0)
{
    throw new ArgumentException("Product price cannot be negative.");
}
```

This prevents the controller from becoming responsible for business rules.

---

## 7. Improved RESTful Routes

The original routes were:

```text
GET /api/products/all
GET /api/products/get?id=1
POST /api/products
```

They were replaced with RESTful routes:

```text
GET  /api/products
GET  /api/products/{id}
POST /api/products
```

The route structure now represents the `products` resource clearly.

---

## 8. Corrected HTTP Status Codes

The original API returned `200 OK` even when the request contained invalid data or the product did not exist.

The refactored API uses appropriate status codes:

### `200 OK`

Used when retrieving products successfully.

```http
GET /api/products
```

### `201 Created`

Used when a new product is successfully created.

```http
POST /api/products
```

### `400 Bad Request`

Used when the client sends invalid product data.

Example:

```json
{
  "name": "",
  "price": -10,
  "stock": 5
}
```

### `404 Not Found`

Used when the requested product does not exist.

Example:

```http
GET /api/products/999
```

---

## 9. Cleaner Controller

The controller is now responsible mainly for:

* Receiving HTTP requests
* Calling the service
* Returning appropriate HTTP responses

Example:

```csharp
[HttpGet("{id:int}")]
public IActionResult GetById(int id)
{
    var product = _productService.GetById(id);

    if (product is null)
    {
        return NotFound(new
        {
            message = "Product not found."
        });
    }

    return Ok(product);
}
```

The controller no longer manages the product list or contains the product business logic.

---

## 10. Dependency Injection

`IProductService` is registered in `Program.cs`:

```csharp
builder.Services.AddSingleton<IProductService, ProductService>();
```

ASP.NET Core Dependency Injection then provides the service to `ProductsController`.

This reduces coupling and makes the application easier to test and maintain.

---

# Before vs After

| Before                           | After                                |
| -------------------------------- | ------------------------------------ |
| Business logic inside Controller | Business logic inside Service        |
| No service layer                 | `IProductService` + `ProductService` |
| Public fields                    | C# properties                        |
| No DTOs                          | Request and Response DTOs            |
| Query parameters for POST        | JSON request body                    |
| `200 OK` for validation errors   | `400 Bad Request`                    |
| `200 OK` for missing product     | `404 Not Found`                      |
| `200 OK` for creation            | `201 Created`                        |
| `/all` and `/get` routes         | RESTful routes                       |
| Controller manages storage       | Service manages storage              |
| Product model returned directly  | `ProductResponse` returned           |

---

# API Endpoints

| Method | Endpoint             | Description       | Success Status |
| ------ | -------------------- | ----------------- | -------------- |
| POST   | `/api/products`      | Create a product  | `201 Created`  |
| GET    | `/api/products`      | Get all products  | `200 OK`       |
| GET    | `/api/products/{id}` | Get product by ID | `200 OK`       |

Possible error responses:

| Status Code       | Description            |
| ----------------- | ---------------------- |
| `400 Bad Request` | Invalid product data   |
| `404 Not Found`   | Product does not exist |

---

# Example Requests

## Create Product

### Request

```http
POST /api/products
Content-Type: application/json
```

```json
{
  "name": "ASP.NET Core Book",
  "price": 250,
  "stock": 10
}
```

### Response

```http
201 Created
```

```json
{
  "id": 1,
  "name": "ASP.NET Core Book",
  "price": 250,
  "stock": 10
}
```

---

## Get All Products

```http
GET /api/products
```

Response:

```http
200 OK
```

```json
[
  {
    "id": 1,
    "name": "ASP.NET Core Book",
    "price": 250,
    "stock": 10
  }
]
```

---

## Get Product By ID

```http
GET /api/products/1
```

Response:

```http
200 OK
```

---

## Product Not Found

```http
GET /api/products/999
```

Response:

```http
404 Not Found
```

```json
{
  "message": "Product not found."
}
```

---

## Invalid Product

```http
POST /api/products
```

```json
{
  "name": "",
  "price": -10,
  "stock": 5
}
```

Response:

```http
400 Bad Request
```

---

# Project Structure

```text
task-06-api-standards-refactor-pack/
│
├── README.md
│
├── OriginalBadCode/
│   └── ProductsController.cs
│
└── RefactoredApi/
    │
    ├── Controllers/
    │   └── ProductsController.cs
    │
    ├── DTOs/
    │   ├── CreateProductRequest.cs
    │   └── ProductResponse.cs
    │
    ├── Models/
    │   └── Product.cs
    │
    ├── Services/
    │   ├── IProductService.cs
    │   └── ProductService.cs
    │
    ├── Program.cs
    └── RefactoredApi.csproj
```

---

# What I Learned

Through this task, I learned how to recognize common problems in poorly designed APIs and refactor them into a cleaner structure. I learned why controllers should not contain business logic or data storage. I also learned how DTOs can separate API requests and responses from internal models. Another important lesson was using the correct HTTP status codes instead of returning `200 OK` for every situation. I learned how RESTful routes make API endpoints clearer and easier to understand. Finally, using a service interface with dependency injection makes the application more organized, maintainable, and easier to test.

---

# Testing

The API was tested using Swagger to verify:

* Product creation
* Get all products
* Get product by ID
* Invalid product validation
* Product not found scenario
* Correct HTTP status codes
* Correct request and response structures

Screenshots can be added to the repository under:

```text
Screenshots/
```

Recommended screenshots:

```text
Screenshots/
├── create-product-201.png
├── get-all-200.png
├── get-by-id-200.png
├── invalid-product-400.png
└── product-not-found-404.png
```

---

# Conclusion

The API was successfully refactored from a controller-heavy implementation into a cleaner structure using Models, DTOs, Services, Interfaces, Dependency Injection, RESTful routes, and appropriate HTTP status codes.

The original functionality was preserved while improving code organization, readability, maintainability, and API design standards.
