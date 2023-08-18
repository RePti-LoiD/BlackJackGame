using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class BlackjackCardManager : MonoBehaviour
{
    [SerializeField] private BlackjackPlayer player, bot;

    [Header("---------------")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] public List<Card> cards = new List<Card>();
    [SerializeField] private List<BlackjackCard> spawnedCards = new List<BlackjackCard>(); 
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI turnText;

    [Header("Other")]
    [SerializeField] private float botTimeToThink = 2f;

    public int MaxBlackjackScore = 21;

    private int currentCard = 0;

    private BlackjackPlayer currentPlayer;

    private void Start()
    {
        cards = cards.ShuffleList();

        string listToStr = string.Empty;
        foreach (Card card in cards)
        {
            GameObject spawnedCard = Instantiate(cardPrefab, transform);
            spawnedCard.GetComponent<BlackjackCard>().SetCardStruct(card);

            spawnedCards.Add(spawnedCard.GetComponent<BlackjackCard>());

            listToStr += card.CardWeight + " ";
        }
        Debug.Log(listToStr);

        player.SetCard(PutCard());
        player.SetCard(PutCard());

        bot.SetCard(PutCard());
        bot.SetCard(PutCard());

        SetCurrentPlayer(player);
    }

    public void NextTurn()
    {
        print($"{currentPlayer.gameObject.name} is player {currentPlayer is BlackjackPlayer}");
        
        if (bot.TurnState == TurnState.Stand && player.TurnState == TurnState.Stand)
        {
            turnText.text = "Game ended";

            return;
        }

        if (currentPlayer == bot)
        {
            SetCurrentPlayer(player);
        }

        else if (currentPlayer == player)
        {
            SetCurrentPlayer(bot);

            StartCoroutine(Duration(botTimeToThink));
        }
    }

    public void EndTurn()
    {
        switch(currentPlayer.OnTurnEnd())
        {
            case TurnState.Stand:
                break;
            case TurnState.Hit:
                currentPlayer.SetCard(PutCard());
                break;
        }

        NextTurn();
    }

    private void SetCurrentPlayer(BlackjackPlayer blackjackPlayer)
    {
        if (blackjackPlayer is BlackjackBotPlayer) 
            turnText.text = "Bot turn";
        else 
            turnText.text = "Your turn";

        currentPlayer = blackjackPlayer;
        
        currentPlayer.OnTurn(this);
    }

    public BlackjackCard PutCard() 
    {
        if (currentCard + 1 > spawnedCards.Count - 1) return null;

        BlackjackCard blackjackCard = spawnedCards[currentCard];

        currentCard++;

        return blackjackCard;
    }

    IEnumerator Duration(float seconds) 
    {
        yield return new WaitForSeconds(seconds);

        EndTurn();
    }
}