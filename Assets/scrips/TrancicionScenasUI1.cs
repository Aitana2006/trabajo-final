using System;

public class TrancicionScenasUI
{
    public static object Instance { get; internal set; }

    public static implicit operator TrancicionScenasUI(trancicionScenasui v)
    {
        throw new NotImplementedException();
    }
}