using System;
using System.Collections;
using UnityEngine;

public class BotBJPlayer : BJPlayer
{
    public override void StartMove(BJGameManager manager)
    {
        StartCoroutine(
            DoAfterDelay(2, () => 
                manager.PlayerStep(this, UnityEngine.Random.Range(0, 2) == 1 ? true : false)));
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