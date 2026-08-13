
namespace Task_01_csharp_drills.Drills;

public static class Drill03_LoginValidator
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 03: Simple Login Validator ===");

        // 1. Constants
        const string correctUsername = "admin";
        const string correctPassword = "123";
        const int maxAttempts = 3;

        bool isAuthenticated = false;

        // 2. Loop from 1 to 3
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            Console.Write("Enter username: ");
            string? inputUsername = Console.ReadLine();

            Console.Write("Enter password: ");
            string? inputPassword = Console.ReadLine();

            // 3. Comparisons
            bool isUsernameValid = string.Equals(inputUsername, correctUsername, StringComparison.OrdinalIgnoreCase);
            bool isPasswordValid = inputPassword == correctPassword;

            if (isUsernameValid && isPasswordValid)
            {
                isAuthenticated = true;
                break; // Break loop on success
            }

            Console.WriteLine($"Incorrect username or password. Remaining attempts: {maxAttempts - attempt}\n");
        }

        // 4. Final Output
        if (isAuthenticated)
        {
            Console.WriteLine("Login successful.");
        }
        else
        {
            Console.WriteLine("Account locked. Too many failed attempts.");
        }
    }
}