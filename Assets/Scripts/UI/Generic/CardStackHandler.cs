using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CardStackHandler : MonoBehaviour
{
    [SerializeField] private List<BlackjackCard> cards = new List<BlackjackCard>();
    
    [Header("Preferences")]
    [SerializeField] private CardStackAxis stackAxis;
    [SerializeField] private int cardsOffset;
    [SerializeField] private float transitionSpeed = 5;
    [SerializeField] private bool backNumeration = false;

    public bool TryGetCard(out BlackjackCard blackjackCard, int index)
    {
        if (index >= 0 && index < cards.Count)
        {
            blackjackCard = cards.ToList()[index];
        } 
        else
        {
            blackjackCard = null;
            return false;
        }

        return true;
    }

    public BlackjackCard PeekCard(int index)
    {
        return cards.ToList()[index];
    }

    public void SetCard(BlackjackCard blackjackCard)
    {
        blackjackCard.transform.parent = transform;
        if (cards.Count > 0) 
            blackjackCard.ShowCard();
        cards.Add(blackjackCard);
    }

    public int GetTotalCardWeight()
    {
        return cards.Sum((x) => x.CardData.CardWeight);
    }

    private void Update()
    {
        int i = 0;
        foreach (BlackjackCard card in cards)
        {
            Vector2 newPosition = new Vector2(
                stackAxis == CardStackAxis.Horizontal ? i * cardsOffset : 0, 
                stackAxis == CardStackAxis.Vertical ? i * cardsOffset : 0 
            );

            card.transform.localPosition = Vector2.Lerp(
                card.transform.localPosition, 
                newPosition, 
                Time.deltaTime * transitionSpeed);

            card.SetRenderIndex(i * (backNumeration ? -1 : 1));
            i++;
        }
    }
}