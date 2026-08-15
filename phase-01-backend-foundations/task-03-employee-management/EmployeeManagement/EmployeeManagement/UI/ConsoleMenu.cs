using EmployeeManagement.Models;
using EmployeeManagement.Services;

namespace EmployeeManagement.UI;

public class ConsoleMenu
{
    private readonly EmployeeService _employeeService;
    private readonly EmployeeReportService _reportService;
    public ConsoleMenu( EmployeeService employeeService, EmployeeReportService reportService)
    {
        _employeeService = employeeService;
        _reportService = reportService;
    }
    public void Start()
    {
        bool running = true;

        while (running)
        {
            DisplayMenu();

            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            Console.Clear();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddEmployee();
                        break;

                    case "2":
                        UpdateEmployee();
                        break;

                    case "3":
                        DeactivateEmployee();
                        break;

                    case "4":
                        SearchEmployee();
                        break;

                    case "5":
                        FilterByDepartment();
                        break;

                    case "6":
                        SortEmployees();
                        break;

                    case "7":
                        ShowSalaryReports();
                        break;

                    case "8":
                        ViewAllEmployees();
                        break;

                    case "9":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (running)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("====== Employee Management System ======");
        Console.WriteLine("1. Add Employee");
        Console.WriteLine("2. Update Employee");
        Console.WriteLine("3. Deactivate Employee");
        Console.WriteLine("4. Search Employee");
        Console.WriteLine("5. Filter by Department");
        Console.WriteLine("6. Sort Employees");
        Console.WriteLine("7. Show Salary Reports");
        Console.WriteLine("8. View All Employees");
        Console.WriteLine("9. Exit");
        Console.WriteLine("========================================");
    }
    private void AddEmployee()
    {
        Console.WriteLine("=== Add Employee ===");

        Console.Write("Full Name: ");
        string fullName = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Department (IT, HR, Sales, Finance, Marketing, Support): ");
        Department department = Enum.Parse<Department>(
            Console.ReadLine() ?? "",
            true);

        Console.Write("Position: ");
        string position = Console.ReadLine() ?? "";

        Console.Write("Salary: ");
        decimal salary = decimal.Parse(Console.ReadLine() ?? "");

        Console.Write("Hire Date (yyyy-MM-dd): ");
        DateTime hireDate = DateTime.Parse(Console.ReadLine() ?? "");

        var employee = _employeeService.AddEmployee(
            fullName,
            email,
            department,
            position,
            salary,
            hireDate);

        Console.WriteLine(
            $"\nEmployee added successfully! ID: {employee.EmployeeId}");
    }

    private void UpdateEmployee()
    {
        Console.WriteLine("=== Update Employee ===");

        Console.Write("Employee ID: ");
        string employeeId = Console.ReadLine() ?? "";

        Console.Write("New Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("New Department: ");
        Department department = Enum.Parse<Department>(
            Console.ReadLine() ?? "",
            true);

        Console.Write("New Position: ");
        string position = Console.ReadLine() ?? "";

        Console.Write("New Salary: ");
        decimal salary = decimal.Parse(Console.ReadLine() ?? "");

        bool updated = _employeeService.UpdateEmployee(
            employeeId,
            email,
            department,
            position,
            salary);

        Console.WriteLine(
            updated
                ? "Employee updated successfully."
                : "Employee not found.");
    }

    private void DeactivateEmployee()
    {
        Console.WriteLine("=== Deactivate Employee ===");

        Console.Write("Employee ID: ");
        string employeeId = Console.ReadLine() ?? "";

        bool deactivated =
            _employeeService.DeactivateEmployee(employeeId);

        Console.WriteLine(
            deactivated
                ? "Employee deactivated successfully."
                : "Employee not found.");
    }
    private void SearchEmployee()
    {
        Console.WriteLine("=== Search Employee ===");

        Console.Write("Search by ID or Name: ");
        string searchTerm = Console.ReadLine() ?? "";

        var employees = _employeeService.SearchEmployees(searchTerm);

        DisplayEmployees(employees);
    }
    private void FilterByDepartment()
    {
        Console.WriteLine("=== Filter By Department ===");

        Console.Write("Department: ");

        Department department = Enum.Parse<Department>(
            Console.ReadLine() ?? "",
            true);

        var employees =
            _employeeService.FilterByDepartment(department);

        DisplayEmployees(employees);
    }
    private void SortEmployees()
    {
        Console.WriteLine("=== Sort Employees ===");
        Console.WriteLine("1. Salary Ascending");
        Console.WriteLine("2. Salary Descending");
        Console.WriteLine("3. Hire Date Ascending");
        Console.WriteLine("4. Hire Date Descending");
        Console.WriteLine("5. Name");

        Console.Write("Choose: ");
        string? choice = Console.ReadLine();

        var employees = choice switch
        {
            "1" => _employeeService.SortBySalaryAscending(),
            "2" => _employeeService.SortBySalaryDescending(),
            "3" => _employeeService.SortByHireDateAscending(),
            "4" => _employeeService.SortByHireDateDescending(),
            "5" => _employeeService.SortByName(),
            _ => new List<Employee>()
        };

        DisplayEmployees(employees);
    }
    private void ShowSalaryReports()
    {
        Console.WriteLine("=== Salary Reports ===");

        Console.WriteLine(
            $"Average Salary: {_reportService.GetAverageSalary():C}");

        var highest = _reportService.GetHighestSalaryEmployee();

        if (highest != null)
        {
            Console.WriteLine(
                $"Highest Salary: {highest.FullName} - {highest.Salary:C}");
        }

        var lowest = _reportService.GetLowestSalaryEmployee();

        if (lowest != null)
        {
            Console.WriteLine(
                $"Lowest Salary: {lowest.FullName} - {lowest.Salary:C}");
        }

        Console.WriteLine(
            $"Total Payroll: {_reportService.GetTotalPayroll():C}");

        Console.WriteLine(
            $"Active Employees: {_reportService.GetActiveCount()}");

        Console.WriteLine(
            $"Inactive Employees: {_reportService.GetInactiveCount()}");

        Console.WriteLine("\nEmployees By Department:");

        var departmentCounts =
            _reportService.GetEmployeeCountByDepartment();

        foreach (var item in departmentCounts)
        {
            Console.WriteLine(
                $"{item.Key}: {item.Value}");
        }
    }
    private void ViewAllEmployees()
    {
        Console.WriteLine("=== All Employees ===");

        var employees = _employeeService.GetAllEmployees();

        DisplayEmployees(employees);
    }
    private void DisplayEmployees(IEnumerable<Employee> employees)
    {
        var employeeList = employees.ToList();

        if (!employeeList.Any())
        {
            Console.WriteLine("No employees found.");
            return;
        }

        foreach (var employee in employeeList)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ID: {employee.EmployeeId}");
            Console.WriteLine($"Name: {employee.FullName}");
            Console.WriteLine($"Email: {employee.Email}");
            Console.WriteLine($"Department: {employee.Department}");
            Console.WriteLine($"Position: {employee.Position}");
            Console.WriteLine($"Salary: {employee.Salary:C}");
            Console.WriteLine($"Hire Date: {employee.HireDate:yyyy-MM-dd}");
            Console.WriteLine($"Status: {(employee.IsActive ? "Active" : "Inactive")}");
        }

        Console.WriteLine("----------------------------------------");
    }
}