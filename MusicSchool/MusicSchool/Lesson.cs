namespace MusicSchool;
public class Lesson{
    public DateOnly Date {get;set;}
    public TimeSlot TimeSlot {get;set;}
    public int SubjectId {get;init;}
    public int RoomId {get;set;}
    public int StudentId {get;init;}
    public int TeacherId {get;set;}
    public Lesson(DateOnly date, 
                TimeSlot timeSlot, 
                int subjectId, 
                int roomId, 
                int teacherId, 
                int studentId){
        Date = date;
        TimeSlot = timeSlot;
        SubjectId = subjectId;
        RoomId = roomId;
        TeacherId = teacherId;
        StudentId = studentId;
    }
}