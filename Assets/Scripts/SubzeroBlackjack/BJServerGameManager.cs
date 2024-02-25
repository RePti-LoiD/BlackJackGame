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
            byte[] buffer = new byte[1024];
            await dataStream.ReadAsync(buffer);

            HandleNetworkMessage(Encoding.UTF8.GetString(buffer));
        }
    }

    protected override void HandleNetworkMessage(string message)
    {
        JProperty messageObject = JObject.Parse(message).Properties().ToList()[0];

        switch (messageObject.Name)
        {
            case "StepState":
                string[] value = messageObject.Value<string>().Split("/");
                Guid id = Guid.Parse(value[0]);
                BJStepState state = (BJStepState) Enum.Parse(typeof(BJStepState), value[1]);
                
                if (localPlayer.UserData.Id == id)
                    PlayerStep(localPlayer, state);
                else
                    PlayerStep(enemyPlayer, state);

                break;

            default:
                break;
        }
    }

    public override void GameEnd()
    {
        IsGameEnd = true;
        BJPlayer winner;
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
        print($"{sender} -> {stepState}");

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
        print(player);
        BlackjackCard blackjackCard = card;
        player.CardHandler.SetCard(blackjackCard);

        dataStream.WriteAsync(FromObjectToByteArray(new {SetCard = $"SetCard/{player.UserData.Id}/{blackjackCard.CardData.CardWeight}" }));
    }
}