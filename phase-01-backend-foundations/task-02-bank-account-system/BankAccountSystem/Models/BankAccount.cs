namespace BankAccountSystem.Models;

public class BankAccount
{
    public string AccountNumber { get; private set; }

    public Customer Customer { get; private set; }

    public decimal Balance { get; private set; }

    public AccountType AccountType { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public bool IsActive { get; private set; }

    public List<Transaction> Transactions { get; private set; }

    public BankAccount(
        string accountNumber,
        Customer customer,
        decimal initialBalance,
        AccountType accountType)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number is required.");

        if (customer == null)
            throw new ArgumentNullException(nameof(customer));

        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.");

        AccountNumber = accountNumber;
        Customer = customer;
        Balance = initialBalance;
        AccountType = accountType;
        CreatedAt = DateTime.Now;
        IsActive = true;
        Transactions = new List<Transaction>();
    }

    public void Deposit(decimal amount)
    {
        if (!IsActive)
            throw new InvalidOperationException("Account is inactive.");

        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be greater than zero.");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (!IsActive)
            throw new InvalidOperationException("Account is inactive.");

        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be greater than zero.");

        if (amount > Balance)
            throw new InvalidOperationException("Insufficient balance.");

        Balance -= amount;
    }

    public void AddTransaction(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        Transactions.Add(transaction);
    }
}