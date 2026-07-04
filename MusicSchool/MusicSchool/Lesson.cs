namespace MusicSchool;
public class Lesson{
    public DateOnly Date {get;init;}
    public TimeSlot TimeSlot {get;init;}
    public int SubjectId {get;init;}
    public int RoomId {get;init;}
    public int StudentId {get;init;}
    public int TeacherId {get;init;}
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