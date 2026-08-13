using System;
using System.Text;

namespace Task_01_csharp_drills.Drills;

public static class Drill13_PalindromeChecker
{
    public static void Run()
    {
        Console.Write("Enter a word or phrase: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid input.");
            return;
        }

        StringBuilder cleanInput = new StringBuilder();
        foreach (char c in input)
        {
            if (char.IsLetterOrDigit(c))
            {
                cleanInput.Append(char.ToLower(c));
            }
        }

        string original = cleanInput.ToString();
        char[] arr = original.ToCharArray();
        Array.Reverse(arr);
        string reversed = new string(arr);

        if (original == reversed)
        {
            Console.WriteLine($"\"{input}\" is a Palindrome.");
        }
        else
        {
            Console.WriteLine($"\"{input}\" is NOT a Palindrome.");
        }
    }
}