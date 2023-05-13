using System;
using System.Collections;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.Templates;
using game2048.av.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;

namespace game2048.av.Views;

public class UcGrid : Grid
{
    public static readonly StyledProperty<IEnumerable> CellsProperty =
        AvaloniaProperty.Register<UcGrid, IEnumerable>(nameof(Cells));

    private static void OnCellsChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
    {
        // return;
        if (d is UcGrid g && e.NewValue is VmCell[] cc)
        {
            //  поле Cells обновляется раньше поля Range, утаноим его перед заполнением
            var r = Math.Sqrt(cc.Length);
            g.Range = (byte)r;

            g.Children.Clear();
            foreach (var c in cc)
            {
                var ic = CreateChild(g, c);
                if (ic != null)
                {
                    g.Children.Add(ic);
                }
            }
        }
    }

    static IControl? CreateChild(Grid d, object? content)
    {
        var newChild = content as IControl;
        if (newChild == null && content != null)
        {
            var dataTemplate = d.FindDataTemplate(content) ?? FuncDataTemplate.Default;
            newChild = dataTemplate.Build(content);
            newChild.DataContext = content;
        }

        return newChild;
    }

    public static readonly StyledProperty<byte> RangeProperty = AvaloniaProperty.Register<UcGrid, byte>(nameof(Range));

    private static void OnRangeChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
    {
        if (d is UcGrid grid && e.NewValue is byte value)
        {
            var defs = string.Join(",", Enumerable.Repeat("1*", value));
            grid.ColumnDefinitions = ColumnDefinitions.Parse(defs);
            grid.RowDefinitions = RowDefinitions.Parse(defs);
        }
    }

    static UcGrid()
    {
        CellsProperty.Changed.AddClassHandler<UcGrid>(OnCellsChanged);
        RangeProperty.Changed.AddClassHandler<UcGrid>(OnRangeChanged);
    }

    // private readonly GestureRecognizerCollection gestures;
    public UcGrid()
    {
        Gestures.ScrollGestureEvent.Raised
            .Where((o, _) => o.Item1 is UcGame)
            // .Sample(TimeSpan.FromMilliseconds(200))  //  другой поток
            // .Throttle(TimeSpan.FromMilliseconds(200))
            .Select(i => i.Item2 as ScrollGestureEventArgs)
            .Subscribe(OnScrollEnd);
#if DEBUG
        // Background = Brushes.Azure;
        ShowGridLines = true;
#endif
    }

    private bool _lock;
    private void OnScrollEnd(ScrollGestureEventArgs? e)
    {
        if (!_lock && e?.Delta.Length > 20 && DataContext is VmGame vm)
        {
            Direct AsDirect(Vector v)
            {
                return Math.Abs(v.X) > Math.Abs(v.Y)
                    ? v.X < 0 
                        ? Direct.Right 
                        : Direct.Left
                    : v.Y < 0
                        ? Direct.Down
                        : Direct.Up;
            }
            var d = AsDirect(e.Delta);

            vm.CmdNext.Execute(d);

            _lock = true;
            Task.Delay(900).ContinueWith(_ => _lock = false).ConfigureAwait(false);
        }
    }


    public IEnumerable Cells
    {
        get => GetValue(CellsProperty);
        set => SetValue(CellsProperty, value);
    }

    public byte Range
    {
        get => GetValue(RangeProperty);
        set => SetValue(RangeProperty, value);
    }
}