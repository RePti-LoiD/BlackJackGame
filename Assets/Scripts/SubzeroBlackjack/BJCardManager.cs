using System.Collections.Generic;
using UnityEngine;

public class BJCardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<Card> cards = new List<Card>();

    [SerializeField] private List<BlackjackCard> spawnedCards = new List<BlackjackCard>();
    public bool IsStackEmpty => spawnedCards.Count == 0;

    private void Awake()
    {
        cards = cards.ShuffleList();

        int renderIndex = 0;
        
        foreach (Card card in cards)
        {
            renderIndex++;
            GameObject spawnedCard = Instantiate(cardPrefab, transform);
            spawnedCard.GetComponent<BlackjackCard>().SetCardStruct(card, renderIndex);

            spawnedCards.Add(spawnedCard.GetComponent<BlackjackCard>());
        }
    }

    public BlackjackCard GetCard(int weight)
    {
        foreach (var card in spawnedCards)
        {
            if (card.CardData.CardWeight == weight)
                return card;
        }

        return null;
    }

    public BlackjackCard GetCard()
    {
        var card = spawnedCards[^1];
        spawnedCards.RemoveAt(spawnedCards.Count - 1);

        return card;
    }
}
