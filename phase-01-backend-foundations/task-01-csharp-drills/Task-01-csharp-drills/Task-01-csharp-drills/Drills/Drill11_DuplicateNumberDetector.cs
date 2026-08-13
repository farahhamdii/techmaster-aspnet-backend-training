using System;
using System.Collections.Generic;

namespace Task_01_csharp_drills.Drills;

public static class Drill11_DuplicateNumberDetector
{
    public static void Run()
    {
        Console.Write("Enter numbers separated by spaces (e.g., 1 2 3 2 4 1): ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("No numbers provided.");
            return;
        }

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        HashSet<int> seen = new HashSet<int>();
        HashSet<int> duplicates = new HashSet<int>();

        foreach (string part in parts)
        {
            if (int.TryParse(part, out int num))
            {
                if (!seen.Add(num))
                {
                    duplicates.Add(num);
                }
            }
        }

        if (duplicates.Count > 0)
        {
            Console.WriteLine($"Duplicates: {string.Join(", ", duplicates)}");
        }
        else
        {
            Console.WriteLine("No duplicate numbers found.");
        }
    }
}