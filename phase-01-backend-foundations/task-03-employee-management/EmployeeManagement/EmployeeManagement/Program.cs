using EmployeeManagement.Services;
using EmployeeManagement.UI;

namespace EmployeeManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var employeeService = new EmployeeService();
            var reportService = new EmployeeReportService(employeeService);
            var menu = new ConsoleMenu(employeeService,reportService);
            menu.Start();
        }
    }
}