using System;
using System.Collections.Generic;

namespace Task_01_csharp_drills.Drills;


public static class Drill06_WordCounter
{
    public static void Run()
    {
        Console.Write("Enter a sentence: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Word count: 0");
            return;
        }

        string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Console.WriteLine($"Word count: {words.Length}");
    }
}