using System;
using TMPro;
using UnityEngine;

public class BJBet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyBetText;
    [SerializeField] private char currency;

    public Action<int> OnBetFinished;
    public Action<int> OnBetSet;

    public int CurrentBet;

    public void SetEnemyBet(int currentBet)
    {
        CurrentBet = currentBet;
        enemyBetText.text = currentBet.ToString() + currency;

        OnBetSet?.Invoke(currentBet);
    }
}