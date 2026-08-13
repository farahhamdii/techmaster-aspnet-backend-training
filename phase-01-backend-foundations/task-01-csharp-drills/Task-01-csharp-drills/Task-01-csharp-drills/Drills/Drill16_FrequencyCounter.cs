using System;
using System.Collections.Generic;

namespace Task_01_csharp_drills.Drills;

public static class Drill16_FrequencyCounter
{
    public static void Run()
    {
        Console.Write("Enter numbers separated by spaces (e.g., 1 2 1 3 2 1): ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("No input provided.");
            return;
        }

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Dictionary<int, int> frequencies = new Dictionary<int, int>();

        foreach (string part in parts)
        {
            if (int.TryParse(part, out int num))
            {
                if (frequencies.ContainsKey(num))
                    frequencies[num]++;
                else
                    frequencies[num] = 1;
            }
        }

        Console.WriteLine("\n--- Frequencies ---");
        foreach (var pair in frequencies)
        {
            Console.WriteLine($"{pair.Key} => {pair.Value}");
        }
    }
}