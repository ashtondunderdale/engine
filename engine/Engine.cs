using Engine;
using System.Collections.Generic;

namespace engine;

internal class Engine
{
    public static List<Project> Projects = new();
    public static Project ActiveProject;

    private static int startingPlayerX;
    private static int startingPlayerY;

    public static void Launcher()
    {
        Console.WriteLine("Loaded Launcher.\n");
        Helpers.ReadClear();

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
                    if (Helpers.Exit()) 
                    { Console.Clear(); 
                        continue; 
                    }
                    else return;

                default: Console.Clear(); break;
            }
        }
    }
    
    public static void LoadProject()
    {
        if (Projects.Count == 0)
        {
            Helpers.OutputYellow("\nNo projects to display.");
            Helpers.ReadClear();
            return;
        }

        int projectIndex = 1;

        foreach (var project in Projects)
        {
            Helpers.OutputYellow($"\n{projectIndex} | {project.ID}\n  {project.Name}\n  {project.Description}\n\n");
            projectIndex++;
        }

        Console.WriteLine("\nSelect a project to load (or press any key to go back).");

        string userInput = Console.ReadLine();

        if (int.TryParse(userInput, out int input) && input >= 1 && input <= Projects.Count)
        {
            ActiveProject = Projects[input - 1];
            Console.Clear();
            Helpers.OutputYellow("Loaded Project.\n\n");
            EditSpace();
        }
        else
        {
            Helpers.OutputRed("\n\tInvalid input. Please enter a valid project number (or press any key to go back).");
        }

        Helpers.ReadClear();
    }

    public static void CreateProject()
    {
        string? projectName;

        do
        {
            Console.Write("\nProject Name: ");
            projectName = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(projectName))
            {
                Helpers.OutputRed("\n\tProject name cannot be empty.\n");
            }
            else if (Projects.Any(p => p.Name == projectName))
            {
                Helpers.OutputRed($"\n\tA project with the name '{projectName}' already exists. Please choose a different name.\n ");
                projectName = null;
            }

        } while (string.IsNullOrEmpty(projectName));

        string? projectDescription;

        do
        {
            Console.Write("\nProject Description: ");
            projectDescription = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(projectDescription))
            {
                Helpers.OutputRed("\n\tProject description cannot be empty.\n");
            }

        } while (string.IsNullOrEmpty(projectDescription));

        string ID = Helpers.GenerateString(8);

        List<GameObject> objects = new();
        Project project = new(projectName, projectDescription, objects, ID, false);
        Projects.Add(project);

        Helpers.OutputGreen("\n  Project: ");
        Helpers.OutputYellow($"'{project.Name}'");
        Helpers.OutputGreen(" has been created.");

        Helpers.ReadClear();
    }


    public static void DeleteProject()
    {
        if (Projects.Count == 0) 
        {
            Helpers.OutputYellow("\nNo projects to delete.");
            Helpers.ReadClear();
            return; 
        }

        while (true)
        {
            ListProjects();

            Console.WriteLine("\n\nEnter the name of the project to delete (or press any key to go back).");
            string input = Console.ReadLine();

            Project projectToDelete = Projects.FirstOrDefault(p => p.Name == input);

            if (projectToDelete != null)
            {
                Projects.Remove(projectToDelete);
                Helpers.OutputGreen("\n  Project: ");
                Helpers.OutputYellow($"'{input}'");
                Helpers.OutputGreen(" has been deleted.");
            }
            else
            {
                Helpers.OutputRed($"\n  Project '{input}' not found. Please enter a valid project name.");
            }

            Helpers.ReadClear();
            break;
        }
    }

    public static void ListObjects()
    {

        if (ActiveProject.Objects.Count == 0) 
        {
            Console.WriteLine("\nThis project currently has no objects.\nTry adding a player object to the space with \'add\'.\n\nPress enter.");
            return;
        }

        Console.WriteLine($"\nLoading objects for: {ActiveProject.Name}");

        foreach (var obj in ActiveProject.Objects)
        {
            Helpers.OutputYellow($"\n  Type: {obj.GetType().Name}\n");

            if (obj is Player playerObj) Helpers.OutputYellow($"  Name: {playerObj.Name}\n");
            else if (obj is Block blockObj) Helpers.OutputYellow($"  Name: {blockObj.Name}\n");
        }
    }

    public static void EditSpace()
    {
        while (true)
        {
            Console.WriteLine("Commands:\n\n\'add\' - to add an object into the space\n" +
                "\'del\' - To remove an object from the space\n" +
                "\'play\' - To test the game space\n" +
                "\'load\' - To display all current space objects" +
                "\n\n\'return\' - Return to launcher");

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
                    RunSpace();
                    break;

                case "load":
                    ListObjects();
                    Helpers.ReadClear();
                    break;

                case "return":
                    Helpers.OutputYellow("\nReturning to Launcher.");
                    return;

                default:
                    Helpers.OutputRed("\n\tNot a valid command.");
                    Helpers.ReadClear();
                    break;
            }
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
                    Helpers.OutputRed($"\n\tThe name '{objectName}' is already taken, choose a different object name.");
                    Helpers.ReadClear();
                    return;
                }

            } while (nameTaken);

            switch (input)
            {
                case "1":
                    if (ActiveProject.ContainsPlayerObject)
                    {
                        Helpers.OutputRed("\n\tYou can only add one player object.");
                        Helpers.ReadClear();
                        return;
                    }

                    Player player = new(0, 0, objectName);
                    ActiveProject.Objects.Add(player);
                    ActiveProject.ContainsPlayerObject = true;
                    break;

                case "2":
                    Block block = new(0, 0, objectName);
                    ActiveProject.Objects.Add(block);
                    break;

                case "3":
                    break;

                default:
                    Helpers.OutputRed("\n\tNot a valid object type");
                    Helpers.ReadClear();
                    return;
            }

            Helpers.OutputGreen("\n\tAdded object: ");
            Helpers.OutputYellow($"'{objectName}'");
            Helpers.ReadClear();
            return;
        }
    }



    public static void DeleteObject()
    {
        while (true)
        {
            if (ActiveProject.Objects.Count == 0)
            {
                Helpers.OutputYellow("\nNo objects available to delete.");
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
                    Helpers.OutputGreen($"\n{obj.Name} deleted successfully.");
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Helpers.OutputRed($"\n\tObject with name '{input}' not found. Try again.");
                Helpers.ReadClear();
                return;
            }
            Helpers.ReadClear();
            return;
        }
    }

    public static void ListProjects() 
    {
        if (Projects.Count == 0)
        {
            Helpers.OutputYellow("\nNo projects to display.");
            Helpers.ReadClear();
            return;
        }

        int projectIndex = 1;

        foreach (var project in Projects)
        {
            Helpers.OutputYellow($"\n{projectIndex} | {project.ID}\n  {project.Name}\n  {project.Description}\n\n");
            projectIndex++;
        }
    }

    public static void RunSpace()
    {
        if (!ActiveProject.ContainsPlayerObject)
        {
            Helpers.OutputRed("\n\tNo player object in the space. Add a player object before running the space.");
            Helpers.ReadClear();
            return;
        }

        Player player = ActiveProject.Objects.OfType<Player>().FirstOrDefault();

        if (player != null)
        {
            startingPlayerX = player.X;
            startingPlayerY = player.Y;
        }

        while (true)
        {
            Console.Clear();
            DisplaySpace();

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    MovePlayer(0, -1);
                    break;

                case ConsoleKey.DownArrow:
                    MovePlayer(0, 1);
                    break;

                case ConsoleKey.LeftArrow:
                    MovePlayer(-1, 0);
                    break;

                case ConsoleKey.RightArrow:
                    MovePlayer(1, 0);
                    break;

                case ConsoleKey.Escape:
                    Console.Clear(); Console.SetCursorPosition(0, 0); Console.Write("C");
                    ResetPlayerPosition();
                    return;

                default:
                    break;
            }
        }
    }

    private static void MovePlayer(int deltaX, int deltaY)
    {
        Player player = ActiveProject.Objects.OfType<Player>().FirstOrDefault();
        if (player != null)
        {
            int newX = player.X + deltaX;
            int newY = player.Y + deltaY;

            if (newX >= 0 && newX < Console.WindowWidth && newY >= 0 && newY < Console.WindowHeight)
            {
                player.X = newX;
                player.Y = newY;
            }
        }
    }

    private static void ResetPlayerPosition()
    {
        Player player = ActiveProject.Objects.OfType<Player>().FirstOrDefault();

        if (player is not null)
        {
            player.X = startingPlayerX;
            player.Y = startingPlayerY;
        }
    }

    private static void DisplaySpace()
    {
        Console.SetCursorPosition(0, 0);

        foreach (var obj in ActiveProject.Objects)
        {
            if (obj is Player playerObj)
            {
                Console.SetCursorPosition(playerObj.X, playerObj.Y);
                Console.Write("P");
            }
            else if (obj is Block blockObj)
            {
                Console.SetCursorPosition(blockObj.X, blockObj.Y);
                Console.Write("-");
            }
        }
    }


    public static void AddSampleProjectAndObject()
    {
        List<GameObject> objects = new();
        Player testPlayer = new(0, 0, "TestPlayer");
        objects.Add(testPlayer);
        Project sampleProject = new("Test", "Test Description", objects, "TESTID", true);
        Projects.Add(sampleProject);
    }
}