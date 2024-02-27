using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class BJClientGameManager : BJNetworkGameManager
{
    protected async override Task NetworkInitialization()
    {
        tcpClient = new TcpClient();

        await tcpClient.ConnectAsync(IPAddress.Loopback, 8888);
        print($"Connected to: {tcpClient.Client.RemoteEndPoint}");
    }

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        switch (data.Header)
        {
            case "PlayerID":
                enemyPlayer.UserData.Id = Guid.Parse(data.UserSenderId);
                break;

            case "StepState":
                PlayerStep(GetPlayerByGuid(Guid.Parse(data.UserSenderId)), (BJStepState)0);
                break;

            case "SetCard":
                SetCardToHandler(GetPlayerByGuid(Guid.Parse(data.UserSenderId)), cardManager.GetCard(int.Parse(data.Args[0])));
                break;

            case "StartStep":
                GetPlayerByGuid(Guid.Parse(data.UserSenderId)).StartMove(this);

                break;

            case "EndStep":
                GetPlayerByGuid(Guid.Parse(data.UserSenderId)).EndMove();
                break;

            case "GameEnd":
                print($"Winner: {data.UserSenderId}");
                //print($"{responce[0]}/{responce[1]}");

                break;

            case "UseTrump":

                break;

            default:
                print("Sent unsupported instruction");
                break;
        }
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

    public override void GameEnd()
    {

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