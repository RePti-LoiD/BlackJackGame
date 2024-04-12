using UnityEngine;

public class DragTest : MonoBehaviour
{
    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform target;

    public void OnMouseDrag()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, Camera.main, out Vector2 localPoint);

        target.localPosition = localPoint;
    }
}