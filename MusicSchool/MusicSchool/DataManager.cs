using System.Text.Json;
namespace MusicSchool;
public class DataManager{
    public List<Lesson> Lessons {get;init;} = new List<Lesson>();
    public List<Payment> Payments {get;init;} = new List<Payment>();
    public List<Room> Rooms {get;init;} = new List<Room>();
    public List<Student> Students {get;init;} = new List<Student>();
    public List<Subject> Subjects {get;init;} = new List<Subject>();
    public List<Teacher> Teachers {get;init;} = new List<Teacher>();
    public DataManager(){

    }
    public void LoadData(){
        if (!File.Exists("payments.txt")) return;
        string jsPayments = File.ReadAllText("payments.txt");
        List<Payment> loadedPayments  = JsonSerializer.Deserialize<List<Payment>>(jsPayments);
        Payments.AddRange(loadedPayments);
        if (!File.Exists("rooms.txt")) return;
        string jsRooms = File.ReadAllText("rooms.txt");
        List<Room> loadedRooms  = JsonSerializer.Deserialize<List<Room>>(jsRooms);
        Rooms.AddRange(loadedRooms);
        if (!File.Exists("students.txt")) return;
        string jsStudents = File.ReadAllText("students.txt");
        List<Student> loadedStudents  = JsonSerializer.Deserialize<List<Student>>(jsStudents);
        Students.AddRange(loadedStudents);
        if (!File.Exists("subjects.txt")) return;
        string jsSubjects = File.ReadAllText("subjects.txt");
        List<Subject> loadedSubjects  = JsonSerializer.Deserialize<List<Subject>>(jsSubjects);
        Subjects.AddRange(loadedSubjects);
        if (!File.Exists("teachers.txt")) return;
        string jsTeachers = File.ReadAllText("teachers.txt");
        List<Teacher> loadedTeachers  = JsonSerializer.Deserialize<List<Teacher>>(jsTeachers);
        Teachers.AddRange(loadedTeachers);
        if (!File.Exists("lessons.txt")) return;
        string jsLessons = File.ReadAllText("lessons.txt");
        List<Lesson> loadedLessons  = JsonSerializer.Deserialize<List<Lesson>>(jsLessons);
        Lessons.AddRange(loadedLessons);

    }
    public void SaveData(){
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
    }
    public List<AvailableTimeSlot> GetAvailableTimeSlots(DateOnly date, Subject subject){
        List<AvailableTimeSlot> availableTimeSlots = new List<AvailableTimeSlot>();
        var timeSlots = (TimeSlot[])Enum.GetValues(typeof(TimeSlot));
        foreach(var timeSlot in timeSlots){
            List<Room> availableRooms = GetAvailableRooms(date, timeSlot);
            if (availableRooms.Count == 0) continue;
            List<Teacher> availableTeachers = GetAvailableTeachers(date, timeSlot, subject.Id);
            if (availableTeachers.Count == 0) continue;
            AvailableTimeSlot availableTimeSlot = new AvailableTimeSlot(timeSlot, availableTeachers[0], availableRooms[0]);
            availableTimeSlots.Add(availableTimeSlot);
        }
        return availableTimeSlots;
        

    }
    public List<int> GetReservedRoomIds(DateOnly date, TimeSlot timeSlot){
        List<int> reservedRoomIds = new List<int>();
        foreach(var lesson in Lessons){
            if (lesson.Date == date && lesson.TimeSlot == timeSlot){
                reservedRoomIds.Append(lesson.RoomId);
            }
        }
        return reservedRoomIds;
    }
    public List<int> GetReservedTeacherIds(DateOnly date, TimeSlot timeSlot){
        List<int> reservedTeacherIds = new List<int>();
        foreach(var lesson in Lessons){
            if (lesson.Date == date && lesson.TimeSlot == timeSlot){
                reservedTeacherIds.Append(lesson.TeacherId);
            }
        }
        return reservedTeacherIds;
    }
    public bool IsStudentAssignmentToLesson(DateOnly date, TimeSlot timeSlot, int studentId){
        bool isStudentAssignmentToLesson = false;
        foreach(var lesson in Lessons){
            if(lesson.StudentId == studentId &&
            lesson.Date == date &&
            lesson.TimeSlot == timeSlot){
               isStudentAssignmentToLesson = true; 
            }
        }
        return isStudentAssignmentToLesson;
    }

    public List<Room> GetAvailableRooms(DateOnly date, TimeSlot timeSlot){
        List<Room> availableRooms = new List<Room>();
        List<int> reservedRoomIds = GetReservedRoomIds(date, timeSlot);
        foreach (var room in Rooms){
            foreach (var roomId in reservedRoomIds){
                if (room.Id != roomId){
                    continue;
                }
            }
            availableRooms.Add(room);
        }
        return availableRooms;
    }
    public List<Teacher> GetAvailableTeachers(DateOnly date, TimeSlot timeSlot, int subjectId){
        List<Teacher> availableTeachers = new List<Teacher>();
        List<int> reservedTeacherIds = GetReservedTeacherIds(date, timeSlot);
        foreach (var teacher in Teachers){
            foreach (var teacherId in reservedTeacherIds){
                if (teacher.Id != teacherId){
                    continue;
                }
            }
            if (teacher.SubjectId == subjectId){
                availableTeachers.Add(teacher);
            }
        }
        return availableTeachers;
    }
    public Student AddNewStudent(string name){
        int maxId = 0;
        foreach(var student in Students){
            if(student.Id > maxId){
                maxId = student.Id;
            }
        }
        int newId = maxId + 1;
        Student newStudent = new Student(newId, name, false, EducationLevel.beginner ,false);
        Students.Add(newStudent);
        SaveData();
        return newStudent;
    }
    public List<Lesson> GetDailyLessonReport(DateOnly date){
        List<Lesson> dailyScheduleReport = new List<Lesson>();
        foreach(var lesson in Lessons){
            if (lesson.Date == date){
                dailyScheduleReport.Add(lesson);
            }
        }
        return dailyScheduleReport;
    }
    public Teacher GetTeacher(int teacherId){
        foreach(var teacher in Teachers){
            if (teacher.Id == teacherId){
                return teacher;
            }
        }
        return null;
    }
    public Student GetStudent(int studentId){
        foreach(var student in Students){
            if (student.Id == studentId){
                return student;
            }
        }
        return null;
    }
    public Subject GetSubject(int subjectId){
        foreach(var subject in Subjects){
            if (subject.Id == subjectId){
                return subject;
            }
        }
        return null;
    }
    public Room GetRoom(int roomId){
        foreach(var room in Rooms){
            if (room.Id == roomId){
                return room;
            }
        }
        return null;
    }
    public string GetStudentPaymentStatus(int year,int month,int studentId){
        string studentPaymentStatus = "Unpaid";
        foreach(var payment in Payments){
            if(payment.StudentId == studentId &&
            payment.Year == year &&
            payment.Month == month){
               studentPaymentStatus = "Current";
            }
        }
    return studentPaymentStatus;
    }
    public bool IsStudentHasPaid(int month, int year, int studentId){
        bool IsStudentHasPaid = false;
        foreach(var payment in Payments){
            if(payment.Month == month &&
            payment.Year == year &&
            payment.StudentId == studentId){
                IsStudentHasPaid = true;
            }
        }
        return IsStudentHasPaid;
    }
}