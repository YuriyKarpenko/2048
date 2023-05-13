using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using game2048.av.Models;

namespace game2048.av.Views;

public class UcButton : UserControl
{
    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is MCommand mc)
        {
            var btn = new Button
            {
                Command = mc.Command,
                CommandParameter = mc.CommandParameter,
                HotKey = mc.HotKey,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            if (!string.IsNullOrEmpty(mc.Tooltip))
            {
                ToolTip.SetTip(btn, mc.Tooltip);
            }

            switch (mc.GetType().Name)
            {
                case nameof(MCommandIcon):
                    btn.Content = new UcIcon();
                    break;

                case nameof(VmMenuItem):
                    btn.Content = new UcButtonContent();
                    break;
            }

            Content = btn;
            // HorizontalContentAlignment = HorizontalAlignment.Stretch;
            // VerticalContentAlignment = VerticalAlignment.Stretch;
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
            if (TryGetIcon(mi.Icon, out var g))
            {
                icon.Data = g!;
            }

            Child = icon;
        }
    }

    public static bool TryGetIcon(string key, out Geometry? icon)
    {
        if (Application.Current!.Resources.TryGetValue(key, out var res) && res is Geometry g)
        {
            icon = g;
            return true;
        }

        icon = default;
        return false;
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

            if (UcIcon.TryGetIcon(mi.Icon, out var g))
            {
                icon.Data = g!;
            }

            Children.Clear();
            Children.Add(icon);
            Children.Add(text);
        }
    }
}