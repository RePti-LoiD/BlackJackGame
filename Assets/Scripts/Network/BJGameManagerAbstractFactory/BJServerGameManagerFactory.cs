using UnityEngine;

public class BJServerGameManagerFactory : BJGameManagerFactory
{
    public override (BJGameManager, BJPlayer) CreateManager(GameObject targetGameObject, GameObject targetPlayerObject, BJGameLoadData data)
    {
        var bJServerGameManager = targetGameObject.AddComponent<BJServerGameManager>();
        bJServerGameManager.dataStream = data.DataStream;
        bJServerGameManager.localPlayer = data.BJLocalUser;
        bJServerGameManager.enemyPlayer = data.BJExternalUser;
        bJServerGameManager.cardManager = data.BJCardManager;

        var bJMinimalPlayer = targetPlayerObject.AddComponent<BJMinimalPlayer>();

        return (bJServerGameManager, bJMinimalPlayer);
    }
}