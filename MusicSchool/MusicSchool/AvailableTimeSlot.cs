namespace MusicSchool;
public class AvailableTimeSlot{
    public TimeSlot TimeSlot {get;init;}
    public Teacher Teacher {get;init;}
    public Room Room {get;init;}
    public AvailableTimeSlot(TimeSlot timeSlot, Teacher teacher, Room room){
        TimeSlot = timeSlot;
        Teacher = teacher;
        Room = room;
    }

}