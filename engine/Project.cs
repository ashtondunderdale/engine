namespace engine;

internal class Project
{
    public string Name;
    public string Description;
    public List<Object> Objects;
    public string ID;

    public Project(string Name, string Description, List<Object> Objects, string ID) 
    {
        this.Name = Name;
        this.Description = Description;
        this.Objects = Objects;
        this.ID = ID;
    }
}
