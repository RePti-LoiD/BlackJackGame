using UnityEngine;

public class BJServerGameManagerFactory : BJGameManagerFactory
{
    public override BJGameManager CreateManager(GameObject targetGameObject, BJGameLoadData data)
    {
        var bJServerGameManager = targetGameObject.AddComponent<BJServerGameManager>();
        bJServerGameManager.LocalEndPoint = data.Address;
        bJServerGameManager.localPlayer = data.BJLocalUser;
        bJServerGameManager.enemyPlayer = data.BJExternalUser;
        bJServerGameManager.cardManager = data.BJCardManager;
        
        Debug.Log("Server Factory");

        return bJServerGameManager;
    }
}