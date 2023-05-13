using System.Collections.Generic;
using Avalonia.Media;
using game2048.av.Models;

namespace game2048.av.ViewModels;

public class VmCell
{
    public static VmCell From(KeyValuePair<Point, uint> src, ConfigAv config) => new VmCell(src, config);

    public VmCell(KeyValuePair<Point, uint> src, ConfigAv config)
    {
        X = src.Key.X;
        Y = src.Key.Y;
        V = src.Value;
        Back = new Avalonia.Media.Immutable.ImmutableSolidColorBrush(config.Background.GetColor(V));
    }

    public byte X { get; }
    public byte Y { get; }
    public uint V { get; }
    public IBrush Back { get; }
    

    public override string ToString()
    {
        return $"{nameof(VmCell)}[{X}:{Y}={V}]";
    }
}