namespace MusicSchool.Tests;

public class LessonTest
{
    [Fact]
    public void TestLessontMain()
    {
        DateOnly date = new DateOnly(2026,07,10);
        Lesson lesson = new Lesson(date, TimeSlot.s9, 2, 3, 4, 1);
        Assert.Equal(date,lesson.Date);
        Assert.Equal(TimeSlot.s9, lesson.TimeSlot); 
        Assert.Equal(2, lesson.SubjectId); 
        Assert.Equal(3, lesson.RoomId); 
        Assert.Equal(4, lesson.TeacherId); 
        Assert.Equal(1, lesson.StudentId); 

    }
}
