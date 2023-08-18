using TMPro;
using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System;

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

public static class CardListExtension
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rand = new System.Random();

        for (int i = 0; i < list.Count - 1; i++)
            list.Swap(i, rand.Next(i, list.Count - 1));
    }

    public static void Swap<T>(this IList<T> list, int i, int randIndex)
    {
        T temp = list[i];
        list[i] = list[randIndex];
        list[randIndex] = temp;
    }
}

public struct CardStruct
{
    public int weight;
    public bool isShown;
}