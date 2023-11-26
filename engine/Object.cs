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
        public Player(int x, int y, string name) : base(x, y, name) { }
    }

    internal class Block : GameObject
    {
        public Block(int x, int y, string name) : base(x, y, name) { }
    }

    internal class Chaser : GameObject
    {
        private DateTime lastMoveTime;

        public Chaser(int x, int y, string name) : base(x, y, name)
        {
            lastMoveTime = DateTime.Now;
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

        private bool IsValidMove(int x, int y, List<GameObject> objects)
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

}
