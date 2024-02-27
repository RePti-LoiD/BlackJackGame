using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class BJServerGameManager : BJNetworkGameManager, IDisposable
{
    private TcpListener tcpListener;

    protected async override Task NetworkInitialization()
    {
        tcpListener = new TcpListener(IPAddress.Loopback, 8888);
        tcpListener.Start();

        tcpClient = await tcpListener.AcceptTcpClientAsync();
        print($"Connected: {tcpClient.Client.RemoteEndPoint}");
    }

    protected override void PostNetworkInitialization()
    {
        SendNetworkMessage(new BJRequestData()
        {
            Header = "SetUp",
            State = "PlayerID",
            UserSenderId = localPlayer.UserData.Id.ToString(),
            Args = new() { enemyPlayer.UserData.Id.ToString() }
        });

        SetCardToHandler(localPlayer);
        SetCardToHandler(localPlayer);
        SetCardToHandler(enemyPlayer);
        SetCardToHandler(enemyPlayer);

        localPlayer.OnStartMove += (p) => SendStartStep(p);
        localPlayer.OnEndMove += (p) => SendEndStep(p);
        localPlayer.OnTrumpChoose += (p) => SendStartStep(p);

        enemyPlayer.OnStartMove += (p) => SendStartStep(p);
        enemyPlayer.OnEndMove += (p) => SendEndStep(p);
        enemyPlayer.OnTrumpChoose += (p) => SendStartStep(p);

        localPlayer.StartMove(this);
        currentPlayer = localPlayer;
    }

    protected void SendStartStep(BJPlayer player) =>
        SendNetworkMessage(new("StartStep", player.UserData.Id.ToString(), "StartStep", new()));

    protected void SendEndStep(BJPlayer player) =>
        SendNetworkMessage(new("EndStep", player.UserData.Id.ToString(), "EndStep", new()));

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        switch (data.Header)
        {
            case "StepState":
                PlayerStep(GetPlayerByGuid(Guid.Parse(data.UserSenderId)), (BJStepState)Enum.Parse(typeof(BJStepState), data.Args[0]));
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

    public override void SetCardToHandler(BJPlayer player)
    {
        SetCardToHandler(player, cardManager.GetCard());
    }


    protected override void SetCardToHandler(BJPlayer player, BlackjackCard card)
    {
        BlackjackCard blackjackCard = card;
        player.CardHandler.SetCard(blackjackCard);

        SendNetworkMessage(new("SetCard", player.UserData.Id.ToString(), "SetCard", new() { blackjackCard.CardData.CardWeight.ToString() }));
    }

    protected BJPlayer GetPlayerByGuid(Guid guid)
    {
        if (localPlayer.UserData.Id == guid)
            return localPlayer;
        else if (enemyPlayer.UserData.Id == guid) 
            return enemyPlayer;
        
        return null;
    }

    public override void Dispose()
    {
        base.Dispose();

        tcpListener?.Stop();
    }
}