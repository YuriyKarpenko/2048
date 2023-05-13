using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

using Avalonia.Input;

using game2048.av.Models;

using ReactiveUI;

using Splat;

namespace game2048.av.ViewModels;

public class VmGame : VmBase
{
    private readonly Stack<Dictionary<Point, uint>> _history = new();
    private readonly BehaviorSubject<bool> _canHistory = new(false);

    // https://habr.com/ru/companies/nix/articles/261031/
    private readonly BehaviorSubject<ConfigAv> _config;
    private readonly BehaviorSubject<Game> _game;
    public SubjectBase<VmCell[]> Cells { get; } = new BehaviorSubject<VmCell[]>(default);

    public VmGame(IObservable<ConfigAv> config)
    {
        CmdReset = ReactiveCommand.Create(OnReset);
        CmdUndo = ReactiveCommand.Create<Direct>(OnUndo, _canHistory);
        CmdNext = ReactiveCommand.Create<Direct>(OnNext);
        CmdNextDown = new MCommandIcon(CmdNext, "pg_Arrowhead_down2") { CommandParameter = Direct.Down, HotKey = new KeyGesture(Key.Down)};
        CmdNextLeft = new MCommandIcon(CmdNext, "pg_Arrowhead_left2") { CommandParameter = Direct.Left, HotKey = new KeyGesture(Key.Left) };
        CmdNextRight = new MCommandIcon(CmdNext, "pg_Arrowhead_right2") { CommandParameter = Direct.Right, HotKey = new KeyGesture(Key.Right) };
        CmdNextUp = new MCommandIcon(CmdNext, "pg_Arrowhead_up2") { CommandParameter = Direct.Up, HotKey = new KeyGesture(Key.Up) };
        _config = new BehaviorSubject<ConfigAv>(default);
        config.Subscribe(_config);

        _game = new(new Game(_config.Value));
        //  при изменении настроек пересоздаём игру
        _config.Select(i => new Game(i)).Subscribe(_game);

        //  при создании игры обновляем ячейки
        _game.Subscribe(OnNext);
    }

    private void OnNext(Game game)
    {
        Cells.OnNext(ToCells(game));
        this.RaisePropertyChanged(nameof(Cells));
    }

    #region actions

    public ICommand CmdReset { get; }
    public ICommand CmdUndo { get; }
    public MCommand CmdNextDown { get; }
    public MCommand CmdNextUp { get; }
    public MCommand CmdNextLeft { get; }
    public MCommand CmdNextRight { get; }
    public ICommand CmdNext { get; }

    private void OnReset()
    {
        _game.Value.Reset();
        OnNext(_game.Value);
        _history.Clear();
        _canHistory.OnNext(false);
    }

    private void OnNext(Direct direct)
    {
        _history.Push(_game.Value.GameGrid.Clone());
        _canHistory.OnNext(true);
        _game.Value.Next(direct);
        OnNext(_game.Value);
    }

    private void OnUndo(Direct direct)
    {
        bool TryPop<T>(Stack<T> src, out T? value)
        {
            try
            {
                if (src.Count > 0)
                {
                    value = src.Pop();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                this.Log().Error(ex, $"{nameof(VmGame)}.{nameof(OnUndo)}()");
            }

            value = default;
            return false;
        }
        if (TryPop(_history, out var grid))
        {
            _game.Value.GameGrid.CopyValues(grid);
            _canHistory.OnNext(_history.Count > 0);
            OnNext(_game.Value);
        }
    }

    #endregion

    private VmCell[] ToCells(Game game)
        => game.GameGrid.Select(i => new VmCell(i, _config.Value)).ToArray();
}