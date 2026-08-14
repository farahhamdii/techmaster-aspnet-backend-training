using BankAccountSystem.Models;
using BankAccountSystem.Services;

namespace BankAccountSystem.UI;

public class ConsoleMenu
{
    private readonly BankService _bankService;

    public ConsoleMenu(BankService bankService)
    {
        _bankService = bankService;
    }

    public void Run()
    {
        while (true)
        {
            ShowMenu();
            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        CreateAccount();
                        break;
                    case "2":
                        Deposit();
                        break;
                    case "3":
                        Withdraw();
                        break;
                    case "4":
                        Transfer();
                        break;
                    case "5":
                        ViewAccountDetails();
                        break;
                    case "6":
                        ViewTransactionHistory();
                        break;
                    case "7":
                        ViewAllAccounts();
                        break;
                    case "8":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("====== TechMaster Bank System ======");
        Console.WriteLine("1. Create Customer Account");
        Console.WriteLine("2. Deposit Money");
        Console.WriteLine("3. Withdraw Money");
        Console.WriteLine("4. Transfer Money");
        Console.WriteLine("5. View Account Details");
        Console.WriteLine("6. View Transaction History");
        Console.WriteLine("7. View All Accounts");
        Console.WriteLine("8. Exit");
        Console.Write("Choose an option: ");
    }

    private void CreateAccount()
    {
        Console.Write("Full Name: ");
        string fullName = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Phone Number: ");
        string phoneNumber = Console.ReadLine() ?? "";

        Console.Write("Initial Balance: ");
        decimal initialBalance = decimal.Parse(Console.ReadLine()!);

        Console.Write("Account Type (1-Savings, 2-Current): ");
        AccountType accountType = (AccountType)int.Parse(Console.ReadLine()!);

        Console.Write("Account Number: ");
        string accountNumber = Console.ReadLine() ?? "";

        BankAccount account = _bankService.CreateAccount(
            fullName,
            email,
            phoneNumber,
            initialBalance,
            accountType,
            accountNumber);

        Console.WriteLine($"Account created successfully: {account.AccountNumber}");
    }

    private void Deposit()
    {
        Console.Write("Account Number: ");
        string accountNumber = Console.ReadLine() ?? "";

        Console.Write("Amount: ");
        decimal amount = decimal.Parse(Console.ReadLine()!);

        _bankService.Deposit(accountNumber, amount);

        Console.WriteLine("Deposit successful.");
    }

    private void Withdraw()
    {
        Console.Write("Account Number: ");
        string accountNumber = Console.ReadLine() ?? "";

        Console.Write("Amount: ");
        decimal amount = decimal.Parse(Console.ReadLine()!);

        _bankService.Withdraw(accountNumber, amount);

        Console.WriteLine("Withdrawal successful.");
    }

    private void Transfer()
    {
        Console.Write("Source Account Number: ");
        string sourceAccount = Console.ReadLine() ?? "";

        Console.Write("Destination Account Number: ");
        string destinationAccount = Console.ReadLine() ?? "";

        Console.Write("Amount: ");
        decimal amount = decimal.Parse(Console.ReadLine()!);

        _bankService.Transfer(
            sourceAccount,
            destinationAccount,
            amount);

        Console.WriteLine("Transfer successful.");
    }

    private void ViewAccountDetails()
    {
        Console.Write("Account Number: ");
        string accountNumber = Console.ReadLine() ?? "";

        BankAccount? account = _bankService.GetAccountByNumber(accountNumber);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        Console.WriteLine("\n--- Account Details ---");
        Console.WriteLine($"Account Number: {account.AccountNumber}");
        Console.WriteLine($"Customer Name: {account.Customer.FullName}");
        Console.WriteLine($"Email: {account.Customer.Email}");
        Console.WriteLine($"Phone: {account.Customer.PhoneNumber}");
        Console.WriteLine($"Account Type: {account.AccountType}");
        Console.WriteLine($"Balance: {account.Balance}");
        Console.WriteLine($"Created At: {account.CreatedAt}");
        Console.WriteLine($"Status: {(account.IsActive ? "Active" : "Inactive")}");
    }

    private void ViewTransactionHistory()
    {
        Console.Write("Account Number: ");
        string accountNumber = Console.ReadLine() ?? "";

        List<Transaction> transactions =
            _bankService.GetTransactionHistory(accountNumber);

        if (transactions.Count == 0)
        {
            Console.WriteLine("No transactions yet.");
            return;
        }

        Console.WriteLine("\n--- Transaction History ---");

        foreach (Transaction transaction in transactions)
        {
            Console.WriteLine(
                $"{transaction.TransactionType} | " +
                $"{transaction.Amount} | " +
                $"{transaction.TransactionDate} | " +
                $"{transaction.Description} | " +
                $"Balance: {transaction.BalanceAfterTransaction}");
        }
    }

    private void ViewAllAccounts()
    {
        List<BankAccount> accounts = _bankService.GetAllAccounts();

        if (accounts.Count == 0)
        {
            Console.WriteLine("No accounts created.");
            return;
        }

        Console.WriteLine("\n--- All Accounts ---");

        foreach (BankAccount account in accounts)
        {
            Console.WriteLine(
                $"{account.AccountNumber} | " +
                $"{account.Customer.FullName} | " +
                $"{account.AccountType} | " +
                $"{account.Balance} | " +
                $"{(account.IsActive ? "Active" : "Inactive")}");
        }
    }
}