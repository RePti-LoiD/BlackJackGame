using System;
using UnityEngine;

public abstract class BJPlayer : MonoBehaviour, IBJPlayerCallbacks
{
    [SerializeField] public CardStackHandler CardHandler;
    public User UserData;

    public Action<BJPlayer> OnStartMove { get; set; }
    public Action<BJPlayer> OnEndMove { get; set; }
    public Action<BJPlayer> OnTrumpChoose { get; set; }

    public abstract void StartMove(BJGameManager manager);
    public abstract void EndMove();
    public abstract void TrumpChoose();
}