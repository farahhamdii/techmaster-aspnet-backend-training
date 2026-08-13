using System;
using System.Collections.Generic;

namespace Task_01_csharp_drills.Drills;

public static class Drill17_SimpleSearchEngine
{
    public static void Run()
    {
        List<string> names = new List<string>
        {
            "Mohamed Ayman",
            "Ahmed Hassan",
            "Sara Ali",
            "Mahmoud Reda",
            "Ayman Zaki"
        };

        Console.Write("Enter search term: ");
        string query = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(query))
        {
            Console.WriteLine("Search query cannot be empty.");
            return;
        }

        List<string> results = new List<string>();
        foreach (string name in names)
        {
            if (name.Contains(query, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(name);
            }
        }

        if (results.Count > 0)
        {
            Console.WriteLine("\n--- Matches Found ---");
            foreach (string result in results)
            {
                Console.WriteLine($"- {result}");
            }
        }
        else
        {
            Console.WriteLine("No matching names found.");
        }
    }
}