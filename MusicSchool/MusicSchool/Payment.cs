namespace MusicSchool;
public class Payment{
    public int Month {get;init;}
    public int Year {get;init;}
    public int StudentId {get;init;}
    public Payment(int month, int year, int studentId){
        Month = month;
        Year = year;
        StudentId = studentId;
    }
}