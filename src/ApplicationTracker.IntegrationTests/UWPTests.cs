namespace ApplicationTracker.IntegrationTests;

public class UWPTests
{
    [Fact]
    public void TestUWP()
    {
        var apps = UWP.All();
        Assert.NotEmpty(apps);
    }
}