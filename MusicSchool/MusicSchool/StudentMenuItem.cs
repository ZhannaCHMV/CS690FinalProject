namespace MusicSchool;
public class StudentMenuItem{
    public Student? Student{get;}
    public bool IsItemMenu{get;}
    public string Text{get;}
    public StudentMenuItem(Student student){
        Student = student;
        IsItemMenu = false;
        Text = student.Name;
    }
    public StudentMenuItem(string text){
        Student = null;
        IsItemMenu = true;
        Text = text;
    }
}