using System.Collections.Generic;

namespace game2048;

public class Config
{
    public byte AppendCount { get; set; }
    public byte Range { get; set; }
}


public class Colors<TColor> : Dictionary<uint, TColor> 
{
    public TColor Default { get; set; }

    
    public TColor GetColor(uint value)
    {
        return TryGetValue(value, out var c)
            ? c
            : Default;
    }
}
