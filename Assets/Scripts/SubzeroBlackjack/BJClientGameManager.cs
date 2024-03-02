using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class BJClientGameManager : BJGameManager
{
    [SerializeField] private TestServerInvoker invoker;

    public IPEndPoint ExternalEndPoint;

    protected async override Task NetworkInitialization()
    {
        tcpClient = new TcpClient();

        await tcpClient.ConnectAsync(ExternalEndPoint.Address, ExternalEndPoint.Port);
        print($"Connected to: {tcpClient.Client.RemoteEndPoint}");
    }

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        switch (data.Header)
        {
            case "SetUp":
                invoker.ShowGuid();
                break;

            case "StepState":
                PlayerStep(GetPlayerByGuid(data.UserSenderId), (BJStepState)0);
                break;

            case "SetCard":
                SetCardToHandler(GetPlayerByGuid(data.UserSenderId), cardManager.GetCard(int.Parse(data.Args[0])));
                break;

            case "StartStep":
                if (data.UserSenderId == localPlayer.UserData.Id.ToString())
                {
                    currentPlayer = localPlayer;
                    localPlayer.StartMove(this);
                }
                else
                {
                    currentPlayer = enemyPlayer;
                }

                break;

            case "EndStep":

                GetPlayerByGuid(data.UserSenderId).EndMove();
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
        if (sender == localPlayer)
            SendNetworkMessage(new("StepState", sender.UserData.Id.ToString(), "StepState", new() { stepState.ToString() }));

        currentPlayer = localPlayer == currentPlayer ? enemyPlayer : localPlayer;
        print(currentPlayer);
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