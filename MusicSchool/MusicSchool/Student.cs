namespace MusicSchool;
public class Student{
    public int Id {get;init;}
    public string Name {get;init;}
    public bool CompletionCurrentLevel {get;set;}
    public EducationLevel EducationLevel {get;set;}
    public bool NeedForMakeUpLesson {get;set;}
    public Student(int id, string name, bool completionCurrentLevel, EducationLevel educationLevel, bool needForMakeUpLesson){
        Id = id;
        Name = name;
        CompletionCurrentLevel = completionCurrentLevel;
        EducationLevel = educationLevel;
        NeedForMakeUpLesson = needForMakeUpLesson;
    }   
    public void RegisterNeedForMakeUpLesson(){
    NeedForMakeUpLesson = true;
    }
    public void MarkCurrentLevelAsCompleted(){
        CompletionCurrentLevel = true;
    }
    public void NextEducationLevel(){
        EducationLevel = EducationLevel + 1;
    }
}