namespace engine
{
    internal class GameInfo
    {
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
    }
}
