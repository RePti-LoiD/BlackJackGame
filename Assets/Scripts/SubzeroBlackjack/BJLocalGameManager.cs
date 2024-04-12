using System.Collections.Generic;
using System.Threading.Tasks;

public class BJLocalGameManager : BJGameManager
{
    protected override void Start() { }

    public override void StartGame()
    {
        currentPlayer = localPlayer;

        SetUpCards(new()
        {
            { localPlayer, new BJCard[] { cardManager.GetCard(), cardManager.GetCard() } },
            { enemyPlayer, new BJCard[] { cardManager.GetCard(), cardManager.GetCard() } }
        });

        localPlayer.StartMove(this);
    }

    private async void SetUpCards(Dictionary<BJPlayer, BJCard[]> playerCards, int delay = 500)
    {
        foreach (var player in playerCards)
            foreach (var card in player.Value)
            {
                SetCardToHandler(player.Key, card);
                await Task.Delay(delay);
            }
    }

    public override void PlayerStep(BJPlayer sender, BJStepState stepState)
    {
        if (sender != currentPlayer) return;
        
        print(sender.UserData);
        
        InvokeHandlers(new BJRequestData("StepState", sender.UserData.Id.ToString(), "StepState", new() { stepState.ToString() }));
        
        if (cardManager.IsStackEmpty)
        {
            sender.EndMove();

            GameEnd();
            return;
        }
        
        if (stepState == BJStepState.Pass && lastStepData == BJStepState.Pass)
        {
            sender.EndMove();

            GameEnd();
            return;
        }

        if (stepState == BJStepState.GetCard)
            SetCardToHandler(sender);

        print($"{sender} -> {stepState}");

        print(stepState == lastStepData);
        lastStepData = stepState;

        sender.EndMove();
        
        currentPlayer = localPlayer == currentPlayer ? enemyPlayer : localPlayer;
        currentPlayer.StartMove(this);
    }

    public override void GameEnd()
    {
        base.GameEnd();

        InvokeHandlers(new BJRequestData("OnGameEnd", currentWinner.UserData.Id.ToString(), "Win", new ()));
    }

    public override void SetCardToHandler(BJPlayer player)
    {
        SetCardToHandler(player, cardManager.GetCard());
    }

    protected override void SetCardToHandler(BJPlayer player, BJCard card)
    {
        player.CardHandler.SetCard(card);
    }

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        throw new System.NotImplementedException();
    }
}