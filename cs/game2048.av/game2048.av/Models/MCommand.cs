using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Layout;

namespace game2048.av.Models;

public class MCommand
{
    public MCommand(ICommand command)
        => Command = command;
    
    public ICommand Command { get; }
    public object? CommandParameter { get; set; }
    public KeyGesture? HotKey { get; set; }
    public string? Tooltip { get; set; }
}

public class MCommandIcon : MCommand
{
    public MCommandIcon(ICommand command, string icon) : base(command)
    {
        Icon = icon;
    }
    
    public string Icon { get; set; }
}

public class VmMenuItem : MCommandIcon
{
    public VmMenuItem(ICommand command, string icon, string header) : base(command, icon)
    {
        Header = header;
        Orientation = Orientation.Horizontal;
    }

    public string Header { get; }
    // public IList<VmMenuItem> Items { get; set; }
    public Orientation Orientation { get; set; }
}