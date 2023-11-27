using Engine;

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
            else if (obj is Chaser chaserObj) Helpers.OutputYellow($"  Name: {chaserObj.Name}\n");
            else if (obj is Item itemObj) Helpers.OutputYellow($"  Name: {itemObj.Name}\n");
            else if (obj is WinTile winTileObj) Helpers.OutputYellow($"  Name: {winTileObj.Name}\n");

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
            Console.WriteLine("\nEnter a command to add an object (something like: 'player myPlayer 0 0')\nYou can also enter \'block\' to place blocks");
            string command = Console.ReadLine();

            if (command == "block")
            {
                AddBlocks();
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

                if (ActiveProject.Objects.Any(obj => obj.Name == objectName))
                {
                    Helpers.OutputRed($"\n\tThe name '{objectName}' is already taken, choose a different object name.");
                    Helpers.ReadClear();
                    return;
                }

                switch (objectType.ToLower())
                {
                    case "player":
                        if (ActiveProject.ContainsPlayerObject)
                        {
                            Helpers.OutputRed("\n\tYou can only add one player object.");
                            Helpers.ReadClear();
                            return;
                        }

                        Player player = new(startingX, startingY, objectName);
                        ActiveProject.Objects.Add(player);
                        ActiveProject.ContainsPlayerObject = true;
                        break;

                    case "chaser":
                        Chaser chaser = new(startingX, startingY, objectName);
                        ActiveProject.Objects.Add(chaser);
                        break;

                    case "item":
                        Item item = new(startingX, startingY, objectName);
                        ActiveProject.Objects.Add(item);
                        break;

                    case "win":
                        WinTile winTile = new(startingX, startingY, objectName);
                        ActiveProject.Objects.Add(winTile);
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
            if (ActiveProject.Objects.Count == 0)
            {
                Helpers.OutputYellow("\nNo objects available to delete.");
                Helpers.ReadClear();
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
        if (!ActiveProject.Objects.OfType<Player>().Any())
        {
            Helpers.OutputRed("\n\tNo player object in the space. Add a player object before running the space.");
            Helpers.ReadClear();
            return;
        }

        Player player = ActiveProject.Objects.OfType<Player>().FirstOrDefault();
        Chaser chaser = ActiveProject.Objects.OfType<Chaser>().FirstOrDefault();

        bool inInventory = false;
        List<Item> pickedUpItems = new();
        bool gameSpaceActive = true; // for when the user goes on a win tile

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

            if (ActiveProject.Objects.OfType<Chaser>().Any())
            {
                foreach (Chaser chaserObj in ActiveProject.Objects.OfType<Chaser>())
                {
                    chaserObj?.ChasePlayer(player, ActiveProject.Objects);
                }
            }

            if (player != null && chaser != null && player.X == chaser.X && player.Y == chaser.Y)
            {
                Console.Clear();
                Helpers.OutputRed("\n\tPlayer caught by chaser. Game over!");
                Helpers.ReadClear();
                ResetPlayerPosition();
                ResetPlayerInventory(pickedUpItems);
                return;
            }
        }

        ResetPlayerPosition();
        ResetPlayerInventory(pickedUpItems);
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
        Player player = ActiveProject.Objects.OfType<Player>().FirstOrDefault();

        if (player != null)
        {
            int newX = player.X + deltaX;
            int newY = player.Y + deltaY;

            if (newX >= 0 && newX < Console.WindowWidth && newY >= 0 && newY < Console.WindowHeight)
            {
                bool tileBlocked = ActiveProject.Objects.OfType<Block>().Any(block =>
                    block.X == newX && block.Y == newY);

                if (!tileBlocked)
                {
                    player.X = newX;
                    player.Y = newY;

                    WinTile winTile = ActiveProject.Objects.OfType<WinTile>().FirstOrDefault(winTile =>
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

                    Item tileItem = ActiveProject.Objects.OfType<Item>().FirstOrDefault(item =>
                        item.X == newX && item.Y == newY);

                    if (tileItem != null)
                    {
                        player.Inventory.Add(tileItem);
                        ActiveProject.Objects.Remove(tileItem);
                        pickedUpItems.Add(tileItem);
                    }
                }
            }
        }
    }




    private static void ResetPlayerPosition()
    {
        Player player = ActiveProject.Objects.OfType<Player>().FirstOrDefault();
        Chaser chaser = ActiveProject.Objects.OfType<Chaser>().FirstOrDefault();

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
        Player player = ActiveProject.Objects.OfType<Player>().FirstOrDefault();
        if (player is not null) player.Inventory.Clear();

        foreach (Item item in pickedUpItems)
        {
            ActiveProject.Objects.Add(item);
        }
    }

    private static void DisplaySpace()
    {
        Console.SetCursorPosition(0, 0);

        int windowWidth = Console.WindowWidth;
        int windowHeight = Console.WindowHeight;

        foreach (var obj in ActiveProject.Objects)
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

        Project sampleProject = new("Test", "Test Description", objects, "TESTID", true);
        Projects.Add(sampleProject);
    }

    public static void AddBlocks()
    {
        Block tempBlock = null;

        while (true)
        {
            Console.Clear();

            foreach (var block in ActiveProject.Objects.OfType<Block>())
            {
                Console.SetCursorPosition(block.X, block.Y);
                Console.Write("+");
            }

            if (tempBlock != null)
            {
                Console.SetCursorPosition(tempBlock.X, tempBlock.Y);
                Console.Write("+");
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (tempBlock == null || tempBlock.Y > 0) tempBlock = new Block(tempBlock?.X ?? 0, tempBlock?.Y - 1 ?? 0, "Block");
                    break;

                case ConsoleKey.DownArrow:
                    if (tempBlock == null || tempBlock.Y < Console.WindowHeight - 1) tempBlock = new Block(tempBlock?.X ?? 0, tempBlock?.Y + 1 ?? 0, "Block");
                    break;

                case ConsoleKey.LeftArrow:
                    if (tempBlock == null || tempBlock.X > 0) tempBlock = new Block(tempBlock?.X - 1 ?? 0, tempBlock?.Y ?? 0, "Block");
                    break;

                case ConsoleKey.RightArrow:
                    if (tempBlock == null || tempBlock.X < Console.WindowWidth - 1) tempBlock = new Block(tempBlock?.X + 1 ?? 0, tempBlock?.Y ?? 0, "Block");
                    break;

                case ConsoleKey.Enter:
                    if (tempBlock is not null) ActiveProject.Objects.Add(tempBlock);
                    break;

                case ConsoleKey.Escape: Console.Clear();
                    return;

                default: break;
            }
        }
    }
}