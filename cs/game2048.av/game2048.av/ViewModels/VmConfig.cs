using System.Reactive.Linq;
using System.Reactive.Subjects;
using game2048.av.Models;
using ReactiveUI;

namespace game2048.av.ViewModels;

public class VmConfig : VmBase
{
    public byte[] AppendCounts { get; } = { 1, 2, 3, 4, 5, 6, 7 };

    private byte _appendCount = 2;
    public byte AppendCount
    {
        get => _appendCount;
        set => this.RaiseAndSetIfChanged(ref _appendCount, value);
    }
    
    public byte[] Ranges { get; } = { 3, 4, 5, 6, 7 };

    private byte _range = 4;
    public byte Range
    {
        get => _range;
        set => this.RaiseAndSetIfChanged(ref _range, value);
    }
    
    // https://habr.com/ru/companies/nix/articles/261031/
    public BehaviorSubject<ConfigAv> Config { get; } = new (new ConfigAv());
    // public IObservable<ConfigAv> Config { get; }

    public VmConfig()
    {
        this.WhenAnyValue(vm => vm.AppendCount, vm => vm.Range)
            .Select(vm => new ConfigAv() { AppendCount = vm.Item1, Range = vm.Item2 })
            .Subscribe(Config);
    }
}