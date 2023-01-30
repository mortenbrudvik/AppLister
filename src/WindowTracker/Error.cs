namespace WindowTracker;

public static class Error
{
    public static TR? TryCatch<TR>(Func<TR?> action) 
        => TryCatch(action, _ => { });

    public static TR TryCatch<TR>(Func<TR> action, TR defaultValue) 
        => TryCatch(action, _ => defaultValue);
    
    public static void TryCatch(Action action) =>
        TryCatch(action, _ => {});

    public static async Task TryCatch(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public static void TryCatch(Action action, Action<Exception> exceptionHandling)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            exceptionHandling(e);
        }
    }
    
    public static TR TryCatch<TR>(Func<TR> func, Func<Exception, TR> exceptionHandling)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            return exceptionHandling(e);
        }
    }
    
    public static TR? TryCatch<TR>(Func<TR> func, Action<Exception> exceptionHandling)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            exceptionHandling(e);
        }
        return default;
    }
}