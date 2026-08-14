namespace BankAccountSystem.Models;

public class Transaction
{
    public int TransactionId { get; private set; }

    public string AccountNumber { get; private set; }

    public TransactionType TransactionType { get; private set; }

    public decimal Amount { get; private set; }

    public DateTime TransactionDate { get; private set; }

    public string Description { get; private set; }

    public decimal BalanceAfterTransaction { get; private set; }

    public Transaction(
        int transactionId,
        string accountNumber,
        TransactionType transactionType,
        decimal amount,
        string description,
        decimal balanceAfterTransaction)
    {
        TransactionId = transactionId;
        AccountNumber = accountNumber;
        TransactionType = transactionType;
        Amount = amount;
        Description = description;
        BalanceAfterTransaction = balanceAfterTransaction;
        TransactionDate = DateTime.Now;
    }
}