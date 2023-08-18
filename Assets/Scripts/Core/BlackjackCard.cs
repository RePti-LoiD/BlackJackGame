using UnityEngine;
using UnityEngine.UI;

public class BlackjackCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Sprite hiddenCard;

    [HideInInspector] public Card cardData;

    private bool isShown = false;

    public void SetCardStruct(Card cardData, bool isShown = false)
    {
        this.cardData = cardData;
        cardImage.sprite = cardData.CardSprite;

        if (isShown) ShowCard();
        else HideCard();
    }

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