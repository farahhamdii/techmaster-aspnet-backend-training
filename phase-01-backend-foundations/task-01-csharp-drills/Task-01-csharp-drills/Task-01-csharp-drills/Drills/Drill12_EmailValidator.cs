using System;

namespace Task_01_csharp_drills.Drills;

public static class Drill12_EmailValidator
{
    public static void Run()
    {
        Console.Write("Enter email address: ");
        string email = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Invalid: Email cannot be empty.");
            return;
        }

        if (email.Contains(" "))
        {
            Console.WriteLine("Invalid: Email cannot contain spaces.");
            return;
        }

        int atIndex = email.IndexOf('@');
        int lastAtIndex = email.LastIndexOf('@');

        if (atIndex <= 0 || atIndex != lastAtIndex || atIndex == email.Length - 1)
        {
            Console.WriteLine("Invalid: Must contain exactly one '@' character and not at the ends.");
            return;
        }

        int dotIndex = email.LastIndexOf('.');
        if (dotIndex <= atIndex + 1 || dotIndex == email.Length - 1)
        {
            Console.WriteLine("Invalid: Must contain a '.' after the '@' symbol.");
            return;
        }

        Console.WriteLine("Valid email address.");
    }
}