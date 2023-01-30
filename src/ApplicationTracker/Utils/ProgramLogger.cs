namespace ApplicationTracker;

public static class ProgramLogger
{
    public static void Exception(string message, Exception exception, Type type, string additionalInfo = null)
    {
        var logMessage = $"[{type.Name}] {message} - {exception.Message} - {exception.StackTrace}";
        if (additionalInfo != null)
        {
            logMessage += $" - {additionalInfo}";
        }
    }

    public static void Warn(string s, Exception exception, Type? declaringType, string path)
    {
        
        
    }
}