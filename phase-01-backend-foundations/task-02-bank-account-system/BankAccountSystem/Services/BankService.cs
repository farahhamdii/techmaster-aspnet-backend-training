using BankAccountSystem.Models;

namespace BankAccountSystem.Services;

public class BankService
{
    private readonly List<BankAccount> _accounts = new();

    private int _nextCustomerId = 1;
    private int _nextTransactionId = 1;

    public BankAccount CreateAccount(
        string fullName,
        string email,
        string phoneNumber,
        decimal initialBalance,
        AccountType accountType,
        string accountNumber)
    {
        if (_accounts.Any(a => a.AccountNumber == accountNumber))
            throw new InvalidOperationException("Account number already exists.");

        Customer customer = new Customer(
            _nextCustomerId,
            fullName,
            email,
            phoneNumber);

        _nextCustomerId++;

        BankAccount account = new BankAccount(
            accountNumber,
            customer,
            initialBalance,
            accountType);

        _accounts.Add(account);

        return account;
    }

    public BankAccount? GetAccountByNumber(string accountNumber)
    {
        return _accounts.FirstOrDefault(
            a => a.AccountNumber == accountNumber);
    }

    public void Deposit(string accountNumber, decimal amount)
    {
        BankAccount? account = GetAccountByNumber(accountNumber);

        if (account == null)
            throw new InvalidOperationException("Account not found.");

        account.Deposit(amount);

        Transaction transaction = new Transaction(
            _nextTransactionId,
            account.AccountNumber,
            TransactionType.Deposit,
            amount,
            "Money deposited",
            account.Balance);

        _nextTransactionId++;

        account.AddTransaction(transaction);
    }

    public void Withdraw(string accountNumber, decimal amount)
    {
        BankAccount? account = GetAccountByNumber(accountNumber);

        if (account == null)
            throw new InvalidOperationException("Account not found.");

        account.Withdraw(amount);

        Transaction transaction = new Transaction(
            _nextTransactionId,
            account.AccountNumber,
            TransactionType.Withdraw,
            amount,
            "Money withdrawn",
            account.Balance);

        _nextTransactionId++;

        account.AddTransaction(transaction);
    }
    public void Transfer(
    string sourceAccountNumber,
    string destinationAccountNumber,
    decimal amount)
    {
        BankAccount? sourceAccount = GetAccountByNumber(sourceAccountNumber);
        BankAccount? destinationAccount = GetAccountByNumber(destinationAccountNumber);

        if (sourceAccount == null)
            throw new InvalidOperationException("Source account not found.");

        if (destinationAccount == null)
            throw new InvalidOperationException("Destination account not found.");

        if (sourceAccountNumber == destinationAccountNumber)
            throw new InvalidOperationException(
                "Source and destination accounts cannot be the same.");

        if (amount <= 0)
            throw new ArgumentException(
                "Transfer amount must be greater than zero.");

        if (amount > sourceAccount.Balance)
            throw new InvalidOperationException(
                "Insufficient balance.");

        // Withdraw from source
        sourceAccount.Withdraw(amount);

        // Deposit into destination
        destinationAccount.Deposit(amount);

        // Transaction for source
        Transaction transferOut = new Transaction(
            _nextTransactionId,
            sourceAccount.AccountNumber,
            TransactionType.TransferOut,
            amount,
            $"Transfer to {destinationAccount.AccountNumber}",
            sourceAccount.Balance);

        _nextTransactionId++;

        sourceAccount.AddTransaction(transferOut);

        // Transaction for destination
        Transaction transferIn = new Transaction(
            _nextTransactionId,
            destinationAccount.AccountNumber,
            TransactionType.TransferIn,
            amount,
            $"Transfer from {sourceAccount.AccountNumber}",
            destinationAccount.Balance);

        _nextTransactionId++;

        destinationAccount.AddTransaction(transferIn);
    }
    public List<Transaction> GetTransactionHistory(string accountNumber)
    {
        BankAccount? account = GetAccountByNumber(accountNumber);

        if (account == null)
            throw new InvalidOperationException("Account not found.");

        return account.Transactions
            .OrderByDescending(t => t.TransactionDate)
            .ToList();
    }
    public List<BankAccount> GetAllAccounts()
    {
        return _accounts.ToList();
    }
}