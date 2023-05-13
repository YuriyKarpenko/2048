using System;
using System.Linq;

namespace game2048;

public class Game
{
    private static readonly Random Rnd = new Random(DateTime.Now.GetHashCode());
    private readonly Config _config;
    
    public Game(Config config)
    {
        _config = config;
        GameGrid = new GameGrid(_config.Range);

        Reset();
    }

    public GameGrid GameGrid { get; }


    public void Reset()
    {
        GameGrid.Clear();
        AppendValues();
    }

    /// <summary> Next step of game </summary>
    /// <param name="direct"></param>
    /// <returns>true - Ok, false - game over</returns>
    public bool Next(Direct direct)
    {
        for (byte i = 0; i < _config.Range; i++)
        {
            var pp = direct switch
            {
                Direct.Down => GameGrid.Col(i).Reverse(),
                Direct.Up => GameGrid.Col(i),
                Direct.Left => GameGrid.Row(i),
                Direct.Right => GameGrid.Row(i).Reverse(),
                _ => throw new OutOfMemoryException("Invalid direction")
                // _ => Enumerable.Empty<Point>()
            };
            Collapse(pp.ToArray());
        }

        return AppendValues();
    }

    
    private void Collapse(Point[] points)
    {
        bool TryFind(Point[] a, int startIdx, out Point p)
        {
            for (int i = startIdx; i < a.Length; i++)
            {
                p = a[i];
                if (GameGrid[p] > 0)
                {
                    return true;
                }
            }

            p = Point.Error;
            return false;
        }

        int i = 0;
        while (TryFind(points, i + 1, out var src))
        {
            var dst = points[i++];
            var v = GameGrid[src];
            if (v == GameGrid[dst]) //  join
            {
                v *= 2;
            }
            else if (GameGrid[dst] > 0) //  dst is not empty, try next point
            {
                continue;
            }
            else //  move to empty, need to test again
            {
                i--;
            }

            if (!src.Equals(dst)) // no erase self
            {
                (GameGrid[dst], GameGrid[src]) = (v, 0u);
            }
        }
    }

    private bool AppendValues()
    {
        bool AppendValue()
        {
            var freeCells = GameGrid
                .Where(i => i.Value == 0)
                .Select(i => i.Key)
                .ToArray();

            var p = freeCells.Length switch
            {
                0 => Point.Error,
                1 => freeCells[0],
                _ => freeCells[Rnd.Next(freeCells.Length)]
            };

            if (Point.Error.Equals(p))
            {
                return false;
            }

            GameGrid[p] = RandomValue();
            return true;
        }
        
        var res =  Enumerable.Range(0, _config.AppendCount).All(_ => AppendValue());
        return res;
    }

    //  TODO: надо оценивать ситуацию на поле
    private uint RandomValue()
    {
        // var res = new List<uint>();
        // return new[] { 1u, 2u };
        return (uint)Rnd.Next(1, 3);
    }

    // private bool CanPut(byte x, byte y, uint value)
    // {
    //     return Grid.First(i => i.Key.X == x && i.Key.Y == y).Value == 0;
    // }

    // //  is there a cell with <value> around the specified one
    // private bool HasNear(Point p, uint value)
    //     => p.Neighbours4(_config.Range).Any(i => Grid[i] == value);
}