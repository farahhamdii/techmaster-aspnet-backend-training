using System;

namespace Task_01_csharp_drills.Drills;

public static class Drill15_ArrayRotation
{
    public static void Run()
    {
        int[] arr = { 1, 2, 3, 4, 5 };

        Console.WriteLine($"Original Array: [{string.Join(", ", arr)}]");

        if (arr.Length > 1)
        {
            int lastElement = arr[arr.Length - 1];

            for (int i = arr.Length - 1; i > 0; i--)
            {
                arr[i] = arr[i - 1];
            }

            arr[0] = lastElement;
        }

        Console.WriteLine($"Rotated Array : [{string.Join(", ", arr)}]");
    }
}