using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class BJServerGameManager : BJNetworkGameManager, IDisposable
{
    private TcpListener tcpListener;

    protected override async void Start()
    {
        base.Start();

        await dataStream.WriteAsync(FromObjectToByteArray(new { SetUp = $"PlayerID/{enemyPlayer.UserData.Id}" }));

        SetCardToHandler(localPlayer);
        SetCardToHandler(localPlayer);
        SetCardToHandler(enemyPlayer);
        SetCardToHandler(enemyPlayer);

        localPlayer.OnStartMove += async (p) => await SendStartStep(p);
        localPlayer.OnEndMove += async (p) => await SendEndStep(p);
        localPlayer.OnTrumpChoose += async (p) => await SendStartStep(p);

        enemyPlayer.OnStartMove += async (p) => await SendStartStep(p);
        enemyPlayer.OnEndMove += async (p) => await SendEndStep(p);
        enemyPlayer.OnTrumpChoose += async (p) => await SendStartStep(p);

        localPlayer.StartMove(this);
        currentPlayer = localPlayer;
    }

    protected async override Task NetworkInitialization()
    {
        tcpListener = new TcpListener(IPAddress.Loopback, 8888);
        tcpListener.Start();

        tcpClient = await tcpListener.AcceptTcpClientAsync();
        print($"Connected : {tcpClient.Client.RemoteEndPoint}");
    }

    protected async Task SendStartStep(BJPlayer player) => 
        await dataStream.WriteAsync(FromObjectToByteArray(new { StartStep = $"StartStep/{player.UserData.Id}" }));

    protected async Task SendEndStep(BJPlayer player) => 
        await dataStream.WriteAsync(FromObjectToByteArray(new { EndStep = $"EndStep/{player.UserData.Id}" }));

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        switch (data.Header)
        {
            case "StepState":
                print("StepState");
                
                PlayerStep(GetPlayerByGuid(Guid.Parse(data.UserSenderId)), (BJStepState)Enum.Parse(typeof(BJStepState), data.Args[0]));
                
                print("StepState post");
                break;

            default:
                break;
        }
    }

    public override void GameEnd()
    {
        IsGameEnd = true;
        int localPlayerWeight = localPlayer.CardHandler.GetTotalCardWeight(), enemyPlayerWeight = enemyPlayer.CardHandler.GetTotalCardWeight();

        if (localPlayerWeight == enemyPlayerWeight)
            dataStream.WriteAsync(FromObjectToByteArray(new { GameEnd = $"GameEnd/Draw" }));
        else if (localPlayerWeight > enemyPlayerWeight)
            dataStream.WriteAsync(FromObjectToByteArray(new { GameEnd = $"GameEnd/{localPlayer.UserData.Id}/Win" }));
        else
            dataStream.WriteAsync(FromObjectToByteArray(new { GameEnd = $"GameEnd/{enemyPlayer.UserData.Id}/Win" }));
    }

    public override async void PlayerStep(BJPlayer sender, BJStepState stepState)
    {
        if (sender != currentPlayer) return;

        if (sender == localPlayer)
            await dataStream.WriteAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { StepState = $"StepState/{sender.UserData.Id}/{stepState}" })));

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
        {
            SetCardToHandler(sender);
            //Transpor layer 
        }

        lastStepData = stepState;

        sender.EndMove();

        currentPlayer = localPlayer == currentPlayer ? enemyPlayer : localPlayer;
        currentPlayer.StartMove(this);
    }

    public override void SetCardToHandler(BJPlayer player)
    {
        SetCardToHandler(player, cardManager.GetCard());
    }

    public override void Dispose()
    {
        base.Dispose();

        tcpListener?.Stop();
    }

    protected override void SetCardToHandler(BJPlayer player, BlackjackCard card)
    {
        BlackjackCard blackjackCard = card;
        player.CardHandler.SetCard(blackjackCard);

        dataStream.WriteAsync(FromObjectToByteArray(new {SetCard = $"SetCard/{player.UserData.Id}/{blackjackCard.CardData.CardWeight}" }));
    }

    protected BJPlayer GetPlayerByGuid(Guid guid)
    {
        if (localPlayer.UserData.Id == guid)
            return localPlayer;
        else if (enemyPlayer.UserData.Id == guid) 
            return enemyPlayer;
        
        return null;
    }
}