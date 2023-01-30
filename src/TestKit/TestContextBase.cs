using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;

namespace TestKit;

/// <summary>
/// Wrapper class for <see cref="XunitContext"/> that implements <see cref="IDisposable"/>.
/// </summary>
public abstract class TestContextBase : IDisposable
{
    private readonly Context _context;

    protected TestContextBase(ITestOutputHelper output, [CallerFilePath] string sourceFile = "")
    {
        Guard.Against.Null(output, nameof(output));
        Guard.Against.NullOrEmpty(sourceFile, nameof(sourceFile));

        Logger = output;
        _context = XunitContext.Register(output, sourceFile);
    }

    public ITestOutputHelper Logger { get; }
    
    public void WriteLine(object value) => _context.WriteLine(value);
    public void WriteLine(string value) => _context.WriteLine(value);
    
    public void Write(char value) => _context.Write(value);
    public void WriteLine() => _context.WriteLine();
    
    public Exception? TestException => _context.TestException;
    public string SourceFile => _context.SourceFile;
    public string SourceDirectory => _context.SourceDirectory;
    public string SolutionDirectory => _context.SolutionDirectory;
    public string UniqueTestName => _context.UniqueTestName;
    
    public void Dispose() => _context.Flush();
}