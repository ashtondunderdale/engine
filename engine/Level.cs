using Engine;

namespace engine;

internal class Level
{

    public string Name { get; set; }
    public List<GameObject> Objects;
    public bool ContainsPlayerObject;

    public Level(string name, List<GameObject> objects, bool containsPlayerObject) 
    { 
        Name = name;
        Objects = objects;
        ContainsPlayerObject = containsPlayerObject;
    }

    public static void GenerateLevel() 
    { 
    
    }
}
