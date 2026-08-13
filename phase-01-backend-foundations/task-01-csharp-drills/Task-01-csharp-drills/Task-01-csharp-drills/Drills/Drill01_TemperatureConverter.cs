
namespace Task_01_csharp_drills.Drills;

public static class Drill01_TemperatureConverter
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 01: Temperature Converter ===");
        Console.Write("Enter temperature in Celsius: ");

        string? input = Console.ReadLine();

        if (double.TryParse(input, out double celsius))
        {
            double fahrenheit = (celsius * 9 / 5) + 32;
            Console.WriteLine($"{celsius}°C is equal to {fahrenheit}°F");
        }
        else
        {
            Console.WriteLine("Invalid input! Please enter a numerical value.");
        }
    }
}