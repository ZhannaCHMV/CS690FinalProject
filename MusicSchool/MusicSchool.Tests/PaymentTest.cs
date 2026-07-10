namespace MusicSchool.Tests;

public class PaymentTest
{
    [Fact]
    public void TestPaymentMain()
    {
        Payment payment = new Payment(7,2026,1);
        Assert.Equal(7,payment.Month);
        Assert.Equal(2026,payment.Year); 
        Assert.Equal(1,payment.StudentId);
    }
}
