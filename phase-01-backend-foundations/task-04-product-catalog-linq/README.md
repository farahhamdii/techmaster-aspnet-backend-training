# Task 04 - Product Catalog with LINQ


## Overview


Product Catalog is a C# Console Application that demonstrates how LINQ can be used to solve common backend business requirements.


The application allows the manager to search, filter, sort, group and analyze products using LINQ.


The project also demonstrates concepts that are commonly used later with Entity Framework Core and ASP.NET Core Web APIs.


---


## Project Structure


```text
task-04-product-catalog/
│
├── README.md
│
└── ProductCatalog/
    │
    ├── Models/
    │   ├── Product.cs
    │   ├── ProductSummary.cs
    │   ├── SupplierReport.cs
    │   └── CategoryStats.cs
    │
    ├── Services/
    │   └── ProductQueryService.cs
    │
    ├── UI/
    │   └── ConsoleMenu.cs
    │
    ├── Program.cs
    └── ProductCatalog.csproj
Technologies
C#
.NET
LINQ
Object-Oriented Programming
Collections
Console Application
Seed Data

The application contains 25 products distributed across different categories and suppliers.

Each product contains:

ProductId
Name
Category
Price
StockQuantity
CreatedAt
IsAvailable
SupplierName

The dataset contains available, unavailable, low-stock and out-of-stock products to make the LINQ queries meaningful.

Features

The application implements 20 LINQ queries:

Get all available products
Filter products by category
Filter products by price range
Search products by name
Sort products by price ascending
Sort products by price descending
Group products by category
Count products per category
Calculate total stock value
Calculate stock value per category
Get top 5 most expensive products
Get low-stock products
Get out-of-stock products
Product summary projection
Supplier report
Recently added products
Category statistics
Products above average price
Combined search and filtering
Pagination using Skip and Take
Main Menu
====== Product Catalog LINQ System ======
1. View Available Products
2. Filter by Category
3. Filter by Price Range
4. Search by Name
5. Sort by Price
6. Group by Category
7. Stock Value Reports
8. Low Stock Products
9. Supplier Report
10. Pagination Demo
11. Exit
LINQ Query Explanations
Query 01 - Get Available Products

This query uses Where to return only products where IsAvailable is true.

_products.Where(p => p.IsAvailable).ToList();

This is useful when displaying products that can currently be offered to customers.

Query 14 - Product Summary Projection

This query uses Select to create a new ProductSummary object instead of returning the complete Product object.

_products.Select(p => new ProductSummary
{
    ProductId = p.ProductId,
    Name = p.Name,
    Price = p.Price,
    StockQuantity = p.StockQuantity
}).ToList();

This is called projection and is similar to creating DTOs in ASP.NET Core Web APIs.

Query 20 - Pagination

This query uses Skip and Take to simulate pagination.

_products
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();

Skip ignores the products from previous pages, while Take returns only the number of products required for the current page.

This is an important concept for backend APIs that return large amounts of data.

Validation

The application validates important inputs:

Category search is case-insensitive.
Product name search is case-insensitive.
Empty search keywords are rejected.
Negative prices are rejected.
Maximum price cannot be lower than minimum price.
Page number must be greater than zero.
Page size must be greater than zero.
Architecture

The project separates responsibilities into three main parts.

Models

Contains the product and report models.

Services

ProductQueryService contains all business queries and LINQ operations.

UI

ConsoleMenu handles user interaction and displays the results.

Program.cs is responsible for creating the service and starting the application.

Business logic and LINQ queries are not placed directly inside Program.cs.

LINQ Concepts Practiced

The project demonstrates:

Where
Contains
OrderBy
OrderByDescending
GroupBy
Count
Sum
Average
Max
Min
Select
Take
Skip
Chained Where queries
DateTime filtering
Projection
Pagination
How to Run

Navigate to the ProductCatalog folder and run:
