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
        public Player(int x, int y, string name) : base(x, y, name)
        {
        }

        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }

    internal class Block : GameObject
    {
        public Block(int x, int y, string name) : base(x, y, name)
        {
        }

        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }

}
