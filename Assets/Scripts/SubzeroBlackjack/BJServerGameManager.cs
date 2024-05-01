using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BJServerGameManager : BJGameManager, IDisposable
{
    //TODO: разобраться с проблемой того, что сервер не видит сообщения клиента. Вернуть в старт ListenNetworkStream()

    protected override void Start()
    {
        ListenNetworkStream();
    }

    protected void Awake() 
    {
        BindMethods();
    }
    
    protected void BindMethods()
    {
        AddNetworkMessageListener("StepState", (data) =>
            PlayerStep(GetPlayerByGuid(data.UserSenderId), (BJStepState)Enum.Parse(typeof(BJStepState), data.Args[0])));

        AddNetworkMessageListener("OnBet", ReceiveBet);
    }

    public override void StartGame()
    {
        PostNetworkInitialization();
    }

    protected override void PostNetworkInitialization()
    {
        SetUpCards(new()
        {
            { localPlayer, new BJCard[] { cardManager.GetCard(), cardManager.GetCard() } },
            { enemyPlayer, new BJCard[] { cardManager.GetCard(), cardManager.GetCard() } }
        });

        localPlayer.OnStartMove += (p) => SendStartStep(p);
        localPlayer.OnEndMove += (p) => SendEndStep(p);
        localPlayer.OnTrumpChoose += (p) => SendStartStep(p);

        enemyPlayer.OnStartMove += (p) => SendStartStep(p);
        enemyPlayer.OnEndMove += (p) => SendEndStep(p);
        enemyPlayer.OnTrumpChoose += (p) => SendStartStep(p);

        localPlayer.StartMove(this);
        currentPlayer = localPlayer;
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

    protected void SendStartStep(BJPlayer player) =>
        SendNetworkMessage(new("StartStep", player.UserData.Id.ToString(), "StartStep", new() { "" }));

    protected void SendEndStep(BJPlayer player) =>
        SendNetworkMessage(new("EndStep", player.UserData.Id.ToString(), "EndStep", new() { "" }));

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        switch (data.Header)
        {
            case "StepState":
                PlayerStep(GetPlayerByGuid(data.UserSenderId), (BJStepState)Enum.Parse(typeof(BJStepState), data.Args[0]));
                break;

            default:
                break;
        }
    }
    public override void GameEnd()
    {
        IsGameEnd = true;
        int localPlayerWeight = localPlayer.CardHandler.GetTotalCardWeight(), 
            enemyPlayerWeight = enemyPlayer.CardHandler.GetTotalCardWeight();

        if (localPlayerWeight > enemyPlayerWeight)
            SendNetworkMessage(new("GameEnd", localPlayer.UserData.Id.ToString(), "GameEnd", new() { "Win" }));
        else if (localPlayerWeight < enemyPlayerWeight)
            SendNetworkMessage(new("GameEnd", enemyPlayer.UserData.Id.ToString(), "GameEnd", new() { "Win" }));
        else
            SendNetworkMessage(new("GameEnd", localPlayer.UserData.Id.ToString(), "GameEnd", new() { "Draw" }));
    }

    public override void PlayerStep(BJPlayer sender, BJStepState stepState)
    {
        print((sender, currentPlayer));

        if (sender != currentPlayer) return;

        if (sender == localPlayer)
            SendNetworkMessage(new("StepState", localPlayer.UserData.Id.ToString(), "StepState", new() { stepState.ToString() }));

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
            sender.EndMove();
            GameEnd();
            return;
        }

        if (stepState == BJStepState.GetCard)
            SetCardToHandler(sender);
        
            
        lastStepData = stepState;

        sender.EndMove();

        currentPlayer = localPlayer == currentPlayer ? enemyPlayer : localPlayer;
        currentPlayer.StartMove(this);
    }

    public override void OnBet(int bet, int previousBet)
    {
        SendNetworkMessage(new BJRequestData(nameof(OnBet), UserDataWrapper.UserData.Id.ToString(), nameof(OnBet), new() { bet.ToString() }));
    }

    private void ReceiveBet(BJRequestData data)
    {
        Bet.CurrentBet = int.Parse(data.Args[0]);
    }

    public void OnBetFinish()
    {
        SendNetworkMessage(new("OnBetFinish", UserDataWrapper.UserData.Id.ToString(), "OnBetFinish", new() { }));
    }

    public override void SetCardToHandler(BJPlayer player)
    {
        SetCardToHandler(player, cardManager.GetCard());
    }

    protected override void SetCardToHandler(BJPlayer player, BJCard card)
    {
        BJCard blackjackCard = card;
        player.CardHandler.SetCard(blackjackCard);

        SendNetworkMessage(new("SetCard", player.UserData.Id.ToString(), "SetCard", new() { blackjackCard.CardData.CardWeight.ToString() }));
    }

    protected BJPlayer GetPlayerByGuid(string guid)
    {
        if (localPlayer.UserData.Id.ToString() == guid)
            return localPlayer;
        else if (enemyPlayer.UserData.Id.ToString() == guid) 
            return enemyPlayer;
        
        return null;
    }

    public override void Dispose()
    {
        base.Dispose();

        dataStream?.Close();
    }
}