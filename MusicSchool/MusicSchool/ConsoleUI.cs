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
                    "Mark current level of study as completed",
                    "Transfer a student to the next level of study",
                    "Show a student's education level and a status of completion current level",
                    "Registering a need for makeup lessons",
                    "Report on students who need makeup lessons",
                    "Rescheduling the lesson",
                    "Canceling and removing a lesson",
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
            }else if (mainMenu == "Mark current level of study as completed"){
                MarkCurrentLevelAsCompleted();   
            }else if (mainMenu == "Show a student's education level and a status of completion current level"){
                ShowStudentsEducationLevelAndStatusOfCompletionCurrentLevel();     
            }else if (mainMenu == "Transfer a student to the next level of study"){
                TransferStudentToNextLevelOfStudy();     
            }else if (mainMenu == "Registering a need for makeup lessons"){
                RegisterNeedForMakeUpLesson();
            }else if (mainMenu == "Report on students who need makeup lessons"){
                GetReportOnStudentMakeUpLesson();
            }else if (mainMenu == "Rescheduling the lesson"){
                ReschedulingLesson();        
            }else if (mainMenu == "Canceling and removing a lesson"){
                CancelingRemovingLesson();       
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
        Student selectedStudent = ChooseStudentAddNewStudent();
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
            selectedAvailableTimeSlot.Teacher.Id,selectedStudent.Id);

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
    public Student? ChooseStudentAddNewStudent(){
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
                _dataManager.SaveData();
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
        string studentPaymentStatus = _dataManager.GetStudentPaymentStatus(selectedYear,selectedMonth,selectedStudent.Id);
        AnsiConsole.MarkupLine ($"Student's status: {studentPaymentStatus}");
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("For return to main menu choose 'Back to main menu'").
            AddChoices(new[] { "Back to main menu"}));
    }
    public void RegisterNeedForMakeUpLesson(){
        AnsiConsole.MarkupLine ("Registering a need for makeup lessons"); 
        Student selectedStudent = ChooseStudent();
        AnsiConsole.MarkupLine ($"Selected student: {selectedStudent.Name}");
        if(selectedStudent.NeedForMakeUpLesson){
            AnsiConsole.MarkupLine ($"For the student already there is a registered need for the makeup lesson");
            AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("For return to main menu choose 'Back to main menu'").
            AddChoices(new[] { "Back to main menu"}));
            return;
        }
        selectedStudent.RegisterNeedForMakeUpLesson();
        _dataManager.SaveData();
        AnsiConsole.MarkupLine ($"The need for the makeup lesson is registered");
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("For return to main menu choose 'Back to main menu'").
            AddChoices(new[] { "Back to main menu"}));

    }   
    public Student ChooseStudent(){
        var selectedStudent = AnsiConsole.Prompt(
                new SelectionPrompt<Student>()
                .Title("Choose student")
                .UseConverter(s => s.Name)
                .AddChoices(_dataManager.Students)); 
        return selectedStudent;   
    }
    public void GetReportOnStudentMakeUpLesson(){
        AnsiConsole.MarkupLine("Report on students make up lesson");
        List<Student> needForMakeUpLessonReport = _dataManager.GetNeedForMakeUpLessonReport();
        if(needForMakeUpLessonReport.Count == 0){
            AnsiConsole.MarkupLine ($"There no students who is marked for the make up lesson");
            AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("For return to main menu choose 'Back to main menu'").
                AddChoices(new[] { "Back to main menu"}));
            return;
        }
        foreach(var student in needForMakeUpLessonReport){
            AnsiConsole.MarkupLine(student.Name);
            
        }
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("For return to main menu choose 'Back to main menu'").
                AddChoices(new[] { "Back to main menu"}));
        return;

    }
    public void ReschedulingLesson(){
        AnsiConsole.MarkupLine("ReschedulingLesson");
        Subject selectedSubject = ChooseSubject();
        AnsiConsole.MarkupLine ($"Selected subject: {selectedSubject.Name}");
        while(true){
            DateOnly selectedDate = EnterDate();
            AnsiConsole.MarkupLine ($"Selected date: {selectedDate}");
            
            List<Lesson> lessonBySubjectId = _dataManager.GetLessonsBySubjectId(selectedDate, selectedSubject.Id);
            if(lessonBySubjectId.Count == 0){
                AnsiConsole.MarkupLine("There no lesson on selected date");
                AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("For return to enter date 'Back to enter date'").
                    AddChoices(new[] { "Back to enter date"}));
            continue;
            }
            var selectedLesson = AnsiConsole.Prompt(
                    new SelectionPrompt<Lesson>()
                    .Title("Choose lesson")
                    .UseConverter(ls => $"{ls.TimeSlot.ToStr()}    {_dataManager.GetSubject(ls.SubjectId).Name}    {_dataManager.GetStudent(ls.StudentId).Name}")
                    .AddChoices(lessonBySubjectId));
            AnsiConsole.MarkupLine($"Selected lesson {selectedLesson.TimeSlot.ToStr()}");
            DateOnly selectedNewDate =  EnterNewDate();
            List<AvailableTimeSlot> availableTimeSlots = _dataManager.GetAvailableTimeSlots(selectedNewDate,_dataManager.GetSubject(selectedSubject.Id));
            if(availableTimeSlots.Count == 0){
                AnsiConsole.MarkupLine ("There are not available time slots on selected date");
                AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("For return to enter date 'Back to enter date'").
                    AddChoices(new[] { "Back to enter date"}));
                continue;
            }AvailableTimeSlot selectedAvailableTimeSlot = ChooseAvailableTimeSlot(availableTimeSlots);
            AnsiConsole.MarkupLine ($"Selected time slot: {selectedAvailableTimeSlot.TimeSlot.ToStr()}");
            bool isStudentAssignmentToLesson = _dataManager.IsStudentAssignmentToLesson(selectedNewDate,selectedAvailableTimeSlot.TimeSlot,selectedLesson.StudentId);
            if (isStudentAssignmentToLesson){
                AnsiConsole.MarkupLine($"Selected student: {_dataManager.GetStudent(selectedLesson.StudentId).Name} already has lesson at {selectedAvailableTimeSlot.TimeSlot.ToStr()} on {selectedNewDate}");
                AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("For return to enter date 'Back to enter date'").
                    AddChoices(new[] { "Back to enter date"}));
                continue;
            }
            string selectedActionSaveOrExit = ChooseActionSaveOrExit();
            if(selectedActionSaveOrExit == "Save"){
                selectedLesson.Date = selectedNewDate;
                selectedLesson.TimeSlot = selectedAvailableTimeSlot.TimeSlot;
                selectedLesson.RoomId = selectedAvailableTimeSlot.Room.Id;
                selectedLesson.TeacherId = selectedAvailableTimeSlot.Teacher.Id;
                _dataManager.SaveData();
                return;
            }
        }
        
    }
    public DateOnly EnterNewDate(){
        var datePrompt= new TextPrompt<DateOnly>("Enter new date (yyyy-mm-dd)")
            .ValidationErrorMessage("Wrong date format");
        DateOnly selectedDate = AnsiConsole.Prompt(datePrompt);
        return selectedDate;
    }
    public void CancelingRemovingLesson(){
        AnsiConsole.MarkupLine("Canceling and removing a lesson");
        Subject selectedSubject = ChooseSubject();
        AnsiConsole.MarkupLine ($"Selected subject: {selectedSubject.Name}");
        while(true){
            DateOnly selectedDate = EnterDate();
            AnsiConsole.MarkupLine ($"Selected date: {selectedDate}");
            List<Lesson> lessonBySubjectId = _dataManager.GetLessonsBySubjectId(selectedDate, selectedSubject.Id);
            if(lessonBySubjectId.Count == 0){
                AnsiConsole.MarkupLine("There no lesson on selected date");
                var selectedMenuItem = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("For return to enter date 'Back to enter date'").
                    AddChoices(new[] { "Back to enter date", "Back to main menu"}));
                if (selectedMenuItem == "Back to enter date"){
                    continue;
                }
                else{
                    return;
                }
                
            }
            var selectedLesson = AnsiConsole.Prompt(
                    new SelectionPrompt<Lesson>()
                    .Title("Choose lesson")
                    .UseConverter(ls => $"{ls.TimeSlot.ToStr()}    {_dataManager.GetSubject(ls.SubjectId).Name}    {_dataManager.GetStudent(ls.StudentId).Name}")
                    .AddChoices(lessonBySubjectId));
            _dataManager.Lessons.Remove(selectedLesson);
            _dataManager.SaveData();
            AnsiConsole.MarkupLine("The selected lesson is removed from schedule");
            AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("For return to enter date 'Back to enter date'").
                    AddChoices(new[] { "Back to enter date"}));
            return;

        }
    }
    public void MarkCurrentLevelAsCompleted(){
        AnsiConsole.MarkupLine("Mark current level of study as completed");
        Student selectedStudent = ChooseStudent();
        if(selectedStudent.CompletionCurrentLevel){
            AnsiConsole.MarkupLine("Current level of study is already marked as completed");
            AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("For return to main menu choose 'Back to main menu'").
                AddChoices(new[] { "Back to main menu"}));
            return;
        }
        selectedStudent.MarkCurrentLevelAsCompleted();
        _dataManager.SaveData();
        AnsiConsole.MarkupLine("Student's current level of study is marked as completed");
            AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("For return to main menu choose 'Back to main menu'").
                AddChoices(new[] { "Back to main menu"}));
            return;
    }
    public void TransferStudentToNextLevelOfStudy(){
        AnsiConsole.MarkupLine("Transfer a student to the next of study");
        Student selectedStudent = ChooseStudent();
        if(!selectedStudent.CompletionCurrentLevel){
            AnsiConsole.MarkupLine("Transfer is not allowed. The current level is not completed");
            AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("For return to main menu choose 'Back to main menu'").
                AddChoices(new[] { "Back to main menu"}));
            return;
        } else if(selectedStudent.EducationLevel == EducationLevel.diploma){
            AnsiConsole.MarkupLine("Current level is highest");
            AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("For return to main menu choose 'Back to main menu'").
                AddChoices(new[] { "Back to main menu"}));
            return;
        } else{
            selectedStudent.NextEducationLevel();
            _dataManager.SaveData();
        }

    }                
    public void ShowStudentsEducationLevelAndStatusOfCompletionCurrentLevel(){
        AnsiConsole.MarkupLine("Show a student's education level and a status of completion current level");
        Student selectedStudent = ChooseStudent();
        if(!selectedStudent.CompletionCurrentLevel){
            AnsiConsole.MarkupLine($"Current education level {selectedStudent.EducationLevel.ToString()}. Current level is not completed");
            AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("For return to main menu choose 'Back to main menu'").
                AddChoices(new[] { "Back to main menu"}));
            return;
        }else{
            AnsiConsole.MarkupLine($"Current education level {selectedStudent.EducationLevel.ToString()}. Current level is completed");
            AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("For return to main menu choose 'Back to main menu'").
                AddChoices(new[] { "Back to main menu"}));
            return;
        }

    }
    

}