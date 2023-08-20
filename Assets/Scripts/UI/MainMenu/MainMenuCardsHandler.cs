using TMPro;
using System;
using UnityEngine;

public class MainMenuCardsHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private GameObject cardDragPrefab;
    [SerializeField] private Card[] cards;
    [SerializeField] private int cardCount = 2;
    [SerializeField] private int cardOffset = 30;
    [SerializeField] private float cardAdditionalScale = 0.05f;

    private GameObject[] cardList;

    private void Start()
    {
        cards = Resources.LoadAll<Card>("");
        cardList = new GameObject[cardCount];

        LoadCards();
    }

    private void LoadCards()
    {
        for (int i = 0; i < cardCount; i++) 
        {
            cardList[i] = InstantiateObject(cardDragPrefab, cards[UnityEngine.Random.Range(0, cards.Length - 1)], new Vector2(0, cardOffset * i), ReloadCards);
            cardList[i].transform.localScale = new Vector2(1 + cardAdditionalScale * i, 1 + cardAdditionalScale * i);
        }

        cardList[cardList.Length - 1].GetComponent<CardDragInteraction>().enabled = true;
    }

    private void ReloadCards(CardDragInteraction cardDragInteraction)
    {
        countText.text = (Convert.ToInt32(countText.text) + cardDragInteraction.cardData.CardWeight).ToString();

        cardDragInteraction.onDelete -= ReloadCards;
        Destroy(cardDragInteraction.gameObject);
        cardList[cardList.Length - 1] = null;

        for (int i = cardCount - 1; i >= 1; i--)
        {
            cardList[i] = cardList[i - 1];
            cardList[i].GetComponent<CardDragInteraction>().SetInstantPosition(new Vector2(0, cardOffset * i));

            cardList[i].transform.localScale = new Vector2(1 + cardAdditionalScale * i, 1 + cardAdditionalScale * i);
            cardList[i].GetComponent<CardDragInteraction>().enabled = false;
        }

        cardList[0] = InstantiateObject(cardDragPrefab, cards[UnityEngine.Random.Range(0, cards.Length - 1)], new Vector2(0, 0), ReloadCards);
        cardList[0].transform.SetSiblingIndex(0);

        cardList[cardList.Length - 1].GetComponent<CardDragInteraction>().enabled = true;
    }

    private GameObject InstantiateObject(GameObject prefab, Card cardData, Vector2 instantPosition, Action<CardDragInteraction> action)
    {
        GameObject card = Instantiate(prefab, Vector2.zero, Quaternion.identity, transform);
        card.GetComponent<CardDragInteraction>().SetCardData(cardData);
        card.GetComponent<CardDragInteraction>().SetInstantPosition(instantPosition);
        card.GetComponent<CardDragInteraction>().onDelete += action;

        return card;
    }
}