using System;
using Avalonia.Controls;
using Avalonia.Layout;
using game2048.av.ViewModels;

namespace game2048.av.Views;

public class UcCell : UserControl
{
    private static TextBlock InitTb(TextBlock txt)
    {
        txt.FontSize = 60;
        // txt.Margin = new Thickness(2);
        txt.HorizontalAlignment = HorizontalAlignment.Center;
        txt.VerticalAlignment = VerticalAlignment.Center;
        return txt;
    }
    
    
    private readonly TextBlock _text = InitTb(new());

    public UcCell()
    {
        Content = _text;
        // base.BorderBrush = Brushes.Beige;
        // base.BorderThickness = new Thickness(2);
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is VmCell c)
        {
            Background = c.Back;
            _text.Text = c.V.ToString();
            _text.IsVisible = c.V > 0;
            UpdateGrid(c);
        }
    }


    private void UpdateGrid(VmCell c)
    {
        Grid.SetColumn(this, c.X);
        Grid.SetRow(this, c.Y);
    }

    private static bool TryFindPatrnt<T>(Control o, out T? val) where T : class
    {
        var parent = o.Parent;
        while (parent != null)
        {
            if (parent is T t)
            {
                val = t;
                return true;
            }

            parent = parent.Parent;
        }

        val = default;
        return false;
    }
}