using Avalonia.Media;

namespace game2048.av.Models;

public class ConfigAv : Config
{
    public ConfigAv()
    {
        Background = new Colors<Color>
        {
            { 0x0001, Colors.Red },
            { 0x0002, Colors.OrangeRed },
            { 0x0004, Colors.DarkOrange },
            { 0x0008, Colors.Orange },
            { 0x0010, Colors.GreenYellow },
            { 0x0020, Colors.YellowGreen },
            { 0x0040, Colors.Cyan },
            { 0x0080, Colors.DarkCyan },
            { 0x0100, Colors.LightGreen },
        };
        Background.Default = Colors.Aqua;
    }


    public Colors<Color> Background { get; set; }
}