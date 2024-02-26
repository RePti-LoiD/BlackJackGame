using UnityEngine;

public class BlackjackCard : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cardImage;
    [SerializeField] private Sprite hiddenCard;

    [HideInInspector] public Card CardData;

    private bool isShown = false;

    public void SetCardStruct(Card cardData, int renderIndex = 0, bool isShown = false)
    {
        this.CardData = cardData;
        cardImage.sprite = cardData.CardSprite;
        cardImage.sortingOrder = renderIndex - 100;

        if (isShown) ShowCard();
        else HideCard();
    }

    public void SetRenderIndex(int renderIndex) => cardImage.sortingOrder = renderIndex - 100;

    public void ShowCard()
    {
        isShown = true;

        cardImage.sprite = CardData.CardSprite;
    }

    public void HideCard()
    {
        isShown = false;

        cardImage.sprite = hiddenCard;
    }
}