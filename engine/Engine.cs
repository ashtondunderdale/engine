namespace engine;

internal class Engine
{
    public static void RunEngine() 
    {
        Console.WriteLine("Loaded Engine");
        Console.ReadKey(); Console.Clear();
    }

    public static void Launcher() 
    {
        while (true)
        {
            Console.WriteLine($"{new string('_', 20)}\n\n1. Load Projects\n2. Exit Launcher");

            string? input = Console.ReadLine();

            if (input == "1") LoadProjects();
            else if (input == "2") break;

        }
    }

    public static void LoadProjects() 
    { 
        
    }
}
