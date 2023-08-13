using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragInteraction : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject rotatableObject;
    [SerializeField] private GameObject positionableObject;

    [Header("----------")]
    [SerializeField] private Vector2 instantPosition;
    [SerializeField] private float returnSpeed = 8;

    private bool isDrag;

    public void OnBeginDrag(PointerEventData eventData) => isDrag = true;

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        print(eventData.delta);

        positionableObject.transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y);
        rotatableObject.transform.localRotation *= Quaternion.Euler(0f, 0f, -eventData.delta.x / 20);
    }

    public void OnEndDrag(PointerEventData eventData) => isDrag = false;

    private void Update()
    {
        if (!isDrag)
        {
            positionableObject.transform.localPosition = Vector3.Lerp(positionableObject.transform.localPosition, 
            instantPosition, 
            returnSpeed * Time.deltaTime);
            
            rotatableObject.transform.localRotation = Quaternion.Lerp(rotatableObject.transform.localRotation, 
                Quaternion.identity, 
                returnSpeed * Time.deltaTime);
        }
    }
}
