namespace Engine
{
    internal class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public virtual string Name { get; set; }

        public GameObject(int x, int y, string name)
        {
            X = x;
            Y = y;
            Name = name;
        }
    }

    internal class Player : GameObject
    {
        public List<Item> Inventory { get; private set; }
        public int OriginalX { get; private set; }
        public int OriginalY { get; private set; }
        public Player(int x, int y, string name) : base(x, y, name)
        {
            Inventory = new List<Item>();
            OriginalX = x;
            OriginalY = y;
        }
    }

    internal class Block : GameObject
    {
        public Block(int x, int y, string name) : base(x, y, name) { }
    }

    internal class Chaser : GameObject
    {
        private DateTime lastMoveTime;
        public int OriginalX { get; private set; }
        public int OriginalY { get; private set; }

        public Chaser(int x, int y, string name) : base(x, y, name)
        {
            lastMoveTime = DateTime.Now;
            OriginalX = x;
            OriginalY = y;
        }

        public void ChasePlayer(Player player, List<GameObject> objects)
        {
            TimeSpan elapsed = DateTime.Now - lastMoveTime;

            if (elapsed.TotalMilliseconds >= 1000)
            {
                int deltaX = Math.Sign(player.X - X);
                int deltaY = Math.Sign(player.Y - Y);

                int newX = X + deltaX;
                int newY = Y + deltaY;

                if (IsValidMove(newX, newY, objects))
                {
                    X = newX;
                    Y = newY;
                }

                lastMoveTime = DateTime.Now;
            }
        }

        private static bool IsValidMove(int x, int y, List<GameObject> objects)
        {
            if (x < 0 || x >= Console.WindowWidth || y < 0 || y >= Console.WindowHeight)
            {
                return false;
            }

            foreach (var obj in objects)
            {
                if (obj.X == x && obj.Y == y && !(obj is Chaser))
                {
                    return false; 
                }
            }

            return true;
        }
    }

    internal class Item : GameObject
    {
        public int OriginalX { get; private set; }
        public int OriginalY { get; private set; }

        public Item(int x, int y, string name) : base(x, y, name)
        {
            OriginalX = x;
            OriginalY = y;
        }
    }
}
