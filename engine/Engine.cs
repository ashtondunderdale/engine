using Engine;
using System.Reflection.Emit;

namespace engine;

internal class Engine
{
    public static List<Project> Projects = new();
    public static Project ActiveProject;

    public static void Launcher()
    {
        AddSampleProjectAndObject(); // for dev testing

        Console.WriteLine("Loaded Launcher.\n");
        Helpers.ReadClear();

        while (true)
        {
            Console.WriteLine("1. Load Projects\n2. Create a new project\n3. Delete a project\n4. Help\n5. Exit");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1": LoadProject(); break;

                case "2": CreateProject(); break;

                case "3": DeleteProject(); break;

                case "4": GameInfo.Help(); break;

                case "5":
                    if (Helpers.Exit()) continue; 
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
        List<Level> levels = new();
        //Level level = new("Level 1", objects, false);
        Level level = new("None", objects, false);

        Project project = new(projectName, projectDescription, ID, levels, level);
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

        if (ActiveProject.ActiveLevel.Objects.Count == 0) 
        {
            Helpers.OutputRed("\n\tThis project currently has no objects.");
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

    public static void ListLevels() 
    {
        if (ActiveProject.Levels.Count == 0) 
        {
            Helpers.OutputRed("\nThere are no levels to list.");
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

    public static void EditSpace()
    {
        while (true)
        {
            Console.WriteLine("Commands:\n\n" +
                "cr obj         - Create a new object for the currently active level\n" +
                "dl obj         - Removes an object from the currently active level\n" +
                "ld obj         - Displays all objects stored in the currently active level\n\n" +
                
                "cr lvl         - Creates a new level for the currently active project\n" +
                "dl lvl         - Removes a level from the currently active project\n" +
                "ld lvl         - Displays all levels stored in the currently active project\n" +
                "sl lvl         - Selects a level from the currently active project\n\n" +

                "pl             - Runs the current state of the game space\n" +
                "help           - Shows a list of commands + some other useful information\n\n" + // make user command input text coloured

                "rt             - Return to launcher");

            string input = Console.ReadLine().ToLower().Trim();

            switch (input) 
            {
                case "cr obj":
                    AddObject();
                    break;

                case "dl obj":
                    DeleteObject();
                    break;

                case "pl":
                    RunSpace();
                    break;

                case "ld obj":
                    ListObjects();
                    Helpers.ReadClear();
                    break;

                case "ld lvl":
                    ListLevels();
                    Helpers.ReadClear();
                    break;

                case "cr lvl":
                    AddLevel();
                    Helpers.ReadClear();
                    break;

                case "sl lvl":
                    SelectLevel();
                    Helpers.ReadClear();
                    break;

                case "dl lvl":
                    DeleteLevel();
                    Helpers.ReadClear();
                    break;

                case "rt":
                    Helpers.OutputYellow("\nReturning to Launcher.");
                    return;

                case "help":
                    HelpCommands();
                    Helpers.ReadClear();
                    break;

                default:
                    Helpers.OutputRed("\n\tNot a valid command.");
                    Helpers.ReadClear();
                    break;
            }
        }
    }

    public static void HelpCommands() 
    {
        Console.Clear();
        Helpers.OutputYellow("Help\n\n");
    }

    public static void AddObject()
    {
        while (true)
        {
            Console.WriteLine("\nEnter a command to add an object (something like: 'player myPlayer 0 0')\nYou can also enter \'block\' to place blocks");
            string command = Console.ReadLine();

            if (command == "block")
            {
                AddMultipleObjects(command);
                return;
            }

            else if (command == "item")
            {
                AddMultipleObjects(command);
                return;
            }

            else if (command == "win") 
            {
                AddMultipleObjects(command);
                return;
            }

            else if (command == "chaser")
            {
                AddMultipleObjects(command);
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

            ListObjects();

            Console.WriteLine("\nSelect the object to delete (or enter any key to go back):");
            string input = Console.ReadLine();

            bool found = false;
            foreach (var obj in ActiveProject.ActiveLevel.Objects.ToList())
            {
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
        if (ActiveProject.Levels.Count == 0)
        {
            Helpers.OutputRed("\n\tCreate a level first to run a game space.");
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
        Chaser chaser = ActiveProject.ActiveLevel.Objects.OfType<Chaser>().FirstOrDefault();

        bool inInventory = false;
        List<Item> pickedUpItems = new();
        bool gameSpaceActive = true;

        while (gameSpaceActive)
        {
            Console.Clear();

            if (inInventory)
            {
                DisplayInventory(player);
            }
            else
            {
                DisplaySpace();
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (!inInventory) MovePlayer(0, -1, pickedUpItems, ref gameSpaceActive);
                    break;

                case ConsoleKey.DownArrow:
                    if (!inInventory) MovePlayer(0, 1, pickedUpItems, ref gameSpaceActive);
                    break;

                case ConsoleKey.LeftArrow:
                    if (!inInventory) MovePlayer(-1, 0, pickedUpItems, ref gameSpaceActive);
                    break;

                case ConsoleKey.RightArrow:
                    if (!inInventory) MovePlayer(1, 0, pickedUpItems, ref gameSpaceActive);
                    break;

                case ConsoleKey.I:
                    ToggleInventory(player, ref inInventory);
                    break;

                case ConsoleKey.Escape:
                    Console.Clear();
                    Console.Write("C");
                    ResetPlayerPosition();
                    ResetPlayerInventory(pickedUpItems);
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
                    ResetPlayerPosition();
                    ResetPlayerInventory(pickedUpItems);
                    return;
                }
            }
        }

        ResetPlayerPosition();
        ResetPlayerInventory(pickedUpItems);
    }

    public static void AddLevel() 
    {
        Console.WriteLine("\nEnter the name of your new level");
        string input = Console.ReadLine();
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

            ListLevels();

            Console.WriteLine("\n\nSelect the level to delete (or enter any key to go back):");
            string input = Console.ReadLine();

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
                    Helpers.OutputGreen($"\n{level.Name} deleted successfully.");
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Helpers.OutputRed($"\n\tLevel with name '{input}' not found. Try again.");
                Helpers.ReadClear();
                return;
            }
            return;
        }
    }

    public static void SelectLevel() 
    {
        ListLevels();

        if (ActiveProject.Levels.Count == 0) 
        {
            Helpers.OutputRed("\nThere are no levels to load in this project.");
            return;
        }

        Console.WriteLine("\n\nChoose a level to edit or test");

        string userInput = Console.ReadLine();

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
            Helpers.OutputRed("\n\tInvalid input. Please enter a valid level number (or press any key to go back).");
            return;
        }
    }

    private static void ToggleInventory(Player player, ref bool inInventory)
    {
        inInventory = !inInventory;

        Console.Clear();

        if (inInventory)  DisplayInventory(player);
        else DisplaySpace();
    }

    private static void DisplayInventory(Player player)
    {
        Console.WriteLine("Inventory:");

        foreach (var item in player.Inventory) Console.WriteLine($"- {item.Name}");

        Console.WriteLine("\nPress 'i' again to close inventory.");
        Console.SetCursorPosition(player.X, player.Y);
    }



    private static void MovePlayer(int deltaX, int deltaY, List<Item> pickedUpItems, ref bool gameSpaceActive)
    {
        Player player = ActiveProject.ActiveLevel.Objects.OfType<Player>().FirstOrDefault();

        if (player != null)
        {
            int newX = player.X + deltaX;
            int newY = player.Y + deltaY;

            if (newX >= 0 && newX < Console.WindowWidth && newY >= 0 && newY < Console.WindowHeight)
            {
                bool tileBlocked = ActiveProject.ActiveLevel.Objects.OfType<Block>().Any(block =>
                    block.X == newX && block.Y == newY);

                if (!tileBlocked)
                {
                    player.X = newX;
                    player.Y = newY;

                    WinTile winTile = ActiveProject.ActiveLevel.Objects.OfType<WinTile>().FirstOrDefault(winTile =>
                        winTile.X == newX && winTile.Y == newY);

                    if (winTile is WinTile)
                    {
                        Console.Clear();
                        Helpers.OutputGreen("\n\tCongratulations! You won!");
                        Helpers.ReadClear();
                        ResetPlayerPosition();
                        ResetPlayerInventory(pickedUpItems);
                        gameSpaceActive = false;
                        return;
                    }

                    Item tileItem = ActiveProject.ActiveLevel.Objects.OfType<Item>().FirstOrDefault(item =>
                        item.X == newX && item.Y == newY);

                    if (tileItem != null)
                    {
                        player.Inventory.Add(tileItem);
                        ActiveProject.ActiveLevel.Objects.Remove(tileItem);
                        pickedUpItems.Add(tileItem);
                    }
                }
            }
        }
    }

    private static void ResetPlayerPosition()
    {
        Player player = ActiveProject.ActiveLevel.Objects.OfType<Player>().FirstOrDefault();
        Chaser chaser = ActiveProject.ActiveLevel.Objects.OfType<Chaser>().FirstOrDefault();

        if (player is not null)
        {
            player.X = player.OriginalX;
            player.Y = player.OriginalY;
        }

        if (chaser is not null)
        {
            chaser.X = chaser.OriginalX;
            chaser.Y = chaser.OriginalY;
        }
    }

    private static void ResetPlayerInventory(List<Item> pickedUpItems) 
    {
        Player player = ActiveProject.ActiveLevel.Objects.OfType<Player>().FirstOrDefault();
        if (player is not null) player.Inventory.Clear();

        foreach (Item item in pickedUpItems)
        {
            ActiveProject.ActiveLevel.Objects.Add(item);
        }
    }

    private static void DisplaySpace()
    {
        Console.SetCursorPosition(0, 28);
        Console.Write("GameSpace");

        Console.SetCursorPosition(0, 29);
        Console.Write($"Active: {ActiveProject.ActiveLevel.Name}");

        Console.SetCursorPosition(0, 28);
        Console.Write(new string('_', 120));

        Console.SetCursorPosition(0, 0);

        int windowWidth = Console.WindowWidth;
        int windowHeight = Console.WindowHeight;

        foreach (var obj in ActiveProject.ActiveLevel.Objects)
        {
            int x = obj.X;
            int y = obj.Y;

            x = Math.Max(0, Math.Min(x, windowWidth - 1));
            y = Math.Max(0, Math.Min(y, windowHeight - 1));

            Console.SetCursorPosition(x, y)
                ;
            if (obj is Player)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("P");
            }

            else if (obj is Block)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("+");
            }

            else if (obj is Chaser)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("0");
            }

            else if (obj is Item)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("i");
            }

            else if (obj is WinTile)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("X");
            }

            Console.ResetColor();
        }
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

        Project sampleProject = new("Test", "Test Description", "TESTID", levels, testLevel);
        Projects.Add(sampleProject);
    }

