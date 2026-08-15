using EmployeeManagement.Models;

namespace EmployeeManagement.Services;

public class EmployeeReportService
{
    private readonly EmployeeService _employeeService;

    public EmployeeReportService(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public decimal GetAverageSalary()
    {
        var employees = _employeeService.GetAllEmployees();
        return employees.Any()? employees.Average(e => e.Salary): 0;
    }

    public Employee? GetHighestSalaryEmployee()
    {
        return _employeeService
            .GetAllEmployees()
            .OrderByDescending(e => e.Salary)
            .FirstOrDefault();
    }

    public Employee? GetLowestSalaryEmployee()
    {
        return _employeeService
            .GetAllEmployees()
            .OrderBy(e => e.Salary)
            .FirstOrDefault();
    }

    public decimal GetTotalPayroll()
    {
        return _employeeService
            .GetAllEmployees()
            .Where(e => e.IsActive)
            .Sum(e => e.Salary);
    }

    public Dictionary<Department, int> GetEmployeeCountByDepartment()
    {
        return _employeeService
            .GetAllEmployees()
            .GroupBy(e => e.Department)
            .ToDictionary(g => g.Key,g => g.Count());
    }

    public int GetActiveCount()
    {
        return _employeeService
            .GetAllEmployees()
            .Count(e => e.IsActive);
    }

    public int GetInactiveCount()
    {
        return _employeeService
            .GetAllEmployees()
            .Count(e => !e.IsActive);
    }
}