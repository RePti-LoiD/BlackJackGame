using UnityEngine;

public class BJGameManager : MonoBehaviour
{
    [SerializeField] private BJPlayer localPlayer;
    [SerializeField] private BJPlayer enemyPlayer;

    private BJPlayer currentPlayer;
    private bool lastStepData;

    private void Start()
    {
        localPlayer.StartMove(this);
    }

    public void PlayerStep(BJPlayer sender, bool data)
    {
        if (sender == currentPlayer)
        {
            if (lastStepData == data)
            {
                print("Game end");
                return;
            }

            lastStepData = data;
            print($"{sender} -> {data}");
        }
        sender.EndMove();

        currentPlayer = localPlayer == currentPlayer ? enemyPlayer : localPlayer;
        currentPlayer.StartMove(this);
    }
}