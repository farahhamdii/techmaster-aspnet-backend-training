using System;
namespace Task_01_csharp_drills.Drills;

public static class Drill10_SimpleAtmMenu
{
    public static void Run()
    {
        decimal balance = 1000m;
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- ATM MENU ---");
            Console.WriteLine("1. Check Balance");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine($"Current Balance: {balance:C2}");
                    break;

                case "2":
                    Console.Write("Enter deposit amount: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal deposit) && deposit > 0)
                    {
                        balance += deposit;
                        Console.WriteLine($"Successfully deposited {deposit:C2}. New Balance: {balance:C2}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid deposit amount.");
                    }
                    break;

                case "3":
                    Console.Write("Enter withdrawal amount: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal withdraw) && withdraw > 0)
                    {
                        if (withdraw <= balance)
                        {
                            balance -= withdraw;
                            Console.WriteLine($"Successfully withdrew {withdraw:C2}. Remaining Balance: {balance:C2}");
                        }
                        else
                        {
                            Console.WriteLine("Insufficient balance.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid withdrawal amount.");
                    }
                    break;

                case "4":
                    running = false;
                    Console.WriteLine("Thank you for using our ATM.");
                    break;

                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
}