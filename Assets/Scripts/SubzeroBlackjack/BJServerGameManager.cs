using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class BJServerGameManager : BJNetworkGameManager, IDisposable
{
    private TcpListener tcpListener;

    private async void Start()
    {
        tcpListener = new TcpListener(IPAddress.Loopback, 8888);
        tcpListener.Start();

        tcpClient = await tcpListener.AcceptTcpClientAsync();
        print($"Connected : {tcpClient.Client.RemoteEndPoint}");

        dataStream = tcpClient.GetStream();
        _ = ListenClient();
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

    protected async Task SendStartStep(BJPlayer player) => 
        await dataStream.WriteAsync(FromObjectToByteArray(new { StartStep = $"StartStep/{player.UserData.Id}" }));

    protected async Task SendEndStep(BJPlayer player) => 
        await dataStream.WriteAsync(FromObjectToByteArray(new { EndStep = $"EndStep/{player.UserData.Id}" }));

    protected async Task ListenClient()
    {
        while (true)
        {
            byte[] buffer = new byte[128];
            await dataStream.ReadAsync(buffer);

            HandleNetworkMessage(buffer);
        }
    }

    protected override void HandleNetworkMessage(byte[] message)
    {
        string mes = Encoding.UTF8.GetString(message);
        print(mes);
        var dataProperty = JObject.Parse(mes).Properties().ToArray()[0];
        string[] responce = dataProperty.Value.ToString().Split("/");

        switch (responce[0])
        {
            case "StepState":
                print("StepState");
                
                PlayerStep(GetPlayerByGuid(Guid.Parse(responce[1])), (BJStepState)Enum.Parse(typeof(BJStepState), responce[2]));
                
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