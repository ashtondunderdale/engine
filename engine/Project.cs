using Engine;

namespace engine;

internal class Project
{
    public string Name;
    public string Description;
    public List<GameObject> Objects;
    public string ID;
    public bool ContainsPlayerObject;

    public Project(string Name, string Description, List<GameObject> Objects, string ID, bool ContainsPlayerObject) 
    {
        this.Name = Name;
        this.Description = Description;
        this.Objects = Objects;
        this.ID = ID;
        this.ContainsPlayerObject = ContainsPlayerObject;
    }
}
