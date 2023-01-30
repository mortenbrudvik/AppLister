namespace TestKit;

public static class LoggerExtensions
{
    public static IEnumerable<T> Log<T>(this IEnumerable<T> source)
    {
        var items = source.ToList();
        foreach (var element in items)
        {
            XunitContext.WriteLine("" + element);
        }

        return items;
    }
}