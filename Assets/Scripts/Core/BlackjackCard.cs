using UnityEngine;

public class BlackjackCard : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cardImage;
    [SerializeField] private Sprite hiddenCard;

    [HideInInspector] public Card cardData;

    private bool isShown = false;

    public void SetCardStruct(Card cardData, int renderIndex = 0, bool isShown = false)
    {
        this.cardData = cardData;
        cardImage.sprite = cardData.CardSprite;
        cardImage.sortingOrder = renderIndex;

        if (isShown) ShowCard();
        else HideCard();
    }

    public void SetRenderIndex(int renderIndex) => cardImage.sortingOrder = renderIndex;

    public void ShowCard()
    {
        isShown = true;

        cardImage.sprite = cardData.CardSprite;
    }

    public void HideCard()
    {
        isShown = false;

        cardImage.sprite = hiddenCard;
    }
}