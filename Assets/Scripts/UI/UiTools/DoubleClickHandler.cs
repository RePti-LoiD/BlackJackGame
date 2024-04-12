using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DoubleClickHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float maxClickDelay = 0.4f;
    [SerializeField] private UnityEvent OnDoubleClick;

    private int clickCount = 0;
    private float lastClickTime = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        clickCount++;

        if ((clickCount > 1 || clickCount >= 2) && Time.time - lastClickTime < maxClickDelay)
        {
            OnDoubleClick.Invoke();
            clickCount = 0;
        }

        lastClickTime = Time.time;
    }
}