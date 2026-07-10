namespace MusicSchool.Tests;
public class StudentTest{
    [Fact]
    public void ContrustorTest(){
        Student student1 = new Student(1, "Dan Balan", true, EducationLevel.beginner, false);
        Assert.NotNull(student1);    
    }
    [Fact]
        public void RegisterNeedForMakeUpLessonTest(){
            Student student1 = new Student(1, "Dan Balan", true, EducationLevel.beginner, false);
            student1.RegisterNeedForMakeUpLesson();
            Assert.True(student1.NeedForMakeUpLesson);
        }
}