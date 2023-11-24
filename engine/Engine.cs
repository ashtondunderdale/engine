namespace engine;

internal class Engine
{
    public static List<Project> Projects = new List<Project>();
    public static void RunEngine()
    {
        Console.WriteLine("Loaded Engine.");
        Console.ReadKey(); Console.Clear();
    }

    public static void Launcher()
    {
        while (true)
        {
            Console.WriteLine("1. Load Projects\n2. Create a new project\n3. Exit");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1": LoadProjects(); break;

                case "2": CreateProject(); break;

                case "3":
                    if (Exit()) { Console.Clear(); continue; }
                    else return;

                default: Console.Clear(); break;
            }
        }

    }

    public static void LoadProjects()
    {
        if (Projects.Count == 0) Console.WriteLine("\nNo projects to display.");

        foreach (var project in Projects) Console.WriteLine($"\n{project.ID}\n  {project.Name}\n  {project.Description}");

        string? input = Console.ReadLine();
        Console.Clear();
    }

    public static void CreateProject()
    {
        string? projectName;

        do
        {
            Console.Write("\nProject Name: ");
            projectName = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(projectName)) Console.WriteLine("Project name cannot be empty.");

        } while (string.IsNullOrEmpty(projectName));

        string? projectDescription;

        do
        {
            Console.Write("\nProject Description: ");
            projectDescription = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(projectDescription)) Console.WriteLine("Project description cannot be empty.");

        } while (string.IsNullOrEmpty(projectDescription));

        string ID = Helpers.GenerateString(8);

        List<Object> objects = new();
        Project project = new(projectName, projectDescription, objects, ID);
        Projects.Add(project);

        Helpers.OutputGreen("\n  Project added");
        Console.ReadKey(); Console.Clear();
    }

    public static bool Exit() 
    { 
        while (true) 
        {
            Console.WriteLine("Confirm exit (y / n)");
            string? input = Console.ReadLine().ToLower();

            if (input != "y") return true;
            else if (input != "n") return false;
            else continue;
        }
    }
}
