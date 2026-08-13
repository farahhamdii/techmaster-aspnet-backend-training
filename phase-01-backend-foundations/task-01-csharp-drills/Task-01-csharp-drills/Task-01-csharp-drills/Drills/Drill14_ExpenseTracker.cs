using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_01_csharp_drills.Drills;

public static class Drill14_ExpenseTracker
{
    public static void Run()
    {
        List<decimal> expenses = new List<decimal>();

        while (true)
        {
            Console.Write("Enter an expense amount (or type 'done' to finish): ");
            string input = Console.ReadLine();

            if (input?.Trim().ToLower() == "done")
                break;

            if (decimal.TryParse(input, out decimal amount) && amount > 0)
            {
                expenses.Add(amount);
            }
            else
            {
                Console.WriteLine("Invalid expense amount. Enter a positive number.");
            }
        }

        if (expenses.Count == 0)
        {
            Console.WriteLine("No expenses recorded.");
            return;
        }

        decimal total = expenses.Sum();
        decimal average = expenses.Average();
        decimal max = expenses.Max();

        Console.WriteLine("\n--- Expense Summary ---");
        Console.WriteLine($"Total Expenses : {total:C2}");
        Console.WriteLine($"Average Expense: {average:C2}");
        Console.WriteLine($"Highest Expense: {max:C2}");
    }
}