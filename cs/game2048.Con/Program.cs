using System;
using System.Linq;

namespace game2048.Con;

public class Program
{
    public static void Main(params string[] args)
    {
        var p = new Program();
        p.Start();
    }

    private readonly ConfigCon _config;
    private Program()
    {
        _config = new ConfigCon();
    }

    private void Start()
    {
        ConsoleKey[] toNext =
            { ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.UpArrow, ConsoleKey.R };

        var game = new Game(_config);
        game.Reset();
        Draw(game.GameGrid);

        ConsoleKey k;
        while (toNext.Contains(k = Console.ReadKey().Key))
        {
            if (k == ConsoleKey.R)
            {
                game.Reset();
            }
            else
            {
                var (dir, c) = k switch
                {
                    ConsoleKey.DownArrow => (Direct.Down, '↓'),
                    ConsoleKey.LeftArrow => (Direct.Left, '←'),
                    ConsoleKey.RightArrow => (Direct.Right, '→'),
                    ConsoleKey.UpArrow => (Direct.Up, '↑'),
                    _ => throw new OutOfMemoryException("bad key")
                };
                Console.WriteLine($"                {c,-20}");

                if (!game.Next(dir))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game over!");
                    Console.ForegroundColor = ConsoleColor.White;
                    k = Console.ReadKey().Key;
                    if (k == ConsoleKey.R)
                    {
                        game.Reset();
                    }
                    else
                    {
                        return;
                    }
                }
            }

            Draw(game.GameGrid);
        }

        Console.ReadKey();
    }

// ╔═╤═╗
// ║ │ ║
// ╟─┼─╢
// ╚═╧═╝
    void Draw(GameGrid p)
    {
        // ConsoleColor FG(uint value)
        //     => value switch
        //     {
        //         0x00 => ConsoleColor.Black,
        //         0x01 => ConsoleColor.Red,
        //         0x02 => ConsoleColor.DarkRed,
        //         0x04 => ConsoleColor.Magenta,
        //         0x08 => ConsoleColor.DarkMagenta,
        //         0x10 => ConsoleColor.Yellow,
        //         0x20 => ConsoleColor.DarkYellow,
        //         0x40 => ConsoleColor.Cyan,
        //         0x80 => ConsoleColor.Blue,
        //         _ => ConsoleColor.Green
        //     };
#if !_DEBUG
        Console.Clear();
#endif

        // Console.WriteLine("╔══════╤══════╤══════╤══════╗");
        Console.WriteLine("┌──────┬──────┬──────┬──────┐");
        for (byte r = 0; r < _config.Range; r++)
        {
            var row = p.Row(r)
                .Select(i => p[i])
                // .Select(i => i == 0 ? " " : i.ToString())
                .ToArray();

            Console.Write("│");
            foreach (var v in row)
            {
                Console.ForegroundColor = _config.ForeColors.GetColor(v);
                var s = v == 0 ? "" : v.ToString();
                Console.Write($"{s,4}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("  │");
            }

            Console.WriteLine();
            Console.WriteLine("├──────┼──────┼──────┼──────┤");
        }

        Console.CursorTop--;
        Console.WriteLine("└──────┴──────┴──────┴──────┘");

        Console.WriteLine("arrows - to next, r - reset, other - exit");
        Console.CursorTop--;
    }
}

public class ConfigCon : Config
{
    public ConfigCon()
    {
        AppendCount = 2;
        Range = 4;
        ForeColors = new Colors<ConsoleColor>
        {
            { 0x00, ConsoleColor.Black },
            { 0x01, ConsoleColor.Red },
            { 0x02, ConsoleColor.DarkRed },
            { 0x04, ConsoleColor.Magenta },
            { 0x08, ConsoleColor.DarkMagenta },
            { 0x10, ConsoleColor.Yellow },
            { 0x20, ConsoleColor.DarkYellow },
            { 0x40, ConsoleColor.Cyan },
            { 0x80, ConsoleColor.Blue },
        };
        ForeColors.Default = ConsoleColor.Green;
    }
    
    public Colors<ConsoleColor> ForeColors { get; }
}