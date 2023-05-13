using System;
using System.Collections.Generic;

namespace game2048;

public readonly struct Point : IEquatable<Point>
{
    public static Point Zero;
    public static Point Error = new Point(byte.MaxValue, byte.MaxValue);

    public static byte Normalize(byte range, int xy) => xy < 0
        ? (byte)0
        : xy >= range
            ? (byte)(range - 1)
            : (byte)xy;

    public Point(byte x, byte y)
    {
        X = x;
        Y = y;
    }

    public readonly byte X;
    public readonly byte Y;

    #region IEquatable

    bool IEquatable<Point>.Equals(Point p) => X == p.X && Y == p.Y;

    #endregion

    public bool Try(byte range, int dx, int dy, out Point p)
    {
        p = new Point(Normalize(range, X + dx), Normalize(range, Y + dy));
        return !this.Equals(p);
    }

    public IEnumerable<Point> Neighbours4(byte range)
    {
        if (Try(range, -1, 0, out var p))
        {
            yield return p;
        }

        if (Try(range, 1, 0, out p))
        {
            yield return p;
        }

        if (Try(range, 0, -1, out p))
        {
            yield return p;
        }

        if (Try(range, 0, 1, out p))
        {
            yield return p;
        }
    }

    public override string ToString() => $"{nameof(Point)}[{X}:{Y}]";
}