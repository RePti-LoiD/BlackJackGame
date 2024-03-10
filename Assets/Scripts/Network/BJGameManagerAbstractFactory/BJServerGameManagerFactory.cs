using System.Net.Sockets;
using UnityEngine;

public class BJServerGameManagerFactory : BJGameManagerFactory
{
    public override (BJGameManager, BJPlayer) CreateManager(GameObject targetGameObject, GameObject targetPlayerObject, BJGameLoadData data)
    {
        TcpListener listener = new TcpListener(data.EndPoint);
        listener.Start();
        TcpClient client = listener.AcceptTcpClient();

        var bJServerGameManager = targetGameObject.AddComponent<BJServerGameManager>();
        bJServerGameManager.dataStream = client.GetStream();
        bJServerGameManager.localPlayer = data.BJLocalUser;
        bJServerGameManager.enemyPlayer = data.BJExternalUser;
        bJServerGameManager.cardManager = data.BJCardManager;

        var bJMinimalPlayer = targetPlayerObject.AddComponent<BJMinimalPlayer>();

        return (bJServerGameManager, bJMinimalPlayer);
    }
}