using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class NetworkCard : NetworkBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Sprite[] cards;
    [SerializeField] private Sprite hiddenCard;

    [SerializeField] public CardStruct cardStruct;
    [SerializeField] private float transitionSpeed;

    [SerializeField] private int weight;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public void SetCardStruct(CardStruct cardStruct)
    {
        this.cardStruct = cardStruct;
        weight = cardStruct.weight;

        cardImage.sprite = cards[cardStruct.weight - 1];

        if (cardStruct.isShown) ShowCard();
        else HideCard();
    }

    public void ShowCard()
    {
        cardStruct.isShown = true;

        cardImage.sprite = cards[cardStruct.weight];
    }

    public void HideCard()
    {
        cardStruct.isShown = false;

        cardImage.sprite = hiddenCard;
    }

    private void Update()
    {
        if (cardStruct.isShown)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * transitionSpeed);
        }
    }

    public void SetTargetTransform(Vector3 position, Quaternion rotation, Transform parent)
    {
        targetPosition = position;
        targetRotation = rotation;

        transform.parent = parent;
    }
}