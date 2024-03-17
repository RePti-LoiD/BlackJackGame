using System.Threading.Tasks;

public class BJLocalGameManagerFactory : BJGameManagerFactory
{
    public override async Task<(BJGameManager, BJPlayer)> CreateManagerAsync(BJGameLoadData data)
    {
        BJLocalGameManager localGameManager = data.GameManagerObject.AddComponent<BJLocalGameManager>();
        BJBotPlayer enemyPlayer = data.BJExternalPlayerGameObject.AddComponent<BJBotPlayer>();
        enemyPlayer.CardHandler = data.CardHandlerExternalPlayer;

        localGameManager.cardManager = data.BJCardManager;

        localGameManager.localPlayer = data.BJLocalUser;
        localGameManager.localPlayer.UserData = data.LocalUser;

        localGameManager.enemyPlayer = enemyPlayer;
        enemyPlayer.UserData = data.ExternalUser;

        
        return (localGameManager, enemyPlayer);
    }
}