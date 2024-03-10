using System.Net.Sockets;
using UnityEngine;

public class BJClientGameManagerFactory : BJGameManagerFactory
{
    public override (BJGameManager, BJPlayer) CreateManager(GameObject targetGameObject, GameObject targetPlayerObject, BJGameLoadData data)
    {
        TcpClient client = new TcpClient();
        client.Connect(data.EndPoint);

        var bJClientGameManager = targetGameObject.AddComponent<BJClientGameManager>();
        bJClientGameManager.dataStream = client.GetStream();
        bJClientGameManager.localPlayer = data.BJLocalUser;
        bJClientGameManager.enemyPlayer = data.BJExternalUser;
        bJClientGameManager.cardManager = data.BJCardManager;

        var bJMinimalPlayer = targetPlayerObject.AddComponent<BJMinimalPlayer>();


        return (bJClientGameManager, bJMinimalPlayer);
    }
}