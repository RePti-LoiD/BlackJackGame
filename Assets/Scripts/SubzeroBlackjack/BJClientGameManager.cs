using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class BJClientGameManager : BJNetworkGameManager
{
    private async void Start()
    {
        tcpClient = new TcpClient();

        await tcpClient.ConnectAsync(IPAddress.Loopback, 8888);
        print($"Connected to: {tcpClient.Client.RemoteEndPoint}");

        dataStream = tcpClient.GetStream();

        _ = ListenServer(dataStream);
    }

    private async Task ListenServer(NetworkStream stream)
    {
        while (true)
        {
            byte[] message = new byte[128];
            await stream.ReadAsync(message, 0, 128);

            HandleNetworkMessage(message);
        }
    }

    public override void GameEnd()
    {

    }

    public override async void PlayerStep(BJPlayer sender, BJStepState stepState)
    {
        if (currentPlayer != null && sender != currentPlayer) 
            return;

        //“”“ Õ¿ƒŒ ¡”ƒ≈“ »Õ¬≈–“»–Œ¬¿“‹ Õ¿ localPlayer
        if (sender == localPlayer)
        {
            print("local player");
        }
        await dataStream.WriteAsync(FromObjectToByteArray(new { StepState = $"{localPlayer.UserData.Id}/{stepState}" }));

        currentPlayer = localPlayer == currentPlayer ? enemyPlayer : localPlayer;
        print(currentPlayer);
    }

    protected override void HandleNetworkMessage(byte[] message)
    {
        string mes = Encoding.UTF8.GetString(message);
        //print(mes);
        var dataProperty = JObject.Parse(mes).Properties().ToArray()[0];
        string[] responce = dataProperty.Value.ToString().Split("/");

        Guid guid = Guid.Parse(responce[1]);

        switch (responce[0])
        {
            case "PlayerID":
                enemyPlayer.UserData.Id = Guid.Parse(responce[1]);
                break;

            case "StepState":
                PlayerStep(GetPlayerByGuid(guid), (BJStepState) 0);
                break;

            case "SetCard":
                print(mes);
                SetCardToHandler(GetPlayerByGuid(guid), cardManager.GetCard(int.Parse(responce[2])));
                break;

            case "StartStep":
                GetPlayerByGuid(Guid.Parse(responce[1])).StartMove(this);

                break;

            case "EndStep":
                GetPlayerByGuid(Guid.Parse(responce[1])).EndMove();

                break;

            case "GameEnd":
                print($"Winner: {responce[1]}");
                print($"{responce[0]}/{responce[1]}");

                break;

            case "UseTrump":

                break;

            default:
                print("Sent unsupported instruction");
                break;
        }
    }

    private BJPlayer GetPlayerByGuid(Guid id)
    {
        if (localPlayer.UserData.Id == id)
        {
            print(localPlayer);
            return localPlayer;
        }

        else if (enemyPlayer.UserData.Id == id)
        {
            print(enemyPlayer);
            return enemyPlayer;
        }

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