namespace MusicSchool.Tests;

public class RoomTest
{
    [Fact]
    public void TestRoomMain()
    {
        Room room = new Room(1,"Room 1");
        Assert.Equal(1,room.Id);
        Assert.Equal("Room 1", room.Name); 
    }
}
