namespace engine;

internal class Helpers
{
    public static void OutputGreen(string message) 
    { 
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(message);
        Console.ResetColor();
    }

    public static void OutputRed(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(message);
        Console.ResetColor();
    }

    public static void OutputYellow(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(message);
        Console.ResetColor();
    }

    public static string InputCyan() 
    { 
        Console.ForegroundColor = ConsoleColor.Cyan;
        string input = Console.ReadLine();
        Console.ResetColor();

        return input;
    }

    public static bool Exit()
    {
        while (true)
        {
            Console.WriteLine("\nConfirm exit (y / n)");
            string? input = Console.ReadLine().ToLower();

            if (input != "y")
            {
                OutputYellow("\nReturning to Launcher.");
                ReadClear();
                return true;
            }
            else if (input != "n")
            {
                OutputYellow("\nExiting Now.");
                return false; 
            }
            else continue;
        }
    }

    public static void ReadClear() 
    {
        Console.ReadKey();
        Console.Clear();
    }
}
