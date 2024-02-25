using System;
using System.Collections;
using UnityEngine;

public class BJBotPlayer : BJPlayer
{
    [SerializeField] private CardStackHandler cardStackHandler;

    public override void StartMove(BJGameManager manager)
    {
        StartCoroutine(
            DoAfterDelay(2, () => 
                manager.PlayerStep(this, (BJStepState)UnityEngine.Random.Range(0, 2))));
    }

    private IEnumerator DoAfterDelay(int sec, Action action)
    {
        yield return new WaitForSeconds(sec);
        action();
    }

    public override void EndMove()
    {

    }

    public override void TrumpChoose()
    {
        throw new System.NotImplementedException();
    }
}