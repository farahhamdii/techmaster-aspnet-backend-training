using System;

namespace Task_01_csharp_drills.Drills;

public static class Drill20_MethodRefactoringChallenge
{
    public static void Run()
    {
        Console.WriteLine("=== Refactored Drill 01: Temperature Converter ===");
        RunTemperatureConverter();

        Console.WriteLine("\n=== Refactored Drill 02: Grade Calculator ===");
        RunGradeCalculator();

        Console.WriteLine("\n=== Refactored Drill 19: Ticket Price Calculator ===");
        RunTicketCalculator();
    }

    // -------------------------------------------------------------
    // 1. Refactored Temperature Converter
    // -------------------------------------------------------------
    private static void RunTemperatureConverter()
    {
        if (TryGetCelsiusInput(out double celsius))
        {
            double fahrenheit = ConvertCelsiusToFahrenheit(celsius);
            PrintTemperatureResult(celsius, fahrenheit);
        }
        else
        {
            Console.WriteLine("Invalid temperature value.");
        }
    }

    private static bool TryGetCelsiusInput(out double celsius)
    {
        Console.Write("Enter temperature in Celsius: ");
        return double.TryParse(Console.ReadLine(), out celsius);
    }

    private static double ConvertCelsiusToFahrenheit(double celsius)
    {
        return (celsius * 9 / 5) + 32;
    }

    private static void PrintTemperatureResult(double celsius, double fahrenheit)
    {
        Console.WriteLine($"{celsius:F2}°C = {fahrenheit:F2}°F");
    }

    // -------------------------------------------------------------
    // 2. Refactored Grade Calculator
    // -------------------------------------------------------------
    private static void RunGradeCalculator()
    {
        int score = ReadScoreInput();

        if (!ValidateScore(score))
        {
            Console.WriteLine("Score must be between 0 and 100.");
            return;
        }

        char grade = ProcessGradeCalculation(score);
        PrintGradeResult(grade);
    }

    private static int ReadScoreInput()
    {
        Console.Write("Enter grade score (0-100): ");
        int.TryParse(Console.ReadLine(), out int score);
        return score;
    }

    private static bool ValidateScore(int score)
    {
        return score >= 0 && score <= 100;
    }

    private static char ProcessGradeCalculation(int score)
    {
        return score switch
        {
            >= 90 => 'A',
            >= 80 => 'B',
            >= 70 => 'C',
            >= 60 => 'D',
            _ => 'F'
        };
    }

    private static void PrintGradeResult(char grade)
    {
        Console.WriteLine($"Grade: {grade}");
    }

    // -------------------------------------------------------------
    // 3. Refactored Ticket Price Calculator
    // -------------------------------------------------------------
    private static void RunTicketCalculator()
    {
        const decimal basePrice = 100m;

        if (!TryGetTicketUserInputs(out int age, out bool isStudent))
        {
            Console.WriteLine("Invalid age provided.");
            return;
        }

        decimal discountRate = GetDiscountRate(age, isStudent);
        decimal finalPrice = CalculateFinalTicketPrice(basePrice, discountRate);

        PrintTicketReceipt(basePrice, discountRate, finalPrice);
    }

    private static bool TryGetTicketUserInputs(out int age, out bool isStudent)
    {
        isStudent = false;
        Console.Write("Enter age: ");
        if (!int.TryParse(Console.ReadLine(), out age) || age < 0)
        {
            return false;
        }

        Console.Write("Are you a student? (yes/no): ");
        string response = Console.ReadLine()?.Trim().ToLower();
        isStudent = response == "yes" || response == "y";

        return true;
    }

    private static decimal GetDiscountRate(int age, bool isStudent)
    {
        if (age < 12) return 0.50m;
        if (age > 60) return 0.30m;
        if (isStudent) return 0.20m;
        return 0m;
    }

    private static decimal CalculateFinalTicketPrice(decimal basePrice, decimal discountRate)
    {
        return basePrice * (1m - discountRate);
    }

    private static void PrintTicketReceipt(decimal basePrice, decimal discountRate, decimal finalPrice)
    {
        Console.WriteLine($"Base Price: {basePrice:C2} | Discount: {discountRate * 100}% | Final: {finalPrice:C2}");
    }
}