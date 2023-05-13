using System;
using System.Globalization;
using System.Reactive.Subjects;
using System.Windows.Input;

using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;

using game2048.av.Models;

using ReactiveUI;

namespace game2048.av.ViewModels;

public class VmMain : VmBase
{
    private VmConfig VmConfig { get; }
    public VmGame VmGame { get; }

    public BehaviorSubject<VmBase> Content { get; }

    public VmMenuItem[] Cmds { get; }

    public VmMain()
    {
        CmdOption = ReactiveCommand.Create(OnOption);

        VmConfig = new VmConfig();
        VmGame = new(VmConfig.Config);

        Content = new BehaviorSubject<VmBase>(VmGame);

        Cmds = new[]
        {
            new VmMenuItem(CmdOption, "pg_Settings", "Options"),
            new VmMenuItem(VmGame.CmdReset, "pg_Refresh", "Reset"){HotKey = new KeyGesture(Key.F5)},
            new VmMenuItem(VmGame.CmdUndo, "pg_Undo", "Undo"){ HotKey = new KeyGesture(Key.Z, KeyModifiers.Control)},
        };
    }

    #region actions

    public ICommand CmdOption { get; }

    private void OnOption()
    {
        // Content.OnNext(isVisible == true ? VmConfig : VmGame);
        Content.OnNext(Content.Value == VmGame ? VmConfig : VmGame);
    }

    #endregion
}

public class ConvertToDefine : IValueConverter
{
    public static readonly ConvertToDefine Instance = new ConvertToDefine();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string val && parameter is string par)
        {
            switch (par.ToLower())
            {
                case "col": return ColumnDefinitions.Parse(val);
                case "row": return RowDefinitions.Parse(val);
            }
        }

        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}