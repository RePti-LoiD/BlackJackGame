using System.Collections.Generic;
using UnityEngine;

public class BJCardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<Card> cards = new List<Card>();
    [SerializeField] private bool cardsLayering = true;

    [SerializeField] private List<BJCard> spawnedCards = new List<BJCard>();

    [SerializeField] private int cardsRotation = 25; 
    public bool IsStackEmpty => spawnedCards.Count == 0;

    public void Awake()
    {
        cards = cards.ShuffleList();

        int renderIndex = 0;
        
        foreach (Card card in cards)
        {
            renderIndex++;
            GameObject spawnedCard = Instantiate(cardPrefab, transform);
            spawnedCard.GetComponent<BJCard>().SetCardStruct(card, cardsLayering ? renderIndex : -renderIndex);
            spawnedCard.gameObject.transform.eulerAngles = new Vector3(0, 0,cardsRotation);

            spawnedCards.Add(spawnedCard.GetComponent<BJCard>());
        }
    }

    public BJCard GetCard(int weight)
    {
        BJCard bjCard = null;

        foreach (var card in spawnedCards)
        {
            if (card.CardData.CardWeight == weight)
                bjCard = card;
        }
        
        spawnedCards.Remove(bjCard);

        return bjCard;
    }

    public BJCard GetCard()
    {
        var card = spawnedCards[^1];
        spawnedCards.RemoveAt(spawnedCards.Count - 1);

        return card;
    }

    public BJCard PeekLastCard()
    {
        return spawnedCards[^1];
    }

    public void ReturnCard(BJCard card)
    {
        spawnedCards.Add(card);

        card.gameObject.transform.parent = transform;
        card.transform.rotation = Quaternion.identity;
        card.HideCard();

        spawnedCards = spawnedCards.ShuffleList();
    }
}
