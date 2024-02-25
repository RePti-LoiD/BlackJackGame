using System;

public interface IBJPlayerCallbacks
{
    public Action<BJPlayer> OnStartMove { get; set; }
    public Action<BJPlayer> OnEndMove { get; set; }
    public Action<BJPlayer> OnTrumpChoose { get; set; }
}