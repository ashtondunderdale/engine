using System.Collections.Generic;

namespace engine;

internal class Engine
{
    public static List<Project> Projects = new();
    public static Project ActiveProject;
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
                case "1": LoadProject(); break;

                case "2": CreateProject(); break;

                case "3":
                    if (Helpers.Exit()) { Console.Clear(); continue; }
                    else return;

                default: Console.Clear(); break;
            }
        }
    }
    
    public static void LoadProject()
    {
        if (Projects.Count == 0)
        {
            Console.WriteLine("\nNo projects to display.");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        int projectIndex = 1;

        foreach (var project in Projects)
        {
            Console.WriteLine($"\n{project.ID}\n  {project.Name}\n  {project.Description}");
            projectIndex++;
        }

        Console.WriteLine($"\nSelect project to load or enter 0 to exit:");

        if (int.TryParse(Console.ReadLine(), out int input) && input >= 1 && input <= Projects.Count)
        {
            ActiveProject = Projects[input - 1];
            Console.WriteLine($"\nLoading project: {ActiveProject.Name}");
        }
        else if (input == 0) Console.WriteLine("\nExiting LoadProjects.");
        else Console.WriteLine("\nInvalid input. Please enter a valid project number or 0 to exit.");

        Console.ReadKey();
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
}