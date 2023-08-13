using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragInteraction : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Image")]
    [SerializeField] public Card cardData; 
    [SerializeField] public Image cardImage;

    [Header("Objects")]
    [SerializeField] private GameObject rotatableObject;
    [SerializeField] private GameObject positionalObject;

    [Header("----------")]
    [SerializeField] private float returnSpeed = 8;
    [SerializeField] private bool yAxis;
    [SerializeField] private float deleteMaximum;

    [HideInInspector] public Vector2 instantPosition;
    private bool isDrag;

    public Action<CardDragInteraction> onDelete;

    private void Awake()
    {
        positionalObject.transform.localPosition = instantPosition;
    }

    public void OnBeginDrag(PointerEventData eventData) => 
        isDrag = true;

    public void OnDrag(PointerEventData eventData)
    {
        positionalObject.transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y * Convert.ToInt32(yAxis));
        rotatableObject.transform.localEulerAngles += new Vector3(0f, 0f, -eventData.delta.x / 20);
    }

    public void OnEndDrag(PointerEventData eventData) =>
        isDrag = false;

    private void Update()
    {
        if (!isDrag)
        {
            positionalObject.transform.localPosition = Vector3.Lerp(positionalObject.transform.localPosition, 
            instantPosition, 
            returnSpeed * Time.deltaTime);
            
            rotatableObject.transform.localRotation = Quaternion.Lerp(rotatableObject.transform.localRotation, 
                Quaternion.identity, 
                returnSpeed * Time.deltaTime);
        }
        else if (positionalObject.transform.localPosition.x < -deleteMaximum || positionalObject.transform.localPosition.x > deleteMaximum)
        {
            onDelete?.Invoke(this);
        }
    }

    public void SetCardData(Card card)
    {
        cardData = card;

        cardImage.sprite = cardData.CardSprite;
    }

    public void SetInstantPosition(Vector2 position)
    {
        instantPosition = position;

        positionalObject.transform.localPosition = instantPosition;
    }
}
