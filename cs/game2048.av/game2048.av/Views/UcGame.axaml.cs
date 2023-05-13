using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace game2048.av.Views;

public partial class UcGame : Grid
{
    public UcGame()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}