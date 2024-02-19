using System.Collections;
using UnityEngine;

public abstract class BJPlayer : MonoBehaviour
{
    public abstract void StartMove(BJGameManager manager);
    public abstract void EndMove();
    public abstract void TrumpChoose();
}