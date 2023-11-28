using Engine;

namespace engine;

internal class Project
{
    public string Name;
    public string Description;
    public List<Level> Levels;
    public Level ActiveLevel;

    public Project(string Name, string Description, List<Level> levels, Level activeLevel) 
    {
        this.Name = Name;
        this.Description = Description;
        this.Levels = levels;
        this.ActiveLevel = activeLevel;
    }
}
