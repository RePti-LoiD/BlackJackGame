using UnityEngine;

public class BJClientGameManagerFactory : BJGameManagerFactory
{
    public override BJGameManager CreateManager(GameObject targetGameObject, BJGameLoadData data)
    {
        var bJClientGameManager = targetGameObject.AddComponent<BJClientGameManager>();
        bJClientGameManager.ExternalEndPoint = data.Address;
        bJClientGameManager.localPlayer = data.BJLocalUser;
        bJClientGameManager.enemyPlayer = data.BJExternalUser;
        bJClientGameManager.cardManager = data.BJCardManager;

        return bJClientGameManager;
    }
}