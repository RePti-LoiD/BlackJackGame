using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[Obsolete("Use UiChangeAction as aggregation component for ui event invoker")]
public class ButtonSpriteChanger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image targetButton;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pointerDownSprite;

    public void OnPointerDown(PointerEventData eventData) => targetButton.sprite = pointerDownSprite;

    public void OnPointerUp(PointerEventData eventData) => targetButton.sprite = defaultSprite;
}