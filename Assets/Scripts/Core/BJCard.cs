using UnityEngine;

public class BJCard : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cardImage;
    [SerializeField] private Sprite hiddenCard;
    [SerializeField] private Animator animator;

    [HideInInspector] public Card CardData;

    private bool isShown = false;

    public void SetCardStruct(Card cardData, int renderIndex = 0, bool isShown = false)
    {
        CardData = cardData;
        cardImage.sprite = cardData.CardSprite;
        cardImage.sortingOrder = renderIndex - 100;

        if (isShown) ShowCard();
        else HideCard();
    }

    public void SetRenderIndex(int renderIndex) => cardImage.sortingOrder = renderIndex - 100;

    public void ShowCard()
    {
        isShown = true;
        animator.SetTrigger(nameof(ShowCard));
    }

    public void HideCard()
    {
        isShown = false;

        cardImage.sprite = hiddenCard;
    }

    public void ShowCardSprite()
    {
        cardImage.sprite = CardData.CardSprite;
    }
}