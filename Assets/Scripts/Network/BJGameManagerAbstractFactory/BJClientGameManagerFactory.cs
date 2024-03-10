using UnityEngine;

public class BJClientGameManagerFactory : BJGameManagerFactory
{
    public override (BJGameManager, BJPlayer) CreateManager(GameObject targetGameObject, GameObject targetPlayerObject, BJGameLoadData data)
    {
        var bJClientGameManager = targetGameObject.AddComponent<BJClientGameManager>();
        bJClientGameManager.dataStream = data.DataStream;
        bJClientGameManager.localPlayer = data.BJLocalUser;
        bJClientGameManager.enemyPlayer = data.BJExternalUser;
        bJClientGameManager.cardManager = data.BJCardManager;

        var bJMinimalPlayer = targetPlayerObject.AddComponent<BJMinimalPlayer>();


        return (bJClientGameManager, bJMinimalPlayer);
    }
}