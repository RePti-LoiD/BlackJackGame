using UnityEngine;

public class UiFrame : ActivatableObject
{
    [SerializeField] private RectTransform exitZone;
    [SerializeField] private RectTransform uiFrame;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private float defaultYOffset;
    [SerializeField] private float returnTime;
    [SerializeField] private float minPointToClose;

    private bool parentDrag = false;
    private bool isDragging = false;

    private float currentTargetYOffser;

    public void ContentParentDrag() => parentDrag = true;

    public void ContentParentEndDrag() => parentDrag = false;

    public void PointerDrag()
    {
        Vector3 dragPosition;
        bool isEditor = true;
        isDragging = true;

#if !UNITY_EDITOR
        isEditor = false;
#endif
        if (isEditor)
            dragPosition = Input.mousePositionDelta;
        else
            dragPosition = Input.GetTouch(0).deltaPosition;


        if (!parentDrag)
            uiFrame.position = new Vector3(uiFrame.position.x, Mathf.Clamp(uiFrame.position.y + dragPosition.y, 0, uiFrame.rect.height));

        if (uiFrame.position.y < minPointToClose)
            RequireClose();
    }

    public void PointerDragEnd()
    {
        isDragging = false;
    }

    private void Update()
    {
        if (!isDragging)
        {
            uiFrame.position = Vector3.Lerp(uiFrame.position, new Vector3(uiFrame.position.x, currentTargetYOffser), returnTime * Time.deltaTime);
        }
    }

    public void RequireOpen()
    {
        currentTargetYOffser = defaultYOffset;

        gameObject.SetActive(true);
        OnActivate?.Invoke();
    }

    public void RequireClose()
    {
        isDragging = false;
        currentTargetYOffser = -20;

        OnDeactivate?.Invoke();
        Invoke(nameof(DisableObject), 0.3f);
    }

    private void DisableObject() => gameObject.SetActive(false);

    public override void OnEnable()
    { }

    public override void OnDisable()
    { }
}