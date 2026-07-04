using MusicSchool;
internal class Program{
    static void Main(string[] args){
        DataManager dataManager = new DataManager();
        dataManager.LoadData();
        ConsoleUI consoleUI = new ConsoleUI(dataManager);
        consoleUI.Show();
    }
}
