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
            Console.WriteLine($"\n{projectIndex} | {project.ID}\n  {project.Name}\n  {project.Description}");
            projectIndex++;
        }

        Console.WriteLine("\nSelect a project to load (or press any key to go back).");

        string userInput = Console.ReadLine();

        if (int.TryParse(userInput, out int input) && input >= 1 && input <= Projects.Count)
        {
            ActiveProject = Projects[input - 1];
            Console.Clear();
            Console.WriteLine("Loaded Project.\n");
            ListObjects();
            RunSpace();
        }
        else
        {
            Console.WriteLine("\nInvalid input. Please enter a valid project number (or press any key to go back).");
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
            ListProjects();

            Console.WriteLine("\nEnter the name of the project to delete (or press any key to go back).");
            string input = Console.ReadLine();

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

    public static void ListObjects()
    {

        if (ActiveProject.Objects.Count == 0) 
        {
            Console.WriteLine("\nThis project currently has no objects.\nTry adding a player object to the space with \'add\'.\n\nPress enter.");
            Console.ReadKey(); Console.Clear();
            return;
        }

        Console.WriteLine($"\nLoading objects for: {ActiveProject.Name}");

        foreach (var obj in ActiveProject.Objects)
        {
            Console.WriteLine($"\n  Type: {obj.GetType().Name}");

            if (obj is Player playerObj) Console.WriteLine($"  Name: {playerObj.Name}\n");
            else if (obj is Block blockObj) Console.WriteLine($"  Name: {blockObj.Name}\n");
        }
    }

    public static void RunSpace()
    {
        while (true)
        {
            Console.WriteLine("Commands:\n\n\'add\' - to add an object into the space\n" +
                "\'del\' - To remove an object from the space\n" +
                "\'play\' - To test the game space\n" +
                "\'load\' - To display all current space objects");

            string input = Console.ReadLine().ToLower().Trim();

            switch (input) 
            {
                case "add":
                    AddObject();
                    break;

                case "del":
                    DeleteObject();
                    break;

                case "play":

                    break;

                case "load":
                    ListObjects();
                    Console.ReadKey(); Console.Clear();
                    break;

                default:
                    Console.WriteLine("Not a valid command.");
                    Console.ReadKey(); Console.Clear();
                    break;
            }

            // add objects
            // remove objects, via list index of object + 1
            // pl, play game
        }
    }

    public static void AddObject()
    {
        while (true)
        {
            Console.WriteLine("\nSelect object type to add.\n\n1. Player\n2. Block");
            string input = Console.ReadLine();

            string objectName = "";
            bool nameTaken = false;

            do
            {
                Console.WriteLine("\nEnter a name for your object");
                objectName = Console.ReadLine();

                nameTaken = ActiveProject.Objects.Any(obj => obj.Name == objectName);

                if (nameTaken)
                {
                    Console.WriteLine($"\nThe name '{objectName}' is already taken, choose a different object name.");
                    Console.ReadKey(); Console.Clear();
                    return;
                }

            } while (nameTaken);

            switch (input)
            {
                case "1":
                    Player player = new(0, 0, objectName);
                    ActiveProject.Objects.Add(player);
                    break;

                case "2":
                    Block block = new(0, 0, objectName);
                    ActiveProject.Objects.Add(block);
                    break;

                case "3":
                    break;

                default:
                    Console.WriteLine("\nNot a valid object type");
                    Console.ReadKey(); Console.Clear();
                    return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n\tAdded object: {objectName}");
            Console.ResetColor();

            Console.ReadKey(); Console.Clear();
            return;
        }
    }


    public static void DeleteObject()
    {
        while (true)
        {
            if (ActiveProject.Objects.Count == 0)
            {
                Console.WriteLine("\nNo objects available to delete.");
                return;
            }

            ListObjects();

            Console.WriteLine("\nSelect the object to delete (or enter any key to go back):");
            string input = Console.ReadLine();

            bool found = false;
            foreach (var obj in ActiveProject.Objects.ToList())
            {
                if (obj.Name == input)
                {
                    ActiveProject.Objects.Remove(obj);
                    Console.WriteLine($"\n{obj.Name} deleted successfully.");
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine($"\nObject with name '{input}' not found. Try again.");
                Console.ReadKey(); Console.Clear();
                return;
            }
            Console.ReadKey(); Console.Clear();
            return;
        }
    }

    public static void ListProjects() 
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
            Console.WriteLine($"\n{projectIndex} | {project.ID}\n  {project.Name}\n  {project.Description}");
            projectIndex++;
        }
    }
}
