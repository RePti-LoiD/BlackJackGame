using UnityEngine;
using UnityEngine.UI;

public class UiSpriteChangeAction : UiChangeAction
{
    [SerializeField] private Image targetButton;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pointerDownSprite;

    public override void EnabledAction() => targetButton.sprite = pointerDownSprite;
    public override void DisabledAction() => targetButton.sprite = defaultSprite;
}