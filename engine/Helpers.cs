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

    public static void Exit()
    {
        while (true)
        {
            Console.WriteLine("\nConfirm exit (y / n)");
            string? input = InputCyan().ToLower();

            if (input != "y")
            {
                OutputYellow("\nReturning to Launcher.");
                ReadClear();
                break;
            }
            else if (input != "n")
            {
                OutputYellow("\nExiting Now.");
                Environment.Exit(0);
            }
            else continue;
        }
    }

    public static void ReadClear() 
    {
        Console.ReadKey();
        Console.Clear();
    }

    public static void OutputAsTyped(string inputString, int delay, ConsoleColor colour)
    {
        Console.ForegroundColor= ConsoleColor.Yellow;
        foreach (char c in inputString)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }

        Console.ResetColor();
    }
}
