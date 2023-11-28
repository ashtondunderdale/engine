using Engine;
using System.Reflection.Emit;

namespace engine;

internal class Engine
{
    public static List<Project> Projects = new();
    public static Project ActiveProject;

    public static void Main() 
    {
        Helpers.OutputYellow("Loaded Launcher.\n");
        Helpers.ReadClear();

        Launcher();
    }

    public static void Launcher()
    {
        //AddSampleProjectAndObject(); // for dev testing
        while (true)
        {
            Console.Clear();

            Helpers.OutputYellow($"Engine Home\n");
            Console.WriteLine($"{ new string('_', 30)}\n");

            Console.Write ("1 |");
            Helpers.OutputYellow("\t Load Projects\n");

            Console.Write("2 |");
            Helpers.OutputYellow("\t Create New Project\n");

            Console.Write("3 |");
            Helpers.OutputYellow("\t Delete Project\n\n");

            Console.Write("4 |");
            Helpers.OutputYellow("\t Engine Help Guide\n\n");

            Console.Write("5 |");
            Helpers.OutputYellow("\t Exit Engine\n");

            string? input = Helpers.InputCyan();

            switch (input)
            {
                case "1": LoadProject(); 
                    break;

                case "2": CreateProject(); 
                    break;

                case "3": DeleteProject(); 
                    break;

                case "4": Help();
                    break;

                case "5": Helpers.Exit(); 
                    break;
                    
                default: Console.Clear(); 
                    break;
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
            Helpers.OutputYellow($"\n{projectIndex} | {project.Name}\n    {project.Description}\n\n");
            projectIndex++;
        }

        Console.WriteLine("\nSelect a project to load (or press any key to go back).");

        string userInput = Helpers.InputCyan();

        if (int.TryParse(userInput, out int input) && input >= 1 && input <= Projects.Count)
        {
            ActiveProject = Projects[input - 1];
            Console.Clear();
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
            projectName = Helpers.InputCyan().Trim();

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
            projectDescription = Helpers.InputCyan().Trim();

            if (string.IsNullOrEmpty(projectDescription))
            {
                Helpers.OutputRed("\n\tProject description cannot be empty.\n");
            }

        } while (string.IsNullOrEmpty(projectDescription));

        List<GameObject> objects = new();
        List<Level> levels = new();
        Level level = new("None", objects, false);

        Project project = new(projectName, projectDescription, levels, level);
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
            if (Projects.Count == 0)
            {
                Helpers.OutputYellow("\nNo projects to display.");
                Helpers.ReadClear();
                return;
            }

            int projectIndex = 1;

            foreach (var project in Projects)
            {
                Helpers.OutputYellow($"\n{projectIndex} | {project.Name}\n  {project.Description}\n\n");
                projectIndex++;
            }

            Console.WriteLine("\n\nEnter the name of the project to delete (or press any key to go back).");
            string input = Helpers.InputCyan();

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

    public static void Help()
    {
        Console.Clear();
        Helpers.OutputGreen("Welcome.");
        Console.WriteLine("\n\nThis is a 2D implementation of a basic game engine with generic game mechanics such as:\n");

        Helpers.OutputYellow("\t- Collision Detection\n" +
            "\t- Player Movement (built-in)\n" +
            "\t- Enemies with auto-movement\n\n" +
            "\t- Saveable projects\n\n\n");

        Console.Write("To add objects into your game space first,\n\n");

        Helpers.OutputYellow("\t1. Create a game space\n" +
        "\t2. Then, add a player object with: 'player playerName 10 10 (the 10, 10, are the players 'spawn coordinates')\n" +
        "\t3. Now, you can add objects and run your game space.");

        Helpers.ReadClear();
    }

    public static void EditSpace()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("Commands:\n\n" +
                "cr obj         - Create a new object for the currently active level\n" +
                "dl obj         - Removes an object from the currently active level\n" +
                "ld obj         - Displays all objects stored in the currently active level\n\n" +

                "cr lvl         - Creates a new level for the currently active project\n" +
                "dl lvl         - Removes a level from the currently active project\n" +
                "ld lvl         - Displays all levels stored in the currently active project\n" +
                "sl lvl         - Selects a level from the currently active project\n\n" +

                "pl             - Runs the current state of the game space\n" +
                "help           - Shows a list of commands + some other useful information\n\n" +

                "rt             - Return to launcher");

            Console.ForegroundColor = ConsoleColor.Cyan;
            string input = Helpers.InputCyan().ToLower().Trim();
            Console.ResetColor();

            switch (input)
            {
                case "cr obj":
                    CreateObject();
                    break;

                case "dl obj":
                    DeleteObject();
                    break;

                case "ld obj":
                    LoadObjects();
                    Helpers.ReadClear();
                    break;

                case "cr lvl":
                    CreateLevel();
                    Helpers.ReadClear();
                    break;

                case "dl lvl":
                    DeleteLevel();
                    Helpers.ReadClear();
                    break;

                case "ld lvl":
                    LoadLevels();
                    Helpers.ReadClear();
                    break;

                case "sl lvl":
                    SelectLevel();
                    Helpers.ReadClear();
                    break;

                case "pl":
                    RunSpace();
                    break;

                case "help":
                    HelpCommands();
                    Helpers.ReadClear();
                    break;

                case "rt":
                    Helpers.OutputYellow("\nReturning to Launcher.");
                    Launcher();
                    break;

                default:
                    Helpers.OutputRed("\n\tNot a valid command.");
                    Helpers.ReadClear();
                    break;
            }
        }
    }

