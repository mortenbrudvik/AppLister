using System.Windows.Media;

namespace AppLister;

public class Result
{
    public string SubTitle { get; set; }
    public IconDelegate Icon { get; set; }
    public int Score { get; set; }
    public object ContextData { get; set; }
    public string ProgramArguments { get; set; }
    public Func<ActionContext, bool> Action { get; set; }
    public string Title { get; set; }
    public List<int> TitleHighlightData { get; set; }
    public string IcoPath { get; set; }
}
public delegate ImageSource IconDelegate();
public class ActionContext
{
    public SpecialKeyState SpecialKeyState { get; set; }
}
public class SpecialKeyState
{
    public bool CtrlPressed { get; set; }

    public bool ShiftPressed { get; set; }

    public bool AltPressed { get; set; }

    public bool WinPressed { get; set; }
}
