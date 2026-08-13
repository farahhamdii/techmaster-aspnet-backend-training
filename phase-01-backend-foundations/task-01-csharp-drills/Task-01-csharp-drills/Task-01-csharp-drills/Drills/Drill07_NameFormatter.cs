using System;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Task_01_csharp_drills.Drills;

public static class Drill07_NameFormatter
{
    public static void Run()
    {
        Console.Write("Enter full name: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid name.");
            return;
        }

        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        string formattedName = textInfo.ToTitleCase(input.ToLower());

        Console.WriteLine($"Formatted: {formattedName}");
    }
}