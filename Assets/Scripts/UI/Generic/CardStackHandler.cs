using UnityEngine;
using System.Collections.Generic;

public class CardStackHandler : MonoBehaviour
{
    [SerializeField] private List<BlackjackCard> cards = new List<BlackjackCard>();
    
    [Header("Preferences")]
    [SerializeField] private CardStackAxis stackAxis;
    [SerializeField] private int cardsOffset;
    [SerializeField] private float transitionSpeed = 5;
    [SerializeField] private bool backNumeration = false;

    private int cardCount = 0;

    public bool TryGetCard(out BlackjackCard blackjackCard, int index)
    {
        if (index >= 0 && index < cards.Count)
        {
            blackjackCard = cards[index];
        } 
        else
        {
            blackjackCard = null;
            return false;
        }

        return true;
    }

    public BlackjackCard GetCard(int index)
    {
        return cards[index];
    }

    public void SetCard(BlackjackCard blackjackCard)
    {
        cardCount += 1;
        cards.Add(blackjackCard);
    }

    private void Update()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Vector2 newPosition = new Vector2(
                stackAxis == CardStackAxis.Horizontal ? i * cardsOffset : 0, 
                stackAxis == CardStackAxis.Vertical ? i * cardsOffset : 0 
            );

            cards[i].transform.localPosition = Vector2.Lerp(
                cards[i].transform.localPosition, 
                newPosition, 
                Time.deltaTime * transitionSpeed);

            cards[i].SetRenderIndex(i * (backNumeration ? -1 : 1));
        }
    }
}