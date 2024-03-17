using System.Net.Sockets;
using System.Threading.Tasks;

public class BJClientGameManagerFactory : BJGameManagerFactory
{
    public override async Task<(BJGameManager, BJPlayer)> CreateManagerAsync(BJGameLoadData data)
    {
        TcpClient client = new();
        await client.ConnectAsync(data.EndPoint.Address, data.EndPoint.Port);
        UnityEngine.Debug.Log("connected");

        BJClientGameManager bJClientGameManager = data.GameManagerObject.AddComponent<BJClientGameManager>();
        
        BJMinimalPlayer bJMinimalPlayer = data.BJExternalPlayerGameObject.AddComponent<BJMinimalPlayer>();
        bJMinimalPlayer.CardHandler = data.CardHandlerExternalPlayer;

        bJClientGameManager.dataStream = client.GetStream();

        bJClientGameManager.localPlayer = data.BJLocalUser;
        bJClientGameManager.localPlayer.UserData = data.LocalUser;

        bJClientGameManager.enemyPlayer = bJMinimalPlayer;
        bJMinimalPlayer.UserData = data.ExternalUser;

        bJClientGameManager.cardManager = data.BJCardManager;

        return (bJClientGameManager, bJMinimalPlayer);
    }
}