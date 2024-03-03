using System.Collections.Generic;

public abstract class BJGameManager : NetworkManager
{
    protected List<INetworkMessageHandler> messageHandlers = new List<INetworkMessageHandler>();

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

    public void AddNetworkMessageListener(INetworkMessageHandler messageHandler) => 
        messageHandlers.Add(messageHandler);

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        messageHandlers.ForEach((handler) => handler?.ReceiveNetworkMessage(data));
    }
}