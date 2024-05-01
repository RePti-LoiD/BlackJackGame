using System;

public class BJClientGameManager : BJGameManager
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

    public override void StartGame()
    {

    }

    protected void BindMethods()
    {
        AddNetworkMessageListener("OnBet", ReceiveBet);
        AddNetworkMessageListener("OnBetFinish", OnBetFinish);
        AddNetworkMessageListener("StepState", StepStateNetworkMethod);
        AddNetworkMessageListener("StartStep", StartStepNetworkMethod);
        AddNetworkMessageListener("SetCard", SetCardNetworkMethod);
        AddNetworkMessageListener("EndMove", EndMoveNetworkMethod);
    }


    private void StartStepNetworkMethod(BJRequestData data)
    {
        if (data.UserSenderId == localPlayer.UserData.Id.ToString())
        {
            currentPlayer = localPlayer;
            localPlayer.StartMove(this);
        }
        else
        {
            currentPlayer = enemyPlayer;
        }
    }

    private void StepStateNetworkMethod(BJRequestData data)
    {
        BJStepState stepState = (BJStepState)Enum.Parse(typeof(BJStepState), data.Args[0]);

        PlayerStep(GetPlayerByGuid(data.UserSenderId), stepState);
    }
    
    private void SetCardNetworkMethod(BJRequestData data)
    {
        SetCardToHandler(GetPlayerByGuid(data.UserSenderId), cardManager.GetCard(int.Parse(data.Args[0])));
    }
    
    private void EndMoveNetworkMethod(BJRequestData data)
    {
        GetPlayerByGuid(data.UserSenderId).EndMove();
    }
    

    public override void PlayerStep(BJPlayer sender, BJStepState stepState)
    {
        print(currentPlayer);

        if (currentPlayer != null && sender != currentPlayer) 
            return;

        if (sender == localPlayer)
            SendNetworkMessage(new("StepState", sender.UserData.Id.ToString(), "StepState", new() { stepState.ToString() }));
        
        print((sender, stepState));
        sender.EndMove();

        currentPlayer = localPlayer == currentPlayer ? enemyPlayer : localPlayer;
    }

    public override void OnBet(int bet, int previousBet)
    {
        SendNetworkMessage(new("OnBet", UserDataWrapper.UserData.Id.ToString(), "OnBet", new() { bet.ToString() }));
    }

    private void ReceiveBet(BJRequestData data)
    {
        Bet.CurrentBet = int.Parse(data.Args[0]);
    }

    public void OnBetFinish(BJRequestData data)
    {
        localPlayer.PlayerBet = Bet.CurrentBet;
        enemyPlayer.PlayerBet = Bet.CurrentBet;

        foreach (var item in FindObjectsByType<BJPlayerDataVizualization>(UnityEngine.FindObjectsSortMode.None))
            item.UpdateData();

        Bet.gameObject.SetActive(false);

        StartGame();
    }

    public override void GameEnd()
    {

    }

    private BJPlayer GetPlayerByGuid(string id)
    {
        id = id.Trim();

        if (localPlayer.UserData.Id.ToString() == id)
            return localPlayer;

        else if (enemyPlayer.UserData.Id.ToString() == id)
            return enemyPlayer;

        return null;
    }
    
    public override void SetCardToHandler(BJPlayer player)
    {
        SetCardToHandler(player, cardManager.GetCard());
    }

    protected override void SetCardToHandler(BJPlayer player, BJCard card)
    {
        player.CardHandler.SetCard(card);
    }
}