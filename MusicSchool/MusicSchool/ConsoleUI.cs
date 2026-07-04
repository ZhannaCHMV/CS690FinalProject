namespace MusicSchool;
using Spectre.Console;
public class ConsoleUI{
    private readonly DataManager _dataManager;
    public ConsoleUI(DataManager dataManager){
        _dataManager = dataManager;
    }
    public void Show(){
        while (true){
            var mainMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose action")
                .AddChoices(new[]{
                    "Scheduling a lesson",
                    "Daily schedule report",
                    "Student's payment",
                    "Report on student status",
                    "Exit"
                })
            );
            if (mainMenu == "Scheduling a lesson"){
                ScheduleLesson();
            }else if (mainMenu == "Daily schedule report"){
                GetDailyScheduleReport();
            }else if (mainMenu == "Student's payment"){
                RegisterStudentPayment();
            }else if (mainMenu == "Report on student status"){
                GetReportOnStudentStatus();
            }else if (mainMenu == "Exit"){
                return;
            }

        }
    }
    public void ScheduleLesson(){
        AnsiConsole.MarkupLine ("Scheduling a lesson");
        Subject selectedSubject = ChooseSubject();
        AnsiConsole.MarkupLine ($"Selected subject: {selectedSubject.Name}");
        DateOnly selectedDate = EnterDate();
        AnsiConsole.MarkupLine ($"Selected date: {selectedDate}");
        List<AvailableTimeSlot> availableTimeSlots = _dataManager.GetAvailableTimeSlots(selectedDate,selectedSubject);
        if(availableTimeSlots.Count == 0){
            AnsiConsole.MarkupLine ("There are not available time slots on selected date");
            return;
        }
        AvailableTimeSlot selectedAvailableTimeSlot = ChooseAvailableTimeSlot(availableTimeSlots);
        AnsiConsole.MarkupLine ($"Selected time slot: {selectedAvailableTimeSlot.TimeSlot.ToStr()}");
        Student selectedStudent = ChooseStudent();
        if(selectedStudent == null){
            return;
        }
        AnsiConsole.MarkupLine ($"Selected student: {selectedStudent.Name}");
        bool isStudentAssignmentToLesson = _dataManager.IsStudentAssignmentToLesson(selectedDate,selectedAvailableTimeSlot.TimeSlot,selectedStudent.Id);
        if (isStudentAssignmentToLesson){
            AnsiConsole.MarkupLine($"Selected student: {selectedStudent.Name} already has lesson at {selectedAvailableTimeSlot.TimeSlot.ToStr()} on {selectedDate}");
            return;
        }
        
        string selectedActionSaveOrExit = ChooseActionSaveOrExit();
        if(selectedActionSaveOrExit == "Save"){
            Lesson lesson = new Lesson(selectedDate, 
            selectedAvailableTimeSlot.TimeSlot, 
            selectedSubject.Id, selectedAvailableTimeSlot.Room.Id, 
            selectedStudent.Id, selectedAvailableTimeSlot.Teacher.Id);

            _dataManager.Lessons.Add(lesson);
            _dataManager.SaveData();
        }
    }

    public Subject ChooseSubject(){
        var selectedSubject = AnsiConsole.Prompt(
                new SelectionPrompt<Subject>()
                .Title("Choose subject")
                .UseConverter(subject => subject.Name)
                .AddChoices(_dataManager.Subjects)); 
        return selectedSubject;   
    }
    public DateOnly EnterDate(){
        var datePrompt= new TextPrompt<DateOnly>("Enter date (yyyy-mm-dd)")
            .ValidationErrorMessage("Wrong date format");
        DateOnly selectedDate = AnsiConsole.Prompt(datePrompt);
        return selectedDate;
    }
    public AvailableTimeSlot ChooseAvailableTimeSlot(List<AvailableTimeSlot> availableTimeSlots){
        var selectedAvailableTimeSlot = AnsiConsole.Prompt(
                new SelectionPrompt<AvailableTimeSlot>()
                .Title("Choose time slot")
                .UseConverter(ts => ts.TimeSlot.ToStr())
                .AddChoices(availableTimeSlots));     
        return selectedAvailableTimeSlot;
    }
    public string ChooseActionSaveOrExit(){
        var selectedActionSaveOrExit = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose action")
                .AddChoices(new[]{
                    "Save",
                    "Exit without saving"
                })
            );
        return selectedActionSaveOrExit;
    }
    public Student? ChooseStudent(){
        var menuStudentList = _dataManager.Students.Select(s=> new StudentMenuItem(s)).ToList();
        menuStudentList.Add(new StudentMenuItem("Add new student"));
        var selectedMenuStudent = AnsiConsole.Prompt(
            new SelectionPrompt<StudentMenuItem>()
            .Title("Choose a student or add new student")
            .UseConverter(s => s.Text)
            .AddChoices(menuStudentList));
        if (selectedMenuStudent.Text == "Add new student"){
            string newStudentName = AnsiConsole.Prompt(new TextPrompt<string>("Enter student name"));
            string selectedActionSaveOrExit = ChooseActionSaveOrExit();
            if(selectedActionSaveOrExit == "Save"){
                Student newStudent = _dataManager.AddNewStudent(newStudentName);
                return newStudent;
            }else{
                return null;
            }
        } 
        return selectedMenuStudent.Student; 
    }
    public void GetDailyScheduleReport(){
        AnsiConsole.MarkupLine ("Daily schedule report"); 
        DateOnly selectedDate = EnterDate();
        AnsiConsole.MarkupLine ($"Selected date: {selectedDate}");
        List<Lesson> dailyLessonReport = _dataManager.GetDailyLessonReport(selectedDate);
        if(dailyLessonReport.Count == 0){
            AnsiConsole.MarkupLine ($"There are not scheduled lessons on {selectedDate}");      
        }
        var tableReport = new Table();
        tableReport.Border(TableBorder.Rounded);
        tableReport.Title($"Daily schedule report on {selectedDate}");
        tableReport.AddColumn("Time");
        tableReport.AddColumn("Subject");
        tableReport.AddColumn("Teacher");
        tableReport.AddColumn("Room");
        tableReport.AddColumn("Student");
        foreach(var lesson in dailyLessonReport){
            var subjectText = _dataManager.GetSubject(lesson.SubjectId).Name;
            var teacherText = _dataManager.GetTeacher(lesson.TeacherId).Name;
            var roomText = _dataManager.GetRoom(lesson.RoomId).Name;
            var studentText = _dataManager.GetStudent(lesson.StudentId).Name;
            tableReport.AddRow(
                lesson.TimeSlot.ToStr(),
                subjectText, teacherText, roomText, studentText
            );
        }
        AnsiConsole.Write(tableReport);
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("For return to main menu choose 'Back to main menu'").
            AddChoices(new[] { "Back to main menu"})
        );
    }
    public void RegisterStudentPayment(){
        AnsiConsole.MarkupLine ("Students payment"); 
        Student selectedStudent = ChooseStudent();
        if(selectedStudent == null){
            return;
        } 
        AnsiConsole.MarkupLine ($"Selected student: {selectedStudent.Name}");
        var selectedYearPrompt  = new TextPrompt<int>("Enter year (2026-2036):")
            .ValidationErrorMessage("[red]Please enter a valid number![/]")
            .Validate(year => {
                if (year < 2026 || year > 2036){
                    return ValidationResult.Error("[red]Year must be between 2026 and 2036![/]");
                }
            return ValidationResult.Success();
        });
        int selectedYear = AnsiConsole.Prompt(selectedYearPrompt);
        AnsiConsole.MarkupLine ($"Selected year: {selectedYear}");
        var selectedMonthPrompt = new TextPrompt<int>("Enter month (1-12):")
            .ValidationErrorMessage("[red]Please enter a valid number![/]")
            .Validate(month => {
                if (month < 1 || month > 12){
                    return ValidationResult.Error("[red]Month must be between 1 and 12![/]");
                }
                return ValidationResult.Success();
                });
        int selectedMonth = AnsiConsole.Prompt(selectedMonthPrompt);
        AnsiConsole.MarkupLine ($"Selected month: {selectedMonth}");
        bool isStudentHasPaid = _dataManager.IsStudentHasPaid(selectedMonth,selectedYear,selectedStudent.Id);
        if (isStudentHasPaid){
            AnsiConsole.MarkupLine($"Selected student: {selectedStudent.Name} already has paid for {selectedMonth} month in {selectedYear}");
            return;
        }
        string selectedActionSaveOrExit = ChooseActionSaveOrExit();
        if(selectedActionSaveOrExit == "Save"){
            Payment payment = new Payment(selectedMonth, selectedYear, selectedStudent.Id);
            _dataManager.Payments.Add(payment);
            _dataManager.SaveData();
        }
    }
    public void GetReportOnStudentStatus(){
        AnsiConsole.MarkupLine ("Report on student status"); 
        Student selectedStudent = ChooseStudent();
        if(selectedStudent == null){
            return;
        }
        AnsiConsole.MarkupLine ("Selected student: {selectedStudent.name}");
        var selectedYearPrompt  = new TextPrompt<int>("Enter year (2026-2036):")
            .ValidationErrorMessage("[red]Please enter a valid number![/]")
            .Validate(year => {
                if (year < 2026 || year > 2036){
                    return ValidationResult.Error("[red]Year must be between 2026 and 2036![/]");
                }
            return ValidationResult.Success();
        });
        int selectedYear = AnsiConsole.Prompt(selectedYearPrompt);
        AnsiConsole.MarkupLine ($"Selected year: {selectedYear}");
        var selectedMonthPrompt = new TextPrompt<int>("Enter month (1-12):")
            .ValidationErrorMessage("[red]Please enter a valid number![/]")
            .Validate(month => {
                if (month < 1 || month > 12){
                    return ValidationResult.Error("[red]Month must be between 1 and 12![/]");
                }
                return ValidationResult.Success();
                });
        int selectedMonth = AnsiConsole.Prompt(selectedMonthPrompt);
        AnsiConsole.MarkupLine ($"Selected month: {selectedMonth}");
        string studentPaymentStatus = _dataManager.GetStudentPaymentStatus(selectedYear,selectedMonth,selectedStudent.Id);
        AnsiConsole.MarkupLine ($"Student's status: {studentPaymentStatus}");
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("For return to main menu choose 'Back to main menu'").
            AddChoices(new[] { "Back to main menu"}));
    }

 
}