using System;
using TMPro;
using UnityEngine;

public class BJBet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI localPlayerBalanceText;
    [SerializeField] private TextMeshProUGUI betAmountText;

    [SerializeField] private TextMeshProUGUI enemyBetText;
    [SerializeField] private char currency;

    public Action<int> OnBetFinished;
    public Action<int, int> OnBetSet;

    [SerializeField] protected int currentBet = 0;

    public int CurrentBet 
    { 
        get => currentBet; 
        set
        {
            currentBet = value;

            betAmountText.text = currentBet.ToString() + currency;
        }
    }

    protected virtual void Start()
    {
        localPlayerBalanceText.text = UserDataWrapper.UserData.UserWallet.Balance.ToString() + currency;
    }

    public void UpdateBet(int bet)
    {
        OnBetSet?.Invoke(bet, CurrentBet);

        CurrentBet = bet;
        enemyBetText.text = bet.ToString() + currency;
    }

    public void ConfirmBet()
    {
        OnBetFinished?.Invoke(CurrentBet);
    }
}