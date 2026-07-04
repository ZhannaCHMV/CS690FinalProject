namespace MusicSchool;
public enum TimeSlot{
    s9, 
    s10, 
    s11,
    s14,
    s15,
    s16,
    s17,
    s18,
    s19
}
public static class TimeSlotExtensions{
    public static string ToStr(this TimeSlot timeSlot){
        string name = timeSlot.ToString();
        string hour = name.Substring(1);
        return $"{hour}:00";
    }
}