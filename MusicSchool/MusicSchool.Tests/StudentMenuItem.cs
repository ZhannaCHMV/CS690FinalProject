namespace MusicSchool.Tests;

public class StudentMenuItemTest
{
    [Fact]
    public void StudentMenuItemStudentTest(){
        Student student = new Student(1, "Dan Balan", true, EducationLevel.beginner, true);
        StudentMenuItem studentMenuItem = new StudentMenuItem(student);
        Assert.Equal(student,studentMenuItem.Student);
        Assert.False(studentMenuItem.IsItemMenu);
        Assert.Equal(student.Name, studentMenuItem.Student.Name);
        
    }
    [Fact]
    public void StudentMenuItemTextTest(){
        StudentMenuItem studentMenuItem = new StudentMenuItem("ok");
        Assert.True(studentMenuItem.IsItemMenu);
        Assert.Equal("ok",studentMenuItem.Text);
    }
}
