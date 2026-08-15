using EmployeeManagement.Models;

namespace EmployeeManagement.Services;

public class EmployeeService
{
    private readonly List<Employee> _employees = new();

    private int _nextEmployeeNumber = 13;

    public EmployeeService()
    {
        SeedEmployees();
    }

    private void SeedEmployees()
    {
        _employees.AddRange(new List<Employee>
        {
            new Employee
            {
                EmployeeId = "EMP-001",
                FullName = "Mohamed Ayman",
                Email = "mohamed@test.com",
                Department = Department.IT,
                Position = "Backend Developer",
                Salary = 20000,
                HireDate = new DateTime(2025, 1, 10),
                IsActive = true
            },

            new Employee
            {
                EmployeeId = "EMP-002",
                FullName = "Sara Adel",
                Email = "sara@test.com",
                Department = Department.HR,
                Position = "HR Specialist",
                Salary = 12000,
                HireDate = new DateTime(2024, 5, 15),
                IsActive = true
            },

            new Employee
            {
                EmployeeId = "EMP-003",
                FullName = "Ahmed Tarek",
                Email = "ahmed@test.com",
                Department = Department.IT,
                Position = "Junior Developer",
                Salary = 9000,
                HireDate = new DateTime(2026, 1, 1),
                IsActive = true
            },

            new Employee
            {
                EmployeeId = "EMP-004",
                FullName = "Omar Samir",
                Email = "omar@test.com",
                Department = Department.Sales,
                Position = "Sales Executive",
                Salary = 11000,
                HireDate = new DateTime(2023, 11, 20),
                IsActive = true
            },

            new Employee
            {
                EmployeeId = "EMP-005",
                FullName = "Mariam Hassan",
                Email = "mariam@test.com",
                Department = Department.Finance,
                Position = "Accountant",
                Salary = 14000,
                HireDate = new DateTime(2022, 9, 11),
                IsActive = true
            },

            new Employee
            {
                EmployeeId = "EMP-006",
                FullName = "Khaled Ali",
                Email = "khaled@test.com",
                Department = Department.IT,
                Position = "DevOps Trainee",
                Salary = 10000,
                HireDate = new DateTime(2026, 2, 1),
                IsActive = true
            },

            new Employee
            {
                EmployeeId = "EMP-007",
                FullName = "Nour Emad",
                Email = "nour@test.com",
                Department = Department.Marketing,
                Position = "Content Specialist",
                Salary = 9500,
                HireDate = new DateTime(2025, 7, 8),
                IsActive = true
            },

            new Employee
            {
                EmployeeId = "EMP-008",
                FullName = "Youssef Nabil",
                Email = "youssef@test.com",
                Department = Department.Sales,
                Position = "Sales Manager",
                Salary = 18000,
                HireDate = new DateTime(2021, 3, 17),
                IsActive = false
            },

            new Employee
            {
                EmployeeId = "EMP-009",
                FullName = "Dina Farouk",
                Email = "dina@test.com",
                Department = Department.HR,
                Position = "Recruiter",
                Salary = 10500,
                HireDate = new DateTime(2024, 2, 13),
                IsActive = true
            },

            new Employee
            {
                EmployeeId = "EMP-010",
                FullName = "Hady Mahmoud",
                Email = "hady@test.com",
                Department = Department.IT,
                Position = "QA Engineer",
                Salary = 13000,
                HireDate = new DateTime(2025, 10, 1),
                IsActive = true
            },

            new Employee
            {
                EmployeeId = "EMP-011",
                FullName = "Salma Taha",
                Email = "salma@test.com",
                Department = Department.Finance,
                Position = "Finance Manager",
                Salary = 26000,
                HireDate = new DateTime(2020, 12, 12),
                IsActive = true
            },

            new Employee
            {
                EmployeeId = "EMP-012",
                FullName = "Ali Mostafa",
                Email = "ali@test.com",
                Department = Department.Support,
                Position = "Support Agent",
                Salary = 8000,
                HireDate = new DateTime(2026, 3, 5),
                IsActive = true
            }
        });
    }
    public Employee AddEmployee(
    string fullName,
    string email,
    Department department,
    string position,
    decimal salary,
    DateTime hireDate)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required."); //exception andf

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Position is required.");

        if (salary <= 0)
            throw new ArgumentException("Salary must be positive.");

        if (hireDate > DateTime.Now)
            throw new ArgumentException("Hire date cannot be in the future.");

        string employeeId = $"EMP-{_nextEmployeeNumber:D3}";

        var employee = new Employee
        {
            EmployeeId = employeeId,
            FullName = fullName,
            Email = email,
            Department = department,
            Position = position,
            Salary = salary,
            HireDate = hireDate,
            IsActive = true
        };

        _employees.Add(employee);

        _nextEmployeeNumber++;

        return employee;
    }
    public bool UpdateEmployee(
    string employeeId,
    string email,
    Department department,
    string position,
    decimal salary)
    {
        var employee = _employees.FirstOrDefault(e =>
            e.EmployeeId.Equals(employeeId, StringComparison.OrdinalIgnoreCase));
        if (employee == null)
            return false;

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Position is required.");

        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative.");

        employee.Email = email;
        employee.Department = department;
        employee.Position = position;
        employee.Salary = salary;

        return true;
    }
    public bool DeactivateEmployee(string employeeId) //mesh remove
    {
        var employee = _employees.FirstOrDefault(e =>
            e.EmployeeId.Equals(employeeId, StringComparison.OrdinalIgnoreCase));

        if (employee == null)
            return false;

        employee.IsActive = false;

        return true;
    }
    public List<Employee> SearchEmployees(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return new List<Employee>();
        return _employees.Where(e => e.EmployeeId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                || e.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
    public List<Employee> FilterByDepartment(
    Department department,
    bool includeInactive = false)
    {
        return _employees.Where(e => e.Department == department &&
                (includeInactive || e.IsActive))
            .ToList();
    }
    public List<Employee> GetAllEmployees(bool includeInactive = true)
    {
        if (includeInactive)
            return _employees.ToList();

        return _employees.Where(e => e.IsActive).ToList();
    }

    public List<Employee> SortBySalaryAscending()
    {
        return _employees
            .OrderBy(e => e.Salary)
            .ToList();
    }

    public List<Employee> SortBySalaryDescending()
    {
        return _employees
            .OrderByDescending(e => e.Salary)
            .ToList();
    }

    public List<Employee> SortByHireDateAscending()
    {
        return _employees
            .OrderBy(e => e.HireDate)
            .ToList();
    }

    public List<Employee> SortByHireDateDescending()
    {
        return _employees
            .OrderByDescending(e => e.HireDate)
            .ToList();
    }

    public List<Employee> SortByName()
    {
        return _employees
            .OrderBy(e => e.FullName)
            .ToList();
    }
}