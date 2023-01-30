using FluentAssertions;
using TestKit;
using Xunit.Abstractions;

namespace WindowTracker.IntegrationTests;

public class WindowTests : TestContextBase
{
    [Fact]
    public void Test1()
    {
        var windows = Window.GetWindows().ToList();
        
        windows.Log().Should().NotBeEmpty();
    }

    public WindowTests(ITestOutputHelper output) : base(output)
    {
    }
}