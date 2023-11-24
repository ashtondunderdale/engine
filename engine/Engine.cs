using Engine;
using System.Collections.Generic;

namespace engine;

internal class Engine
{
    public static List<Project> Projects = new();
    public static Project ActiveProject;

    public static void Launcher()
    {
        Console.WriteLine("Loaded Launcher.\n");
        Console.ReadKey(); Console.Clear();

        while (true)
        {
            Console.WriteLine("1. Load Projects\n2. Create a new project\n3. Delete a project\n4. Exit");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1": LoadProject(); break;

                case "2": CreateProject(); break;

                case "3": DeleteProject(); break;

                case "4":
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
            Console.ReadKey(); Console.Clear();
            return;
        }

        int projectIndex = 1;

        foreach (var project in Projects)
        {
            Console.WriteLine($"\n{project.ID}\n  {project.Name}\n  {project.Description}");
            projectIndex++;
        }

        Console.WriteLine("\nSelect a project to load (or enter 'r' to go back).");

        string userInput = Console.ReadLine();

        if (userInput.ToLower() == "r")
        {
            Console.Clear();
            return;
        }
        else if (int.TryParse(userInput, out int input) && input >= 1 && input <= Projects.Count)
        {
            ActiveProject = Projects[input - 1];
            LoadObjects();
            RunGameEngine();
        }
        else
        {
            Console.WriteLine("\nInvalid input. Please enter a valid project number or 'r' to go back.");
        }

        Console.ReadKey(); Console.Clear();
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

        List<GameObject> objects = new();
        Project project = new(projectName, projectDescription, objects, ID);
        Projects.Add(project);

        Helpers.OutputGreen("\n  Project added");
        Console.ReadKey(); Console.Clear();
    }

    public static void DeleteProject()
    {
        if (Projects.Count == 0) 
        { 
            Console.WriteLine("\nNo projects to delete.");
            Console.ReadKey(); Console.Clear();
            return; 
        }

        while (true)
        {
            Console.WriteLine("\nEnter the name of the project to delete (or enter 'r' to go back).");
            string input = Console.ReadLine();

            if (input == "r")
            {
                Console.Clear();
                break;
            }

            Project projectToDelete = Projects.FirstOrDefault(p => p.Name == input);

            if (projectToDelete != null)
            {
                Projects.Remove(projectToDelete);
                Console.WriteLine($"\n  Project '{input}' has been deleted.");
            }
            else
            {
                Console.WriteLine($"\n  Project '{input}' not found. Please enter a valid project name.");
            }

            Console.ReadKey(); Console.Clear();
            break;
        }
    }

    public static void LoadObjects()
    {
        Console.WriteLine($"\nLoading objects: {ActiveProject.Name}");

        GameObject player = new Player(0, 0, "PlayerObject");
        ActiveProject.Objects.Add(player);

        foreach (var obj in ActiveProject.Objects)
        {
            Console.WriteLine($"\n  Type: {obj.GetType().Name}");

            if (obj is Player playerObj) Console.WriteLine($"  Name: {playerObj.Name}\n");
            else if (obj is Block blockObj) Console.WriteLine($"  Name: {blockObj.Name}\n");
        }
        Console.ReadKey(); Console.Clear();
    }

    public static void RunGameEngine()
    {

    }
}