    public static void AddMultipleObjects(string objectType)
    {
        GameObject tempObject = null;

        while (true)
        {
            Console.Clear();
            DisplaySpace();

            Console.ForegroundColor = GetObjectColor(objectType);

            foreach (var obj in ActiveProject.ActiveLevel.Objects)
            {
                if (obj is GameObject gameObject && gameObject.Type == objectType)
                {
                    Console.SetCursorPosition(gameObject.X, gameObject.Y);
                    Console.Write(GetObjectSymbol(objectType));
                }
            }

            if (tempObject != null)
            {
                Console.SetCursorPosition(tempObject.X, tempObject.Y);
                Console.Write(GetObjectSymbol(objectType));
            }

            Console.ResetColor();

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    tempObject = CreateObject(tempObject, objectType, tempObject?.X ?? 0, tempObject?.Y - 1 ?? 0);
                    break;

                case ConsoleKey.DownArrow:
                    tempObject = CreateObject(tempObject, objectType, tempObject?.X ?? 0, tempObject?.Y + 1 ?? 0);
                    break;

                case ConsoleKey.LeftArrow:
                    tempObject = CreateObject(tempObject, objectType, tempObject?.X - 1 ?? 0, tempObject?.Y ?? 0);
                    break;

                case ConsoleKey.RightArrow:
                    tempObject = CreateObject(tempObject, objectType, tempObject?.X + 1 ?? 0, tempObject?.Y ?? 0);
                    break;

                case ConsoleKey.Enter:
                    if (tempObject is not null) ActiveProject.ActiveLevel.Objects.Add(tempObject);
                    break;

                case ConsoleKey.Escape:
                    Console.Clear();
                    return;

                default:
                    break;
            }
        }
    }


    private static ConsoleColor GetObjectColor(string objectType)
    {
        switch (objectType)
        {
            case "item":
                return ConsoleColor.Cyan;

            case "win":
                return ConsoleColor.Green;

            case "chaser":
                return ConsoleColor.Red;

            case "block":
                return ConsoleColor.White;

            default:
                return ConsoleColor.Black;
        }
    }

    private static char GetObjectSymbol(string objectType)
    {
        switch (objectType)
        {
            case "item":
                return 'i';

            case "win":
                return 'X';

            case "block":
                return '+';

            case "chaser":
                return '0';

            default:
                return ' ';
        }
    }

    private static GameObject CreateObject(GameObject existingObject, string objectType, int x, int y)
    {
        switch (objectType)
        {
            case "block":
                return new Block(x, y, objectType);

            case "item":
                return new Item(x, y, objectType);

            case "win":
                return new WinTile(x, y, objectType);

            case "chaser":
                return new Chaser(x, y, objectType);

            default:
                break;
        }

        return existingObject;
    }

}