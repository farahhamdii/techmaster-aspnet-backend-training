using System;

namespace Task_01_csharp_drills.Drills;

public static class Drill19_TicketPriceCalculator
{
    public static void Run()
    {
        const decimal basePrice = 100m;

        Console.Write("Enter age: ");
        if (!int.TryParse(Console.ReadLine(), out int age) || age < 0)
        {
            Console.WriteLine("Invalid age.");
            return;
        }

        Console.Write("Are you a student? (yes/no): ");
        string isStudentInput = Console.ReadLine()?.Trim().ToLower();
        bool isStudent = isStudentInput == "yes" || isStudentInput == "y";

        decimal discountPercentage = 0m;

        if (age < 12)
        {
            discountPercentage = 0.50m; // 50%
        }
        else if (age > 60)
        {
            discountPercentage = 0.30m; // 30%
        }
        else if (isStudent)
        {
            discountPercentage = 0.20m; // 20%
        }

        decimal discountAmount = basePrice * discountPercentage;
        decimal finalPrice = basePrice - discountAmount;

        Console.WriteLine($"\nBase Price: {basePrice:C2}");
        Console.WriteLine($"Discount  : {discountAmount:C2} ({discountPercentage * 100}%)");
        Console.WriteLine($"Final Price: {finalPrice:C2}");
    }
}