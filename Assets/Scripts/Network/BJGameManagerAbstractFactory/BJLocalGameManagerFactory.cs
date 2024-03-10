using UnityEngine;

public class BJLocalGameManagerFactory : BJGameManagerFactory
{
    public override (BJGameManager, BJPlayer) CreateManager(GameObject targetGameObject, GameObject targetPlayerObject, BJGameLoadData data)
    {
        var localGameManager = targetGameObject.AddComponent<BJLocalGameManager>();
        localGameManager.cardManager = data.BJCardManager;
        localGameManager.localPlayer = data.BJLocalUser;
        localGameManager.enemyPlayer = data.BJExternalUser;

        var enemyPlayer = targetPlayerObject.AddComponent<BJBotPlayer>();
        
        return (localGameManager, enemyPlayer);
    }
}