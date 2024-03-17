public class BJLocalGameManager : BJGameManager
{
    protected override void Start() { }

    public override void StartGame()
    {
        currentPlayer = localPlayer;

        SetCardToHandler(localPlayer);
        SetCardToHandler(localPlayer);
        SetCardToHandler(enemyPlayer);
        SetCardToHandler(enemyPlayer);

        localPlayer.StartMove(this);
    }

    public override void PlayerStep(BJPlayer sender, BJStepState stepState)
    {
        if (sender != currentPlayer) return;
        
        print(sender.UserData);
        
        InvokeHandlers(new BJRequestData("StepState", sender.UserData.Id.ToString(), "StepState", new() { stepState.ToString() }));
        
        if (cardManager.IsStackEmpty)
        {
            GameEnd();
            sender.EndMove();
            return;
        }
        
        if (lastStepData != BJStepState.Default && 
            stepState == BJStepState.Pass && 
            lastStepData == BJStepState.Pass)
        {
            GameEnd();
            sender.EndMove();
            return;
        }

        if (stepState == BJStepState.GetCard)
        {
            SetCardToHandler(sender);
            //Transpor layer 
        }

        lastStepData = stepState;
        print($"{sender} -> {stepState}");


        sender.EndMove();
        
        currentPlayer = localPlayer == currentPlayer ? enemyPlayer : localPlayer;
        currentPlayer.StartMove(this);
    }

    public override void GameEnd()
    {
        IsGameEnd = true;
        print("Game end");
    }

    public override void SetCardToHandler(BJPlayer player)
    {
        SetCardToHandler(player, cardManager.GetCard());
    }

    protected override void SetCardToHandler(BJPlayer player, BlackjackCard card)
    {
        player.CardHandler.SetCard(card);
    }

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        throw new System.NotImplementedException();
    }
}