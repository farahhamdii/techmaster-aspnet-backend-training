using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_01_csharp_drills.Drills;

public static class Drill18_NumberStatistics
{
    public static void Run()
    {
        Console.Write("Enter space-separated numbers (e.g., -5 10 0 4 -2 8): ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("No input provided.");
            return;
        }

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        List<int> numbers = new List<int>();

        foreach (string part in parts)
        {
            if (int.TryParse(part, out int num))
            {
                numbers.Add(num);
            }
        }

        if (numbers.Count == 0)
        {
            Console.WriteLine("No valid numbers parsed.");
            return;
        }

        int count = numbers.Count;
        int sum = numbers.Sum();
        double average = numbers.Average();
        int max = numbers.Max();
        int min = numbers.Min();
        int positiveCount = numbers.Count(n => n > 0);
        int negativeCount = numbers.Count(n => n < 0);

        Console.WriteLine("\n--- Statistics Summary ---");
        Console.WriteLine($"Total Count   : {count}");
        Console.WriteLine($"Sum           : {sum}");
        Console.WriteLine($"Average       : {average:F2}");
        Console.WriteLine($"Max           : {max}");
        Console.WriteLine($"Min           : {min}");
        Console.WriteLine($"Positive Count: {positiveCount}");
        Console.WriteLine($"Negative Count: {negativeCount}");
    }
}