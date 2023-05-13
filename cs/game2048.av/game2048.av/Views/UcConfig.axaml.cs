using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace game2048.av.Views;

public partial class UcConfig : Grid
{
    public UcConfig()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}