    public static void CreateObject()
    {
        while (true)
        {
            if (ActiveProject.Levels.Count == 0)
            {
                Helpers.OutputYellow("\nCreate a level to add an object.");
                Helpers.ReadClear();
                return;
            }

            if (ActiveProject.ActiveLevel.Name == "None") 
            {
                Helpers.OutputYellow("\nSelect an active level to add an object.");
                Helpers.ReadClear();
                return;
            }

            Console.WriteLine("\nEnter a command to add an object (something like: 'player myPlayer 0 0')\nYou can also enter \'block\' to place blocks");
            string command = Helpers.InputCyan();

            if (command == "block")
            {
                Operations.AddMultipleObjects(command);
                return;
            }

            else if (command == "item")
            {
                Operations.AddMultipleObjects(command);
                return;
            }

            else if (command == "win")
            {
                Operations.AddMultipleObjects(command);
                return;
            }

            else if (command == "chaser")
            {
                Operations.AddMultipleObjects(command);
                return;
            }

            try
            {
                string[] parts = command.Split(' ');

                if (parts.Length < 4)
                {
                    Helpers.OutputRed("\n\tInvalid command. Please provide all required parameters.");
                    Helpers.ReadClear();
                    return;
                }

                string objectType = parts[0];
                string objectName = parts[1];
                int startingX = Convert.ToInt32(parts[2]);
                int startingY = Convert.ToInt32(parts[3]);

                if (ActiveProject.ActiveLevel.Objects.Any(obj => obj.Name == objectName))
                {
                    Helpers.OutputRed($"\n\tThe name '{objectName}' is already taken, choose a different object name.");
                    Helpers.ReadClear();
                    return;
                }

                switch (objectType.ToLower())
                {
                    case "player":
                        if (ActiveProject.ActiveLevel.ContainsPlayerObject)
                        {
                            Helpers.OutputRed("\n\tYou can only add one player object.");
                            Helpers.ReadClear();
                            return;
                        }

                        Player player = new(startingX, startingY, objectName);
                        ActiveProject.ActiveLevel.Objects.Add(player);
                        ActiveProject.ActiveLevel.ContainsPlayerObject = true;
                        break;

                    case "chaser":
                        Chaser chaser = new(startingX, startingY, objectName);
                        ActiveProject.ActiveLevel.Objects.Add(chaser);
                        break;

                    case "item":
                        Item item = new(startingX, startingY, objectName);
                        ActiveProject.ActiveLevel.Objects.Add(item);
                        break;

                    case "win":
                        WinTile winTile = new(startingX, startingY, objectName);
                        ActiveProject.ActiveLevel.Objects.Add(winTile);
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
            catch (Exception ex)
            {
                Helpers.OutputRed($"\n\tError: {ex.Message}");
                Helpers.ReadClear();
                return;
            }
        }
    }

    public static void DeleteObject()
    {
        while (true)
        {
            if (ActiveProject.ActiveLevel.Objects.Count == 0)
            {
                Helpers.OutputYellow("\nNo objects available to delete.");
                Helpers.ReadClear();
                return;
            }

            LoadObjects();

            Console.WriteLine("\nSelect the object to delete (or enter any key to go back):");
            string input = Helpers.InputCyan();

            bool found = false;
            var objectsList = ActiveProject.ActiveLevel.Objects.ToList();

            for (int i = objectsList.Count - 1; i >= 0; i--)
            {
                var obj = objectsList[i];
                if (obj.Name == input)
                {
                    ActiveProject.ActiveLevel.Objects.Remove(obj);
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

    public static void LoadObjects()
    {

        if (ActiveProject.ActiveLevel.Objects.Count == 0) 
        {
            Helpers.OutputYellow("\nThis project currently has no objects.");
            return;
        }

        Console.WriteLine($"\nLoading objects for: {ActiveProject.Name}");

        foreach (var obj in ActiveProject.ActiveLevel.Objects)
        {
            Helpers.OutputYellow($"\n  Type: {obj.GetType().Name}\n");

            if (obj is Player playerObj) Helpers.OutputYellow($"  Name: {playerObj.Name}\n");
            else if (obj is Block blockObj) Helpers.OutputYellow($"  Name: {blockObj.Name}\n");
            else if (obj is Chaser chaserObj) Helpers.OutputYellow($"  Name: {chaserObj.Name}\n");
            else if (obj is Item itemObj) Helpers.OutputYellow($"  Name: {itemObj.Name}\n");
            else if (obj is WinTile winTileObj) Helpers.OutputYellow($"  Name: {winTileObj.Name}\n");

        }
    }

    public static void CreateLevel()
    {
        Console.WriteLine("\nEnter the name of your new level");
        string input = Helpers.InputCyan();

        if (string.IsNullOrEmpty(input)) 
        {
            Helpers.OutputRed("\n\tEnter a valid name for your level");
            return;
        }

        if (ActiveProject.Levels.Any(level => level.Name == input)) 
        {
            Helpers.OutputRed("\n\tA level with this name has already been created");
            return;
        }

        List<GameObject> newObjectList = new();

        Level newLevel = new(input, newObjectList, false);
        ActiveProject.Levels.Add(newLevel);
        Helpers.OutputGreen($"\nAdded level");
        Helpers.OutputYellow($" \'{newLevel.Name}\'");
    }

    public static void DeleteLevel()
    {
        while (true)
        {
            if (ActiveProject.Levels.Count == 0)
            {
                Helpers.OutputYellow("\nNo levels available to delete.");
                return;
            }

            LoadLevels();

            Console.WriteLine("\n\nSelect the level to delete (or enter any key to go back):");
            string input = Helpers.InputCyan();

            bool found = false;
            foreach (var level in ActiveProject.Levels.ToList())
            {
                if (level.Name == input)
                {
                    if (ActiveProject.ActiveLevel.Name == level.Name)
                    {
                        ActiveProject.ActiveLevel.Name = "None";
                    }

                    ActiveProject.Levels.Remove(level);
                    Helpers.OutputYellow($"\nLevel: \'{level.Name}\' ");
                    Helpers.OutputGreen("deleted successfully.");
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Helpers.OutputRed($"\n\tLevel with name '{input}' not found. Try again.");
                return;
            }
            return;
        }
    }

    public static void LoadLevels() 
    {
        if (ActiveProject.Levels.Count == 0) 
        {
            Helpers.OutputYellow("\nThere are no levels to list.");
        }

        Console.WriteLine($"\n\nLoading levels for: {ActiveProject.Name}");

        Helpers.OutputYellow($"\n\tActive Level:");
        Helpers.OutputGreen($" \'{ActiveProject.ActiveLevel.Name}\'\n\n");

        int index = 0;

        foreach (var level in ActiveProject.Levels)
        {
            index++;
            Helpers.OutputYellow($"\n\t  {index} | {level.Name}");
        }
    }

    public static void SelectLevel()
    {
        LoadLevels();

        if (ActiveProject.Levels.Count == 0)
        {
            Helpers.OutputYellow("\nThere are no levels to load in this project.");
            return;
        }

        Console.WriteLine("\n\nChoose a level to edit or test");

        string userInput = Helpers.InputCyan();

        if (int.TryParse(userInput, out int input) && input >= 1 && input <= ActiveProject.Levels.Count)
        {
            ActiveProject.ActiveLevel = ActiveProject.Levels[input - 1];
            Helpers.OutputGreen($"\nSuccessfully Loaded Level:");
            Helpers.OutputYellow($" \'{ActiveProject.ActiveLevel.Name}\'\n\n");
            Helpers.ReadClear();
            EditSpace();
        }
        else
        {
            Helpers.OutputYellow("\n\tEnter a valid level number (or press any key to go back).");
            return;
        }
    }

    public static void RunSpace()
    {
        if (ActiveProject.Levels.Count == 0)
        {
            Helpers.OutputRed("\n\tCreate a level to run this game space.");
            Helpers.ReadClear();
            return;
        }

        if (ActiveProject.ActiveLevel.Name == "None") 
        {
            Helpers.OutputRed("\n\tSelect an active level to run this game space.");
            Helpers.ReadClear();
            return;
        }

        if (!ActiveProject.ActiveLevel.Objects.OfType<Player>().Any())
        {
            Helpers.OutputRed("\n\tNo player object in the space. Add a player object before running the space.");
            Helpers.ReadClear();
            return;
        }

        Player player = ActiveProject.ActiveLevel.Objects.OfType<Player>().FirstOrDefault();

        bool inInventory = false;
        List<Item> pickedUpItems = new();
        bool gameSpaceActive = true;

        while (gameSpaceActive)
        {
            Console.Clear();

            if (inInventory)
            {
                Operations.DisplayInventory(player);
            }
            else
            {
                Operations.DisplaySpace();
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (!inInventory) Operations.MovePlayer(0, -1, pickedUpItems, ref gameSpaceActive);
                    break;

                case ConsoleKey.DownArrow:
                    if (!inInventory) Operations.MovePlayer(0, 1, pickedUpItems, ref gameSpaceActive);
                    break;

                case ConsoleKey.LeftArrow:
                    if (!inInventory) Operations.MovePlayer(-1, 0, pickedUpItems, ref gameSpaceActive);
                    break;

                case ConsoleKey.RightArrow:
                    if (!inInventory) Operations.MovePlayer(1, 0, pickedUpItems, ref gameSpaceActive);
                    break;

                case ConsoleKey.I:
                    Operations.ToggleInventory(player, ref inInventory);
                    break;

                case ConsoleKey.Escape:
                    Console.Clear();
                    Console.Write("C");
                    Operations.ResetPlayerPosition();
                    Operations.ResetPlayerInventory(pickedUpItems);
                    return;

                default:
                    break;
            }

            if (ActiveProject.ActiveLevel.Objects.OfType<Chaser>().Any())
            {
                foreach (Chaser chaserObj in ActiveProject.ActiveLevel.Objects.OfType<Chaser>())
                {
                    chaserObj?.ChasePlayer(player, ActiveProject.ActiveLevel.Objects);
                }
            }

            foreach (Chaser chaserObj in ActiveProject.ActiveLevel.Objects.OfType<Chaser>())
            {
                if (player != null && chaserObj != null && player.X == chaserObj.X && player.Y == chaserObj.Y)
                {
                    Console.Clear();
                    Helpers.OutputRed("\n\tPlayer caught by chaser. Game over!");
                    Helpers.ReadClear();
                    Operations.ResetPlayerPosition();
                    Operations.ResetPlayerInventory(pickedUpItems);
                    return;
                }
            }
        }

        Operations.ResetPlayerPosition();
        Operations.ResetPlayerInventory(pickedUpItems);
    }

    public static void HelpCommands() 
    {
        Console.Clear();
        Helpers.OutputYellow("Help\n\n");
    } 

    public static void AddSampleProjectAndObject()
    {
        List<GameObject> objects = new();

        Player player = new(0, 0, "player");
        objects.Add(player);

        Chaser chaser = new(player.X + 25, player.Y, "chaser");
        objects.Add(chaser);

        Item item = new(10, 10, "item");
        objects.Add(item);

        WinTile winTile = new(10, 12, "item");
        objects.Add(winTile);

        Level testLevel = new("Level 1", objects, false);
        List<Level> levels = new()
        {
            testLevel
        };

        Project sampleProject = new("Test", "Test Description", levels, testLevel);
       Projects.Add(sampleProject);
    }
}