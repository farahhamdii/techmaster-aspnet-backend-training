using BankAccountSystem.Models;
using BankAccountSystem.Services;
using BankAccountSystem.UI;
namespace BankAccountSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //BankService bankService = new BankService();

            //BankAccount account = bankService.CreateAccount(
            //    "Farah Hamdy",
            //    "farah@gmail.com",
            //    "01012345678",
            //    1000,
            //    AccountType.Savings,
            //    "ACC001"
            //);

            //Console.WriteLine("Account Created Successfully!");
            //Console.WriteLine($"Account Number: {account.AccountNumber}");
            //Console.WriteLine($"Customer: {account.Customer.FullName}");
            //Console.WriteLine($"Balance: {account.Balance}");

            //Console.WriteLine("\n--- Deposit 500 ---");

            //bankService.Deposit("ACC001", 500);

            //Console.WriteLine($"New Balance: {account.Balance}");
      
               
            BankService bankService = new BankService();
            ConsoleMenu menu = new ConsoleMenu(bankService);
            menu.Run();
            Console.ReadKey();
        }
    }
}
