# Task 05: Debug & Refactor Pack

## Overview
This task focuses on debugging, analyzing, and refactoring a legacy console application that handles order calculations and invoice summaries.

---

## Code Comparison (Before vs After)

### Before Refactoring (Original Bad Code)
* **Monolithic Structure:** All business logic, input parsing, tax, discount, and shipping calculations were inside a single `Main` method.
* **Magic Numbers & Hardcoded Values:** Floating magic numbers like `0.05`, `0.10`, `0.14`, and `50` were spread directly in code logic.
* **Primitive Obsession:** Customer types were tracked using simple string literals (`"Regular"`, `"Silver"`, etc.), leading to potential casing and parsing bugs.
* **Data Types:** Used `double` for currency operations, which can lead to floating-point precision issues.
* **Poor Naming Conventions:** Ambiguous variable names (`c`, `p`, `pr`, `q`, `t`).

### After Refactoring (Clean Code)
* **Separation of Concerns:** Separated data structures (`Customer`, `Order`), business logic (`OrderCalculator`), and console interaction (`Program`).
* **Strongly Typed Enums:** Converted customer types to a dedicated `CustomerType` enum.
* **Financial Accuracy:** Converted all monetary calculations to use `decimal`.
* **Constants:** Replaced magic numbers with readable `const` fields (`TaxRate`, `ShippingFee`, `FreeShippingLimit`).
* **Domain Models:** Enforced data integrity and basic validation within domain model constructors.

---

## Project Structure
```text
task-05-debug-refactor-pack/
├── README.md
├── original-bad-code/
│   └── Program.cs
└── refactored-code/
    ├── Enums/
    │   └── CustomerType.cs
    ├── Models/
    │   ├── Customer.cs
    │   └── Order.cs
    ├── Services/
    │   └── OrderCalculator.cs
    └── Program.cs
