using System;
using System.Collections.Generic;

namespace Task_01_csharp_drills.Drills;

public static class Drill05_MaxMinFinder
{
    public static void Run()
    {
        Console.Write("Enter numbers separated by commas (e.g., 5, 1, 9, -2): ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("List is empty.");
            return;
        }
        string[] parts = input.Split(',');
        List<int> numbers = new List<int>();

        foreach (string part in parts)
        {
            if (int.TryParse(part.Trim(), out int num))
            {
                numbers.Add(num);
            }
        }

        if (numbers.Count == 0)
        {
            Console.WriteLine("No valid numbers found.");
            return;
        }

        int max = numbers[0];
        int min = numbers[0];

        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] > max)
                max = numbers[i];

            if (numbers[i] < min)
                min = numbers[i];
        }

        Console.WriteLine($"Max: {max} | Min: {min}");
    }
}