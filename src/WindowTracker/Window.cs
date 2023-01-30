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
        if(IsSplashScreen) return false;
        
        // For windows that shouldn't process (start menu, tray, popup menus) 
        // VirtualDesktopManager is unable to retrieve virtual desktop id and returns an error.
        var virtualDesktop = new VirtualDesktopManager();
        
        try
        {
            var isOnCurrentDesktop = virtualDesktop.IsWindowOnCurrentVirtualDesktop(_handle);
            var desktopId = virtualDesktop.GetWindowDesktopId(_handle);
            
            if(isOnCurrentDesktop || desktopId != Guid.Empty)
                return true;
        }
        catch (Exception e)
        {
            // ignored
        }

        return false;        
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
    
    public override string ToString() => $"Title: {Title}, ClassName: {ClassName}";
}