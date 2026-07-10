namespace MusicSchool.Tests;
public class EducationalLevelTest{
    [Fact]
    public void TestEducationLevelMain(){
        Assert.Equal(0, (int)EducationLevel.beginner);
        Assert.Equal(1, (int)EducationLevel.intermediate);
        Assert.Equal(2, (int)EducationLevel.diploma);
    }
}