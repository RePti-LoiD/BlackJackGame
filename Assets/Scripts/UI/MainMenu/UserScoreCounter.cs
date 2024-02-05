using UnityEngine;

public class UserScoreCounter : IAction
{
    [SerializeField] private MainMenuCardsHandler cardsHandler;
    [SerializeField] [Range(0, 10000)] private int minDoubleBalanceChance;
    [SerializeField] private string currencyChar;

    private void Start()
    {
        cardsHandler.OnCardSwipe += (cardWeight) =>
        {
            int addBalance = cardWeight;
            string balanceChangeStr = $"{cardWeight}{currencyChar}";

            if (Random.Range(0, 10000) > minDoubleBalanceChance)
            {
                addBalance *= 2;
                balanceChangeStr = "2x" + balanceChangeStr;
            }

            UserDataWrapper.UserData.UserWallet.AddMoney(addBalance);
            OnBalanceChangeAction?.Invoke(balanceChangeStr);
        };
    }
}