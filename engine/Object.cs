namespace engine
{
    internal class Object
    {
        public int X;
        public int Y;
    }

    internal class Player : Object
    {
        public int PlayerX => X;
        public int PlayerY => Y;
    }
}