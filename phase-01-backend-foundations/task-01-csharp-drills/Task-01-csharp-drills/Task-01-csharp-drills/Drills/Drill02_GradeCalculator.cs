namespace Task_01_csharp_drills.Drills;

public static class Drill02_GradeCalculator
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 02: Grade Calculator ===");
        Console.Write("Enter your score (0 - 100): ");

        string? input = Console.ReadLine();
        if (int.TryParse(input, out int score) && score >= 0 && score <= 100)
        {
            if (score >= 90)
            {
                Console.WriteLine($"Score: {score} -> Grade: A");
            }
            else if (score >= 80)
            {
                Console.WriteLine($"Score: {score} -> Grade: B");
            }
            else if (score >= 70)
            {
                Console.WriteLine($"Score: {score} -> Grade: C");
            }
            else if (score >= 60)
            {
                Console.WriteLine($"Score: {score} -> Grade: D");
            }
            else
            {
                Console.WriteLine($"Score: {score} -> Grade: F");
            }
        }
        else
        {
            Console.WriteLine("Invalid input! Please enter a integer number between 0 and 100.");
        }
    }
}