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

        public void ChasePlayer(Player player)
        {
            TimeSpan elapsed = DateTime.Now - lastMoveTime;

            if (elapsed.TotalMilliseconds >= 1000)
            {
                int deltaX = Math.Sign(player.X - X);
                int deltaY = Math.Sign(player.Y - Y);

                if (X + deltaX >= 0 && X + deltaX < Console.WindowWidth && Y + deltaY >= 0 && Y + deltaY < Console.WindowHeight)
                {
                    X += deltaX;
                    Y += deltaY;
                }

                lastMoveTime = DateTime.Now;
            }
        }
    }
}
