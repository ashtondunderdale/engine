using Engine;

namespace engine;

internal class Project
{
    public string Name;
    public string Description;
    public string ID;
    public List<Level> Levels;
    public Level ActiveLevel;

    public Project(string Name, string Description, string ID, List<Level> levels, Level activeLevel) 
    {
        this.Name = Name;
        this.Description = Description;
        this.ID = ID;
        this.Levels = levels;
        this.ActiveLevel = activeLevel;
    }
}
