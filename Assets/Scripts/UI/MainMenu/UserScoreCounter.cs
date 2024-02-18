using UnityEngine;

public class UserScoreCounter : IAction
{
    [SerializeField] private MainMenuCardsHandler cardsHandler;
    [SerializeField] [Range(0, 10000)] private int minDoubleBalanceChance;
    [SerializeField] private string currencySymbol;

    private void Start()
    {
        cardsHandler.OnCardSwipe += (cardWeight) =>
        {
            int addBalance = cardWeight;
            int bonusMultiplier = Random.Range(0, 10000) > minDoubleBalanceChance ? 2 : 1;

            UserDataWrapper.UserData.UserWallet.AddMoney(addBalance * bonusMultiplier);
            OnBalanceChangeAction?.Invoke(cardWeight, bonusMultiplier, currencySymbol);
        };
    }
}