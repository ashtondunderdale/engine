using Engine;

namespace engine;

internal class Operations
{
    public static void MovePlayer(int deltaX, int deltaY, List<Item> pickedUpItems, ref bool gameSpaceActive)
    {
        Player player = Engine.ActiveProject.ActiveLevel.Objects.OfType<Player>().FirstOrDefault();

        if (player != null)
        {
            int newX = player.X + deltaX;
            int newY = player.Y + deltaY;

            if (newX >= 0 && newX < Console.WindowWidth && newY >= 0 && newY < Console.WindowHeight)
            {
                bool tileBlocked = Engine.ActiveProject.ActiveLevel.Objects.OfType<Block>().Any(block =>
                    block.X == newX && block.Y == newY);

                if (!tileBlocked)
                {
                    player.X = newX;
                    player.Y = newY;

                    WinTile winTile = Engine.ActiveProject.ActiveLevel.Objects.OfType<WinTile>().FirstOrDefault(winTile =>
                        winTile.X == newX && winTile.Y == newY);

                    if (winTile is WinTile)
                    {
                        Console.Clear();
                        Helpers.OutputGreen("\n\tCongratulations! You won!");
                        Helpers.ReadClear();
                        ResetPlayerPosition();
                        ResetPlayerInventory(pickedUpItems);
                        gameSpaceActive = false;
                        Console.Write("e");
                        return;
                    }

                    Item tileItem = Engine.ActiveProject.ActiveLevel.Objects.OfType<Item>().FirstOrDefault(item =>
                        item.X == newX && item.Y == newY);

                    if (tileItem != null)
                    {
                        player.Inventory.Add(tileItem);
                        Engine.ActiveProject.ActiveLevel.Objects.Remove(tileItem);
                        pickedUpItems.Add(tileItem);
                    }
                }
            }
        }
    }

    public static void ResetPlayerPosition()
    {
        Player player = Engine.ActiveProject.ActiveLevel.Objects.OfType<Player>().FirstOrDefault();
        Chaser chaser = Engine.ActiveProject.ActiveLevel.Objects.OfType<Chaser>().FirstOrDefault();

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

    public static void ResetPlayerInventory(List<Item> pickedUpItems)
    {
        Player player = Engine.ActiveProject.ActiveLevel.Objects.OfType<Player>().FirstOrDefault();
        if (player is not null) player.Inventory.Clear();

        foreach (Item item in pickedUpItems)
        {
            Engine.ActiveProject.ActiveLevel.Objects.Add(item);
        }
    }

    public static void DisplaySpace()
    {
        Console.SetCursorPosition(0, 28);
        Console.Write("GameSpace");

        Console.SetCursorPosition(0, 29);
        Helpers.OutputYellow($"Active: ");
        Helpers.OutputGreen(Engine.ActiveProject.ActiveLevel.Name);

        Console.SetCursorPosition(0, 28);
        Console.Write(new string('_', 120));

        Console.SetCursorPosition(0, 0);

        int windowWidth = Console.WindowWidth;
        int windowHeight = Console.WindowHeight;

        foreach (var obj in Engine.ActiveProject.ActiveLevel.Objects)
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

    public static void AddMultipleObjects(string objectType)
    {
        GameObject tempObject = null;

        while (true)
        {
            Console.Clear();
            DisplaySpace();

            Console.ForegroundColor = GetObjectColor(objectType);

            foreach (var obj in Engine.ActiveProject.ActiveLevel.Objects)
            {
                if (obj is GameObject gameObject && gameObject.Type == objectType)
                {
                    Console.SetCursorPosition(gameObject.X, gameObject.Y);
                    
                    Console.Write(GetObjectSymbol(objectType));
                }

            }

            if (tempObject is not null)
            {
                if (tempObject.X >= 0 && tempObject.X < Console.WindowWidth && tempObject.Y >= 0 && tempObject.Y < Console.WindowHeight)
                {
                    Console.SetCursorPosition(tempObject.X, tempObject.Y);
                    Console.Write(GetObjectSymbol(objectType));
                }
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
                    if (tempObject is not null) Engine.ActiveProject.ActiveLevel.Objects.Add(tempObject);
                    break;

                case ConsoleKey.Escape:
                    Console.Clear();
                    Console.Write("e"); // this line stops the program from breaking, do not ask just keep it.
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

    public static void ToggleInventory(Player player, ref bool inInventory)
    {
        inInventory = !inInventory;

        Console.Clear();

        if (inInventory) DisplayInventory(player);
        else DisplaySpace();
    }

    public static void DisplayInventory(Player player)
    {
        Console.WriteLine("Inventory:");

        foreach (var item in player.Inventory) Console.WriteLine($"- {item.Name}");

        Console.WriteLine("\nPress 'i' again to close inventory.");
        Console.SetCursorPosition(player.X, player.Y);
    }

}
