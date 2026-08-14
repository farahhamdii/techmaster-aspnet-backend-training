using BankAccountSystem.Models;
using BankAccountSystem.Services;
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
            //Console.WriteLine($"Transactions: {account.Transactions.Count}");

            //Console.ReadKey();

            BankService bankService = new BankService();

            BankAccount account1 = bankService.CreateAccount(
                "Farah Hamdy",
                "farah@gmail.com",
                "01012345678",
                1000,
                AccountType.Savings,
                "ACC001");

            BankAccount account2 = bankService.CreateAccount(
                "Sara Ahmed",
                "sara@gmail.com",
                "01112345678",
                500,
                AccountType.Current,
                "ACC002");

            Console.WriteLine("Before Transfer:");
            Console.WriteLine($"ACC001 Balance: {account1.Balance}");
            Console.WriteLine($"ACC002 Balance: {account2.Balance}");

            bankService.Transfer("ACC001", "ACC002", 300);

            Console.WriteLine("\nAfter Transfer:");
            Console.WriteLine($"ACC001 Balance: {account1.Balance}");
            Console.WriteLine($"ACC002 Balance: {account2.Balance}");

            Console.WriteLine("\nACC001 Transactions:");

            foreach (var transaction in account1.Transactions)
            {
                Console.WriteLine(
                    $"{transaction.TransactionType} - " +
                    $"{transaction.Amount} - " +
                    $"{transaction.BalanceAfterTransaction}");
            }

            Console.WriteLine("\nACC002 Transactions:");

            foreach (var transaction in account2.Transactions)
            {
                Console.WriteLine(
                    $"{transaction.TransactionType} - " +
                    $"{transaction.Amount} - " +
                    $"{transaction.BalanceAfterTransaction}");

                Console.ReadKey();

                Console.ReadKey();
            }
        }
    }
}
