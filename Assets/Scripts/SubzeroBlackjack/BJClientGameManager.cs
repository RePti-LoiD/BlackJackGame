using System;

public class BJClientGameManager : BJGameManager
{
    protected void Awake()
    {
        BindMethods();
    }

    protected override void Start() { }

    public override void StartGame()
    {
        ListenNetworkStream();
    }

    protected void BindMethods()
    {
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
        if (currentPlayer != null && sender != currentPlayer) 
            return;

        if (sender == localPlayer)
            SendNetworkMessage(new("StepState", sender.UserData.Id.ToString(), "StepState", new() { stepState.ToString() }));

        sender.EndMove();

        currentPlayer = localPlayer == currentPlayer ? enemyPlayer : localPlayer;
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

    protected override void SetCardToHandler(BJPlayer player, BlackjackCard card)
    {
        player.CardHandler.SetCard(card);
    }
}