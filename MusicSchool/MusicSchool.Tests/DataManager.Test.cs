namespace MusicSchool.Tests;
using System.Text.Json;
public class DataManagerTest
{
    [Fact]
    public void LoadDataTest(){
        DateOnly date = new DateOnly(2026,7,10);
        Lesson lesson1 = new Lesson(date, TimeSlot.s9, 2, 3, 4, 1);
        List<Lesson> Lessons  = new List<Lesson>();
        Lessons.Add(lesson1);
        Student student1 = new Student(1, "Dan Balan", true, EducationLevel.beginner, true);
        List<Student> Students = new List<Student>();
        Students.Add(student1);
        Room room1 = new Room(1, "Room1");
        List<Room> Rooms = new List<Room>();
        Rooms.Add(room1);
        Subject subject1 = new Subject(1, "piano");
        List<Subject> Subjects = new List<Subject>();
        Subjects.Add(subject1);
        Payment payment1 = new Payment(12, 2026, 1);
        List<Payment> Payments = new List<Payment>();
        Payments.Add(payment1);
        Teacher teacher1 = new Teacher(1,"Larisa",1);
        List<Teacher> Teachers = new List<Teacher>();
        Teachers.Add(teacher1); 
        try{
            var options = new JsonSerializerOptions{WriteIndented = true};
            string jsPayments = JsonSerializer.Serialize(Payments, options);
            File.WriteAllText("payments.txt", jsPayments);
            string jsRooms = JsonSerializer.Serialize(Rooms, options);
            File.WriteAllText("rooms.txt", jsRooms);
            string jsStudents = JsonSerializer.Serialize(Students, options);
            File.WriteAllText("students.txt", jsStudents);
            string jsSubjects = JsonSerializer.Serialize(Subjects, options);
            File.WriteAllText("subjects.txt", jsSubjects);
            string jsTeachers = JsonSerializer.Serialize(Teachers, options);
            File.WriteAllText("teachers.txt", jsTeachers);
            string jsLessons = JsonSerializer.Serialize(Lessons, options);
            File.WriteAllText("lessons.txt", jsLessons);
            DataManager dataManager = new DataManager();
            dataManager.LoadData();
            Assert.Equal(student1.Id,dataManager.Students[0].Id);
            Assert.Equal(student1.Name,dataManager.Students[0].Name);
            Assert.True(dataManager.Students[0].CompletionCurrentLevel);
            Assert.Equal(student1.EducationLevel,dataManager.Students[0].EducationLevel);
            Assert.True(dataManager.Students[0].NeedForMakeUpLesson);
            Assert.Equal(subject1.Id,dataManager.Subjects[0].Id);
            Assert.Equal(subject1.Name,dataManager.Subjects[0].Name);
            Assert.Equal(room1.Id,dataManager.Rooms[0].Id);
            Assert.Equal(room1.Name,dataManager.Rooms[0].Name);
            Assert.Equal(payment1.Year, dataManager.Payments[0].Year);
            Assert.Equal(payment1.Month, dataManager.Payments[0].Month);
            Assert.Equal(payment1.StudentId, dataManager.Payments[0].StudentId);
            Assert.Equal(teacher1.Id,dataManager.Teachers[0].Id);
            Assert.Equal(teacher1.Name,dataManager.Teachers[0].Name);
            Assert.Equal(teacher1.SubjectId,dataManager.Teachers[0].SubjectId);
            Assert.Equal(lesson1.Date,dataManager.Lessons[0].Date);
            Assert.Equal(lesson1.TimeSlot,dataManager.Lessons[0].TimeSlot);
            Assert.Equal(lesson1.SubjectId,dataManager.Lessons[0].SubjectId);
            Assert.Equal(lesson1.RoomId,dataManager.Lessons[0].RoomId);
            Assert.Equal(lesson1.StudentId,dataManager.Lessons[0].StudentId);
            Assert.Equal(lesson1.TeacherId,dataManager.Lessons[0].TeacherId);
        }
        finally {
            if (File.Exists("payments.txt")) {
                File.Delete("payments.txt");
            }
            if (File.Exists("rooms.txt")) {
                File.Delete("rooms.txt");
            }
            if (File.Exists("students.txt")) {
                File.Delete("students.txt");
            }
            if (File.Exists("subjects.txt")) {
                File.Delete("subjects.txt");
            }
            if (File.Exists("teachers.txt")) {
                File.Delete("teachers.txt");
            }
            if (File.Exists("lessons.txt")) {
                File.Delete("lessons.txt");
            }
        }
    }
    
