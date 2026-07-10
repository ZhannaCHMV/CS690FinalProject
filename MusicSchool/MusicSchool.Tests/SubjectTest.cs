namespace MusicSchool.Tests;

public class SubjectTest
{
    [Fact]
    public void TestSubjectMain()
    {
        Subject subject = new Subject(1,"piano");
        Assert.Equal(1,subject.Id);
        Assert.Equal("piano", subject.Name); 
    }
}
