namespace MusicSchool.Tests;

public class TeacherTest
{
    [Fact]
    public void TestTeacherMain()
    {
        Teacher teacher = new Teacher(1,"Larisa",2);
        Assert.Equal(1,teacher.Id);
        Assert.Equal("Larisa", teacher.Name);
        Assert.Equal(2, teacher.SubjectId);
        
    }
}
