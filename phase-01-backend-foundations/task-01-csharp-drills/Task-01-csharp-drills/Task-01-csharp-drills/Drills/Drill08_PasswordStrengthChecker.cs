using System;
using System.Collections.Generic;
using System.Linq;
namespace Task_01_csharp_drills.Drills;

public static class Drill08_PasswordStrengthChecker
{
    public static void Run()
    {
        Console.Write("Enter password: ");
        string password = Console.ReadLine() ?? string.Empty;

        List<string> missingCriteria = new List<string>();

        if (password.Length < 8)
            missingCriteria.Add("minimum 8 characters");

        if (!password.Any(char.IsUpper))
            missingCriteria.Add("uppercase letter");

        if (!password.Any(char.IsLower))
            missingCriteria.Add("lowercase letter");

        if (!password.Any(char.IsDigit))
            missingCriteria.Add("digit");

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            missingCriteria.Add("special character");

        if (missingCriteria.Count == 0)
        {
            Console.WriteLine("Strong password.");
        }
        else
        {
            Console.WriteLine($"Weak - missing: {string.Join(", ", missingCriteria)}");
        }
    }
}