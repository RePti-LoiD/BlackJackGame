public abstract class BJGameManager : NetworkManager
{
    public BJPlayer localPlayer;
    public BJPlayer enemyPlayer;
    
    public BJCardManager cardManager;

    protected BJPlayer currentPlayer;
    protected BJStepState lastStepData;

    public bool IsGameEnd { get; protected set; } = false;

    public abstract void PlayerStep(BJPlayer sender, BJStepState stepState);

    public abstract void GameEnd();
    public abstract void SetCardToHandler(BJPlayer player);
    protected abstract void SetCardToHandler(BJPlayer player, BlackjackCard card);
}