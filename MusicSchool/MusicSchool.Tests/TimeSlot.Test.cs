namespace MusicSchool.Tests;
public class TimeSlotTest{
    [Fact]
    public void TimeSlotTestMain(){
        Assert.Equal(0, (int)TimeSlot.s9);
        Assert.Equal(1, (int)TimeSlot.s10);
        Assert.Equal(2, (int)TimeSlot.s11);
        Assert.Equal(3, (int)TimeSlot.s14);
        Assert.Equal(4, (int)TimeSlot.s15);
        Assert.Equal(5, (int)TimeSlot.s16);
        Assert.Equal(6, (int)TimeSlot.s17);
        Assert.Equal(7, (int)TimeSlot.s18);
        Assert.Equal(8, (int)TimeSlot.s19);
     
    }
}