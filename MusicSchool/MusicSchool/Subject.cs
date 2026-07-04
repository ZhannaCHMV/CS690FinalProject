namespace MusicSchool;
public class Subject{
    public int Id {get;init;}
    public string Name {get;init;}
    public Subject(int id, string name){
        Id = id;
        Name = name;
    }
}