    [Fact]
    public void SaveDataTest(){
        DataManager dataManager = new DataManager();
        DateOnly date = new DateOnly(2026,7,10);
        Lesson lesson1 = new Lesson(date, TimeSlot.s9, 2, 3, 4, 1);
        dataManager.Lessons.Add(lesson1);
        Student student1 = new Student(1, "Dan Balan", true, EducationLevel.beginner, true);
        dataManager.Students.Add(student1);
        Room room1 = new Room(1, "Room1");
        dataManager.Rooms.Add(room1);
        Subject subject1 = new Subject(1, "piano");
        dataManager.Subjects.Add(subject1);
        Payment payment1 = new Payment(12, 2026, 1);
        dataManager.Payments.Add(payment1);
        Teacher teacher1 = new Teacher(1,"Larisa",1);
        dataManager.Teachers.Add(teacher1);
        try{
            dataManager.SaveData();
            string jsPayments = File.ReadAllText("payments.txt");
            List<Payment> loadedPayments  = JsonSerializer.Deserialize<List<Payment>>(jsPayments);
            string jsRooms = File.ReadAllText("rooms.txt");
            List<Room> loadedRooms  = JsonSerializer.Deserialize<List<Room>>(jsRooms);
            string jsStudents = File.ReadAllText("students.txt");
            List<Student> loadedStudents  = JsonSerializer.Deserialize<List<Student>>(jsStudents);
            string jsSubjects = File.ReadAllText("subjects.txt");
            List<Subject> loadedSubjects  = JsonSerializer.Deserialize<List<Subject>>(jsSubjects);
            string jsTeachers = File.ReadAllText("teachers.txt");
            List<Teacher> loadedTeachers  = JsonSerializer.Deserialize<List<Teacher>>(jsTeachers);
            string jsLessons = File.ReadAllText("lessons.txt");
            List<Lesson> loadedLessons  = JsonSerializer.Deserialize<List<Lesson>>(jsLessons);
            Assert.NotNull(loadedPayments);
            Assert.NotNull(loadedRooms);
            Assert.NotNull(loadedStudents);
            Assert.NotNull(loadedSubjects);
            Assert.NotNull(loadedTeachers);
            Assert.NotNull(loadedLessons);
            Assert.Equal(student1.Id,loadedStudents[0].Id);
            Assert.Equal(student1.Name,loadedStudents[0].Name);
            Assert.True(loadedStudents[0].CompletionCurrentLevel);
            Assert.Equal(student1.EducationLevel,loadedStudents[0].EducationLevel);
            Assert.True(loadedStudents[0].NeedForMakeUpLesson);
            Assert.Equal(subject1.Id,loadedSubjects[0].Id);
            Assert.Equal(subject1.Name,loadedSubjects[0].Name);
            Assert.Equal(room1.Id,loadedRooms[0].Id);
            Assert.Equal(room1.Name,loadedRooms[0].Name);
            Assert.Equal(payment1.Year, loadedPayments[0].Year);
            Assert.Equal(payment1.Month, loadedPayments[0].Month);
            Assert.Equal(payment1.StudentId, loadedPayments[0].StudentId);
            Assert.Equal(teacher1.Id,loadedTeachers[0].Id);
            Assert.Equal(teacher1.Name,loadedTeachers[0].Name);
            Assert.Equal(teacher1.SubjectId,loadedTeachers[0].SubjectId);
            Assert.Equal(lesson1.Date,loadedLessons[0].Date);
            Assert.Equal(lesson1.TimeSlot,loadedLessons[0].TimeSlot);
            Assert.Equal(lesson1.SubjectId,loadedLessons[0].SubjectId);
            Assert.Equal(lesson1.RoomId,loadedLessons[0].RoomId);
            Assert.Equal(lesson1.StudentId,loadedLessons[0].StudentId);
            Assert.Equal(lesson1.TeacherId,loadedLessons[0].TeacherId);

        }
        finally {
            if (File.Exists("payments.txt")) {
                File.Delete("payments.txt");
            }
            if (File.Exists("rooms.txt")) {
                File.Delete("rooms.txt");
            }
            if (File.Exists("students.txt")) {
                File.Delete("students.txt");
            }
            if (File.Exists("subjects.txt")) {
                File.Delete("subjects.txt");
            }
            if (File.Exists("teachers.txt")) {
                File.Delete("teachers.txt");
            }
            if (File.Exists("lessons.txt")) {
                File.Delete("lessons.txt");
            }
        }
    }
    [Fact]
    public void GetLessonsBySubjectIdTest(){
        DataManager dataManager = new DataManager();
        DateOnly date = new DateOnly(2026,7,10);
        Lesson lesson1 = new Lesson(date, TimeSlot.s9, 2, 3, 4, 1);
        Lesson lesson2 = new Lesson(date, TimeSlot.s10, 5, 3, 4, 1);
        dataManager.Lessons.Add(lesson1);
        dataManager.Lessons.Add(lesson2);
        List<Lesson> lessons1 = dataManager.GetLessonsBySubjectId(date, 2);
        List<Lesson> lessons2 = dataManager.GetLessonsBySubjectId(date, 5);
        Assert.Equal(lesson1, lessons1[0]);
        Assert.Equal(lesson2, lessons2[0]);
    }
    [Fact]
    public void GetNeedForMakeUpLessonReportTest(){
        DataManager dataManager = new DataManager();
        Student student1 = new Student(1, "Dan Balan", true, EducationLevel.beginner, true);
        Student student2 = new Student(2, "Mark Kreed", true, EducationLevel.beginner, false);
        dataManager.Students.Add(student1);
        dataManager.Students.Add(student2);
        List<Student> result = dataManager.GetNeedForMakeUpLessonReport();
        Assert.Equal(student1, result[0]);
    }
    [Fact]
    public void IsStudentHasPaidTest(){
        DataManager dataManager = new DataManager();
        Payment payment1 = new Payment(12, 2026, 1);
        Payment payment2 = new Payment(12, 2026, 2);
        dataManager.Payments.Add(payment1);
        dataManager.Payments.Add(payment2);
        bool result1 = dataManager.IsStudentHasPaid(12, 2026, 1);
        bool result2 = dataManager.IsStudentHasPaid(11, 2026, 1);
        Assert.True(result1);
        Assert.False(result2);
    }
    [Fact]
    public void GetStudentPaymentStatusTest(){
        DataManager dataManager = new DataManager();
        Payment payment1 = new Payment(12, 2026, 1);
        Payment payment2 = new Payment(12, 2026, 2);
        dataManager.Payments.Add(payment1);
        dataManager.Payments.Add(payment2);
        string result1 = dataManager.GetStudentPaymentStatus(2026, 12, 1);
        string result2 = dataManager.GetStudentPaymentStatus(2026, 11, 2);
        Assert.Equal("Current", result1);
        Assert.Equal("Unpaid", result2);
    }
    [Fact]
    public void GetRoomTest(){
        DataManager dataManager = new DataManager();
        Room room1 = new Room(1, "Room1");
        Room room2 = new Room(2, "Room2");
        dataManager.Rooms.Add(room1);
        dataManager.Rooms.Add(room2);
        Room result1 = dataManager.GetRoom(1);
        Room result2 = dataManager.GetRoom(2);
        Assert.Equal(room1, result1);
        Assert.Equal(room2, result2);
    }
    [Fact]
    public void GetSubjectTest(){
        DataManager dataManager = new DataManager();
        Subject subject1 = new Subject(1, "piano");
        dataManager.Subjects.Add(subject1);
        Subject result = dataManager.GetSubject(1);
        Assert.Equal(subject1,result);
    }
    [Fact]
    public void GetStudentTest(){
        DataManager dataManager = new DataManager();
        Student student1 = new Student(1, "Dan Balan", true, EducationLevel.beginner, true);
        dataManager.Students.Add(student1);
        Student result = dataManager.GetStudent(1);
        Assert.Equal(student1,result);
    }
    [Fact]
    public void GetTeacherTest(){
        DataManager dataManager = new DataManager();
        Teacher teacher1 = new Teacher(1,"Larisa",1);
        dataManager.Teachers.Add(teacher1);
        Teacher result = dataManager.GetTeacher(1);
        Assert.Equal(teacher1,result);
    }
    [Fact]
    public void GetDailyLessonReport(){
        DataManager dataManager = new DataManager();
        DateOnly date1 = new DateOnly(2026,7,10);
        DateOnly date2 = new DateOnly(2026,7,11);
        DateOnly date3 = new DateOnly(2026,7,12);
        Lesson lesson1 = new Lesson(date1, TimeSlot.s9, 2, 3, 4, 1);
        Lesson lesson2 = new Lesson(date2, TimeSlot.s10, 5, 3, 4, 1);
        dataManager.Lessons.Add(lesson1);
        dataManager.Lessons.Add(lesson2);
        List<Lesson> result1 = dataManager.GetDailyLessonReport(date1);
        List<Lesson> result2 = dataManager.GetDailyLessonReport(date3);
        Assert.Equal(lesson1,result1[0]);
        Assert.Equal(0,result2.Count);
    }
    [Fact]
    public void AddNewStudentTest(){
        DataManager dataManager = new DataManager();
        dataManager.AddNewStudent("Alex");
        Assert.Equal("Alex",dataManager.Students[0].Name);
    }
    [Fact]
    public void GetAvailableTeacherseTest(){
        DataManager dataManager = new DataManager();
        DateOnly date = new DateOnly(2026,7,10);
        Lesson lesson1 = new Lesson(date, TimeSlot.s9, 2, 3, 1, 1);
        Lesson lesson2 = new Lesson(date, TimeSlot.s10, 2, 3, 2, 2);
        dataManager.Lessons.Add(lesson1);
        dataManager.Lessons.Add(lesson2);
        Teacher teacher1 = new Teacher(1,"Larisa",2);
        Teacher teacher2 = new Teacher(2,"Donald",2);
        dataManager.Teachers.Add(teacher1);
        List<Teacher> result = dataManager.GetAvailableTeachers(date,TimeSlot.s10,2);
        Assert.Equal(teacher1, result[0]);
    }
    [Fact]
    public void GetAvailableRoomsTest(){
        DataManager dataManager = new DataManager();
        DateOnly date = new DateOnly(2026,7,10);
        Lesson lesson1 = new Lesson(date, TimeSlot.s9, 2, 1, 1, 1);
        Lesson lesson2 = new Lesson(date, TimeSlot.s10, 2, 2, 2, 2);
        dataManager.Lessons.Add(lesson1);
        dataManager.Lessons.Add(lesson2);
        Room room1 = new Room(1, "Room1");
        Room room2 = new Room(2, "Room2");
        dataManager.Rooms.Add(room1);
        dataManager.Rooms.Add(room2);
        List<Room> result = dataManager.GetAvailableRooms(date,TimeSlot.s9);
        Assert.Equal(room2,result[0]);
    }
    [Fact]
    public void IsStudentAssignmentToLessonTest(){
        DataManager dataManager = new DataManager();
        DateOnly date = new DateOnly(2026,7,10);
        Lesson lesson1 = new Lesson(date, TimeSlot.s9, 2, 1, 1, 1);
        Lesson lesson2 = new Lesson(date, TimeSlot.s10, 2, 2, 2, 2);
        dataManager.Lessons.Add(lesson1);
        dataManager.Lessons.Add(lesson2);
        bool result1 = dataManager.IsStudentAssignmentToLesson(date, TimeSlot.s9,1);
        bool result2 = dataManager.IsStudentAssignmentToLesson(date, TimeSlot.s9,2);
        Assert.True(result1);
        Assert.False(result2);
    }
    [Fact]
    public void GetReservedRoomIdsTest(){
        DataManager dataManager = new DataManager();
        DateOnly date = new DateOnly(2026,7,10);
        Lesson lesson1 = new Lesson(date, TimeSlot.s9, 2, 1, 1, 1);
        Lesson lesson2 = new Lesson(date, TimeSlot.s10, 2, 2, 2, 2);
        dataManager.Lessons.Add(lesson1);
        dataManager.Lessons.Add(lesson2);
        List<int> result1 = dataManager.GetReservedRoomIds(date, TimeSlot.s9);
        List<int> result2 = dataManager.GetReservedRoomIds(date, TimeSlot.s10);
        Assert.Equal(1,result1[0]);
        Assert.Equal(2,result2[0]);
    }
    [Fact]
    public void GetReservedTeacherIdsTest(){
        DataManager dataManager = new DataManager();
        DateOnly date = new DateOnly(2026,7,10);
        Lesson lesson1 = new Lesson(date, TimeSlot.s9, 2, 1, 1, 1);
        Lesson lesson2 = new Lesson(date, TimeSlot.s10, 2, 2, 2, 2);
        dataManager.Lessons.Add(lesson1);
        dataManager.Lessons.Add(lesson2);
        List<int> result1 = dataManager.GetReservedTeacherIds(date, TimeSlot.s9);
        List<int> result2 = dataManager.GetReservedTeacherIds(date, TimeSlot.s10);
        Assert.Equal(1,result1[0]);
        Assert.Equal(2,result2[0]);
    }
    [Fact]
    public void GetAvailableTimeSlots(){
        DataManager dataManager = new DataManager();
        DateOnly date = new DateOnly(2026,7,10);
        Lesson lesson1 = new Lesson(date, TimeSlot.s9, 2, 3, 1, 1);
        Lesson lesson2 = new Lesson(date, TimeSlot.s10, 2, 3, 2, 2);
        dataManager.Lessons.Add(lesson1);
        dataManager.Lessons.Add(lesson2);
        Teacher teacher1 = new Teacher(1,"Larisa",2);
        Teacher teacher2 = new Teacher(2,"Donald",2);
        dataManager.Teachers.Add(teacher1);
        dataManager.Teachers.Add(teacher2);
        Room room1 = new Room(1, "Room1");
        Room room2 = new Room(2, "Room2");
        dataManager.Rooms.Add(room1);
        dataManager.Rooms.Add(room2);
        Subject subject = new Subject(2,"piano");
        List<AvailableTimeSlot> result = dataManager.GetAvailableTimeSlots(date,subject);
        Assert.Equal(9,result.Count);
    }
}