namespace MusicSchool.Tests;

public class AvailvableTimeSlotTest
{
    [Fact]
    public void TestAvailvableTimeSlotMain()
    {
        Teacher teacher = new Teacher(1,"Larisa",1);
        Room room = new Room(1, "Room 1");
        AvailableTimeSlot availableTimeSlot = new AvailableTimeSlot(TimeSlot.s9,teacher,room);
        Assert.Equal(TimeSlot.s9, availableTimeSlot.TimeSlot);
        Assert.Equal(teacher,availableTimeSlot.Teacher); 
        Assert.Equal(room,availableTimeSlot.Room);
    }
}
