namespace BankAccountSystem.Models;

public class Customer
{
    public int CustomerId { get; private set; }

    public string FullName { get; private set; }

    public string Email { get; private set; }

    public string PhoneNumber { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Customer(
        int customerId,
        string fullName,
        string email,
        string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required.");

        CustomerId = customerId;
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        CreatedAt = DateTime.Now;
    }
}