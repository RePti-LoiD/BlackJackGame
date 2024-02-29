using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class BJClientGameManager : BJGameManager
{
    [SerializeField] private TestServerInvoker invoker;

    protected async override Task NetworkInitialization()
    {
        tcpClient = new TcpClient();

        await tcpClient.ConnectAsync(IPAddress.Loopback, 8888);
        print($"Connected to: {tcpClient.Client.RemoteEndPoint}");

        SendNetworkMessage(new BJRequestData()
        {
            Header = "SetUp",
            State = "PlayerID",
            UserSenderId = localPlayer.UserData.Id.ToString(),
            Args = new() { localPlayer.UserData.Id.ToString() }
        });
    }

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        switch (data.Header)
        {
            case "SetUp":
                enemyPlayer.UserData.Id = Guid.Parse(data.Args[0]);
                invoker.ShowGuid();
                break;

            case "StepState":
                PlayerStep(GetPlayerByGuid(Guid.Parse(data.UserSenderId)), (BJStepState)0);
                break;

            case "SetCard":
                SetCardToHandler(GetPlayerByGuid(Guid.Parse(data.UserSenderId)), cardManager.GetCard(int.Parse(data.Args[0])));
                break;

            case "StartStep":
                if (data.UserSenderId == enemyPlayer.UserData.Id.ToString())
                {
                    currentPlayer = enemyPlayer;
                    enemyPlayer.StartMove(this);
                }
                else
                {
                    currentPlayer = localPlayer;
                }

                break;

            case "EndStep":
                GetPlayerByGuid(Guid.Parse(data.UserSenderId)).EndMove();
                break;

            case "GameEnd":
                print($"Winner: {data.UserSenderId}");
                break;

            case "UseTrump":
                break;

            default:
                print("Sent unsupported instruction");
                break;
        }
    }

    public override void PlayerStep(BJPlayer sender, BJStepState stepState)
    {
        if (currentPlayer != null && sender != currentPlayer) 
            return;

        //TODO: “”“ Õ¿ƒŒ ¡”ƒ≈“ »Õ¬≈–“»–Œ¬¿“‹ Õ¿ localPlayer
        if (sender == enemyPlayer)
            SendNetworkMessage(new("StepState", sender.UserData.Id.ToString(), "StepState", new() { stepState.ToString() }));

        currentPlayer = localPlayer == currentPlayer ? enemyPlayer : localPlayer;
        print(currentPlayer);
    }

    public override void GameEnd()
    {

    }

    private BJPlayer GetPlayerByGuid(Guid id)
    {
        if (localPlayer.UserData.Id == id)
            return localPlayer;

        else if (enemyPlayer.UserData.Id == id)
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