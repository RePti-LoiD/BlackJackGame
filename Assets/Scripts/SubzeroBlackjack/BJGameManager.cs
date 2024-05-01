using System.Linq;

public abstract class BJGameManager : NetworkManager
{
    public int GameTarget = 21;

    public BJPlayer localPlayer;
    public BJPlayer enemyPlayer;

    public BJBet Bet;
    
    public BJCardManager cardManager;

    protected BJPlayer currentPlayer;
    protected BJStepState lastStepData;

    protected BJPlayer currentWinner;

    public BJPlayer CurrentPlayer { get => currentPlayer; }
    public bool IsGameEnd { get; protected set; } = false;

    public abstract void StartGame();

    public abstract void PlayerStep(BJPlayer sender, BJStepState stepState);

    public virtual void GameEnd()
    {
        IsGameEnd = true;
        localPlayer.CardHandler.cards[0].ShowCard();
        enemyPlayer.CardHandler.cards[0].ShowCard();

        int localUserScore = localPlayer.CardHandler.cards.Sum(x => x.CardData.CardWeight);
        int enemyUserScore = enemyPlayer.CardHandler.cards.Sum(x => x.CardData.CardWeight);

        if (localUserScore > GameTarget && enemyUserScore > GameTarget)
        {
            if (localUserScore < enemyUserScore)
            {
                localPlayer.UserData.UserWallet.AddMoney(localPlayer.PlayerBet);
                currentWinner = localPlayer;
            }
            else
            {
                localPlayer.UserData.UserWallet.TryGetMoney(localPlayer.PlayerBet);
                currentWinner = enemyPlayer;
            }
        }
        else if (localUserScore <= GameTarget && enemyUserScore <= GameTarget)
        {
            if (localUserScore < enemyUserScore)
            {
                localPlayer.UserData.UserWallet.TryGetMoney(localPlayer.PlayerBet);
                currentWinner = enemyPlayer;
            }
            else if (localUserScore > enemyUserScore)
            {
                localPlayer.UserData.UserWallet.AddMoney(localPlayer.PlayerBet);
                currentWinner = localPlayer;
            }
        }
        else if (localUserScore <= GameTarget && enemyUserScore > GameTarget)
        {
            localPlayer.UserData.UserWallet.AddMoney(localPlayer.PlayerBet);
            currentWinner = localPlayer;
        }
        else if (localUserScore > GameTarget && enemyUserScore <= GameTarget)
        {
            localPlayer.UserData.UserWallet.TryGetMoney(localPlayer.PlayerBet);
            currentWinner = enemyPlayer;
        }
    }

    public virtual void OnBet(int bet, int previousBet)
    {
        InvokeHandlers(new BJRequestData("OnBet", localPlayer.UserData.Id.ToString(), "OnBet", new() { (bet - previousBet).ToString() }));
    }

    public abstract void SetCardToHandler(BJPlayer player);
    protected abstract void SetCardToHandler(BJPlayer player, BJCard card);
}