using System.Xml;
using UnityEngine;

public class BJCardDragHandler : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private BJCardManager cardManager;
    [SerializeField] private BJLocalPlayer localPlayer;
    [SerializeField] private int pickBound = -400;
    [SerializeField] private GameObject pickIndicator;
    [SerializeField] private BJGameManager gameManager;

    private RectTransform currentCardTransform;
    private Vector2 lastPosition;

    public void OnMouseStartDrag()
    {
        if (gameManager == null) 
            gameManager = FindAnyObjectByType<BJGameManager>();

        if (gameManager.CurrentPlayer != localPlayer) return;

        currentCardTransform = cardManager.PeekLastCard().GetComponent<RectTransform>();
        currentCardTransform.transform.parent = canvas.transform;

        pickIndicator.SetActive(true);
    }

    public void OnMouseDrag()
    {
        if (gameManager.CurrentPlayer != localPlayer) return;

        Vector2 localPos;
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer: localPos = Input.mousePosition; break;
            case RuntimePlatform.WebGLPlayer: localPos = Input.mousePosition; break;
            case RuntimePlatform.WindowsEditor: localPos = Input.mousePosition; break;
            case RuntimePlatform.Android: localPos = Input.GetTouch(0).position; break;
            default: localPos = Vector2.zero; break;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, localPos, Camera.main, out Vector2 pos);
        
        currentCardTransform.localPosition = new Vector3(pos.x, pos.y, 0f);
        

        lastPosition = pos;
    }

    public void OnMouseEndDrag()
    {
        if (gameManager.CurrentPlayer != localPlayer) return;

        if (pickBound > currentCardTransform.localPosition.y)
            localPlayer.PickCard();
        else
            currentCardTransform.transform.parent = cardManager.transform;
        
        currentCardTransform = null;
        pickIndicator.SetActive(false);
    }
}