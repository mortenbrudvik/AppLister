namespace ApplicationTracker.IntegrationTests;

public class Win32ProgramTests
{
    [Fact]
    public void TestWin32Program()
    {
        var settings = new ProgramPluginSettings();
        var apps = Win32Program.All(settings);
        Assert.NotEmpty(apps);
    }
}