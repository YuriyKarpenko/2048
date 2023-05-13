using System;
using Avalonia.Controls;
using Avalonia.Media;
using game2048.av.Models;

namespace game2048.av.Views;

public class UcButton : UserControl
{
    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is VmMenuItem mi)
        {
            var btn = new Button
            {
                Command = mi.Command,
                Content = new UcButtonContent()
            };
            Content = btn;
        }
    }
}

public class UcIcon : Border
{
    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is MCommandIcon mi)
        {
            var icon = new PathIcon();
            if (App.Current.Resources.TryGetValue(mi.Icon, out var res) && res is Geometry g)
            {
                icon.Data = g;
            }

            Child = icon;
        }
    }
}

public class UcButtonContent : StackPanel
{
    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is VmMenuItem mi)
        {
            Orientation = mi.Orientation;
            var icon = new PathIcon();
            var text = new TextBlock { Text = mi.Header };

            if (App.Current.Resources.TryGetValue(mi.Icon, out var res) && res is Geometry g)
            {
                icon.Data = g;
            }

            Children.Clear();
            Children.Add(icon);
            Children.Add(text);
        }
    }
}