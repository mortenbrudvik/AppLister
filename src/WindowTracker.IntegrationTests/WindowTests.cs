using FluentAssertions;
using Kassei.TestKit;
using Xunit.Abstractions;

namespace WindowTracker.IntegrationTests;

public class WindowTests : TestContextBase
{
    [Fact]
    public void Test1()
    {
        var windows = Window.GetApplicationWindows().ToList();

        windows.Log().Should().NotBeEmpty();
    }

    public WindowTests(ITestOutputHelper output) : base(output)
    {
    }
}