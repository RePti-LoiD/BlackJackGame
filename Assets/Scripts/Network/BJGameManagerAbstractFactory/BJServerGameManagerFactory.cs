using System.Net.Sockets;
using System.Threading.Tasks;
public class BJServerGameManagerFactory : BJGameManagerFactory
{
    public override async Task<(BJGameManager, BJPlayer)> CreateManagerAsync(BJGameLoadData data)
    {
        TcpListener listener = new TcpListener(data.EndPoint);
        listener.Start();


        BJServerGameManager bJServerGameManager = data.GameManagerObject.AddComponent<BJServerGameManager>();
        bJServerGameManager.dataStream = (await listener.AcceptTcpClientAsync()).GetStream();
        UnityEngine.Debug.Log("connected");

        BJMinimalPlayer bJMinimalPlayer = data.BJExternalPlayerGameObject.AddComponent<BJMinimalPlayer>();
        bJMinimalPlayer.CardHandler = data.CardHandlerExternalPlayer;

        bJServerGameManager.localPlayer = data.BJLocalUser;
        bJServerGameManager.localPlayer.UserData = data.LocalUser;

        bJServerGameManager.enemyPlayer = bJMinimalPlayer;
        bJMinimalPlayer.UserData = data.ExternalUser;

        bJServerGameManager.cardManager = data.BJCardManager;

        return (bJServerGameManager, bJMinimalPlayer);
    }
}