using System;
using System.Collections.Generic;
namespace Task_01_csharp_drills.Drills;
public static class Drill04_EvenOddAnalyzer
{
    public static void Run()
    {
        Console.Write("Enter comma-separated numbers (e.g., 10,7,4,9,2): ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Input cannot be empty.");
            return;
        }

        string[] parts = input.Split(',');
        List<int> evens = new List<int>();
        List<int> odds = new List<int>();

        foreach (string part in parts)
        {
            if (int.TryParse(part.Trim(), out int num))
            {
                if (num % 2 == 0)
                    evens.Add(num);
                else
                    odds.Add(num);
            }
        }

        Console.WriteLine($"Even: {string.Join(", ", evens)} | Odd: {string.Join(", ", odds)}");
    }
}