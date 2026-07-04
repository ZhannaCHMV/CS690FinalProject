namespace MusicSchool;
public class Room{
    public int Id {get;init;}
    public string Name {get;init;}
    public Room(int id, string name){
        Id = id;
        Name = name;
    }
}