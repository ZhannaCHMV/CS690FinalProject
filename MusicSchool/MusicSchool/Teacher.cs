namespace MusicSchool;
public class Teacher{
    public int Id {get;init;}
    public string Name {get;init;}
    public int SubjectId {get;init;}
    public Teacher(int id, string name,int subjectId ){
        Id = id;
        Name = name;
        SubjectId = subjectId;
    }
}