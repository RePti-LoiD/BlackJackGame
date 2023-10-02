using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UiChangeAction action;

    public void OnPointerDown(PointerEventData eventData) => action.EnabledAction();

    public void OnPointerUp(PointerEventData eventData) => action.DisabledAction();
}