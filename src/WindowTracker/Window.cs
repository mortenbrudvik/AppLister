using PInvoke;

using static WindowTracker.Error;

namespace WindowTracker;

public class Window
{
    private readonly nint _handle;

    public Window(nint handle)
    {
        _handle = handle;
    }

    public string Title => TryCatch(() => User32.GetWindowText(_handle), "");

    public bool IsSplashScreen => ClassName == "MsoSplash";
    public string ClassName => User32.GetClassName(_handle);
    public bool IsMinimized => User32.IsIconic(_handle);

    public bool IsProcessable()
    {
        if (IsSplashScreen) return false;

        // For windows that shouldn't process (start menu, tray, popup menus) 
        // VirtualDesktopManager is unable to retrieve virtual desktop id and returns an error.
        var virtualDesktop = new VirtualDesktopManager();

        try
        {
            var desktopId = virtualDesktop.GetWindowDesktopId(_handle);
            var isOnCurrentDesktop = virtualDesktop.IsWindowOnCurrentVirtualDesktop(_handle);

            if (isOnCurrentDesktop || desktopId != Guid.Empty)
                return true;
        }
        catch (Exception e)
        {
            // ignored
        }

        return false;
    }

    public bool IsStandard
    {
        get
        {
            if (User32.GetAncestor(_handle, User32.GetAncestorFlags.GA_ROOT) != _handle)
                return false;

            var style = (User32.WindowStyles) User32.GetWindowLong(_handle, User32.WindowLongIndexFlags.GWL_STYLE);
            var styleEx =
                (User32.WindowStylesEx) User32.GetWindowLong(_handle, User32.WindowLongIndexFlags.GWL_EXSTYLE);

            var isToolWindow = (styleEx & User32.WindowStylesEx.WS_EX_TOOLWINDOW) ==
                               User32.WindowStylesEx.WS_EX_TOOLWINDOW;
            var isVisible = (style & User32.WindowStyles.WS_VISIBLE) == User32.WindowStyles.WS_VISIBLE;

            if (isToolWindow || !isVisible)
                return false;

            bool IsSystemWindow(string className) =>
                new[] {"SysListView32", "WorkerW", "Shell_TrayWnd", "Shell_SecondaryTrayWnd", "Progman"}
                    .Contains(className);

            if (IsSystemWindow(ClassName))
                return false;

            var desktopWindow = User32.GetDesktopWindow();
            var shellWindow = User32.GetShellWindow();
            if (desktopWindow == _handle || shellWindow == _handle)
                return false;

            return true;
        }
    }

    public bool IsPopup
    {
        get
        {
            var style = (User32.WindowStyles)User32.GetWindowLong(_handle, User32.WindowLongIndexFlags.GWL_STYLE);
            return (style & User32.WindowStyles.WS_POPUP) == User32.WindowStyles.WS_POPUP;
        }
    }

    public static IEnumerable<Window> GetWindows()
    {
        var windows = new List<Window>();
        User32.EnumWindows((hWnd, _) =>
        {
            var window = new Window(hWnd);
            if (window.IsProcessable())
                windows.Add(window);
            return true;
        }, nint.Zero);

        return windows;
    }

    public static IEnumerable<Window> GetApplicationWindows() =>
        GetWindows()
            .Where(window => window is {IsStandard: true, IsPopup: false}).ToList();

    public override string ToString() => $"Title: {Title}, ClassName: {ClassName}";
}