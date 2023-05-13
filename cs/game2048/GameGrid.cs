using System.Collections.Generic;
using System.Linq;

namespace game2048;

/// <summary>
/// ┌────> x
/// │
/// ˅ 
/// Y
/// </summary>
public class GameGrid : Dictionary<Point, uint>
{
    private readonly byte _range;
    
    public GameGrid(byte range)
    {
        _range = range;
        for (byte x = 0; x < range; x++)
        {
            for (byte y = 0; y < range; y++)
            {
                this[new Point(x, y)] = 0;
            }
        }
    }


    public new void Clear()
    {
        foreach (var p in Keys)
        {
            this[p] = 0;
        }
    }

    public IEnumerable<Point> Col(byte idx)
        => Keys.Where(i => i.X == idx).OrderBy(i => i.Y);

    public IEnumerable<Point> Row(byte idx)
        => Keys.Where(i => i.Y == idx).OrderBy(i => i.X);

    public bool CopyValues(Dictionary<Point, uint> src)
    {
        if (Keys.Count == src.Keys.Count)
        {
            foreach (var k in src.Keys)
            {
                this[k] = src[k];
            }

            return true;
        }
        return false;
    }
    
    public GameGrid Clone()
    {
        var res = new GameGrid(_range);
        res.CopyValues(this);
        return res;
    }
}