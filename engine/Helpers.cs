namespace engine;

internal class Helpers
{
    public static void OutputGreen(string message) 
    { 
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static string GenerateString(int length) 
    {
        Random random = new();
        return new(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 8)
                                                .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public static bool Exit()
    {
        while (true)
        {
            Console.WriteLine("\nConfirm exit (y / n)");
            string? input = Console.ReadLine().ToLower();

            if (input != "y") return true;
            else if (input != "n") return false;
            else continue;
        }
    }
}
