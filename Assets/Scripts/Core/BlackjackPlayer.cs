using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BlackjackPlayer : MonoBehaviour, IOnTurn, IOnTurnEnd
{
    [SerializeField] protected BlackjackCardManager cardManager;
    [SerializeField] protected Transform cardHandler;
    [SerializeField] protected TextMeshProUGUI scoreText;

    [Header("UI")]
    [SerializeField] protected GameObject turnUiHandler;
    [SerializeField] protected Button hitButton;
    [SerializeField] protected Button standButton;

    [Header("Other")]
    [SerializeField] protected CardStackHandler cardStackHandler;

    protected List<Card> cards = new List<Card>();
    protected TurnState turnState; 

    protected bool isMineTurn;

    public TurnState TurnState => turnState;

    protected virtual void Start()
    {
        hitButton.onClick.AddListener(() =>
        {
            turnState = TurnState.Hit;

            cardManager.EndTurn();
        });

        standButton.onClick.AddListener(() =>
        {
            turnState = TurnState.Stand;

            cardManager.EndTurn();
        });
    }

    public virtual void SetCard(BJCard card)
    {
        if (cards.Count == 0)
            card.HideCard();
        else
            card.ShowCard();

        card.gameObject.transform.SetParent(cardHandler, true);

        cardStackHandler.SetCard(card);

        cards.Add(card.CardData);

        ShowScore(CalculateScore());
    }

    public virtual int CalculateScore()
    {
        int score = 0;
        
        foreach (Card card in cards)
            score += card.CardWeight;

        return score;
    }

    protected virtual void ShowScore(int score) => scoreText.text = score.ToString();

    public virtual void OnTurn(BlackjackCardManager cardManager)
    {
        turnUiHandler.SetActive(true);

        turnState = TurnState.None;
    }

    public virtual TurnState OnTurnEnd()
    {
        turnUiHandler.SetActive(false);

        return turnState;
    }
}