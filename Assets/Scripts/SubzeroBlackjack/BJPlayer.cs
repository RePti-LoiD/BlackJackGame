using System;
using UnityEngine;

public abstract class BJPlayer : MonoBehaviour, IBJPlayerCallbacks
{
    [SerializeField] public CardStackHandler CardHandler;

    [HideInInspector] public int PlayerBet; 

    public User UserData;
    public PlayerStatus PlayerStatus;

    public Action<BJPlayer> OnStartMove { get; set; }
    public Action<BJPlayer> OnEndMove { get; set; }
    public Action<BJPlayer> OnTrumpChoose { get; set; }

    public abstract void StartMove(BJGameManager manager);
    public abstract void EndMove();
    public abstract void TrumpChoose();
}

public enum PlayerStatus
{
    Default,
    Host,
    Client
}