using TMPro;
using Mirror;
using System;
using UnityEngine;

public class CardList : NetworkBehaviour
{
    [SerializeField] public TextMeshProUGUI textArray;
    [SerializeField] private Card[] bjCards;

    [SerializeField] public readonly SyncList<CardStruct> cards = new SyncList<CardStruct>();

    public Action<SyncList<CardStruct>> OnCardListGenerated;

    private void Start()
    {
        if (isServer)
        {
            foreach (var card in bjCards)
            cards.Add(new CardStruct()
            {
                weight = card.CardWeight,
                isShown = false
            });

            cards.Shuffle();
        }

        OnCardListGenerated?.Invoke(cards);
    }
}

public struct CardStruct
{
    public int weight;
    public bool isShown;
}