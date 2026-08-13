using System;

namespace Task_01_csharp_drills.Drills;

public static class Drill09_ShoppingCartTotal
{
    public static void Run()
    {
        Console.Write("How many items? ");
        if (!int.TryParse(Console.ReadLine(), out int itemCount) || itemCount <= 0)
        {
            Console.WriteLine("Invalid item count.");
            return;
        }

        decimal grandTotal = 0m;

        for (int i = 1; i <= itemCount; i++)
        {
            Console.WriteLine($"\n--- Item {i} ---");

            Console.Write("Enter price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                return;
            }

            decimal subtotal = price * quantity;
            grandTotal += subtotal;
        }

        Console.WriteLine($"\nSubtotal: {grandTotal:F2}");

        if (grandTotal > 1000m)
        {
            decimal discount = grandTotal * 0.10m;
            decimal finalTotal = grandTotal - discount;
            Console.WriteLine($"Discount: {discount:F2}, Final: {finalTotal:F2}");
        }
        else
        {
            Console.WriteLine("No discount applied.");
            Console.WriteLine($"Final Total: {grandTotal:F2}");
        }
    }
}