using TMPro;
using Mirror;
using UnityEngine;
using System.Linq;

public class NetworkPlayer : NetworkBehaviour 
{
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private TextMeshProUGUI userIdText;
    [SerializeField] private GameObject isOwnedText;
    [SerializeField] public GameObject cardPosition;

    [SerializeField] private RectTransform[] transforms;

    [SerializeField] [SyncVar] private string userName = UserData.Data.Name;
    [SerializeField] [SyncVar] private string userId = UserData.Data.Id.ToString();

    [SerializeField] public readonly SyncList<CardStruct> userCards = new SyncList<CardStruct>();
    
    private void Start()
    {
        string arrayToStr = "";
        foreach (CardStruct card in FindObjectOfType<CardList>().cards)
        {
            arrayToStr += card.weight + " ";
        }

        Debug.Log(arrayToStr);
        FindObjectOfType<CardList>().textArray.text = arrayToStr;
    }

    public void FixedUpdate()
    {
        InitUser();

        if (isOwned)
        {
            userName = UserData.Data.Name;
            userId = UserData.Data.Id.ToString();
        }

        PlaceUserData(userName, userId);
    }

    private void InitUser()
    {
        transform.parent = FindObjectOfType<TargetParent>().transform;
        isOwnedText.SetActive(isOwned);

        transforms = FindObjectsOfType<RectTransformAnchor>()
        .ToList()
        .Select((item) => item.GetComponent<RectTransform>())
        .ToArray();

        
        int index = 0;
        if (isOwned) index = 1;
        
        RectTransform playerRectTransform = gameObject.GetComponent<RectTransform>();
        playerRectTransform.anchoredPosition = transforms[index].anchoredPosition;
        playerRectTransform.anchorMin = transforms[index].anchorMin;
        playerRectTransform.anchorMax = transforms[index].anchorMax;
        playerRectTransform.pivot = transforms[index].pivot;
    }

    [Command]    
    public void PlaceUserData(string userName, string userId) 
    {
        PlaceUserData2(userName, userId);
    }

    [ClientRpc]
    private void PlaceUserData2(string userName, string userId)
    {
        userNameText.text = userName;
        userIdText.text = userId;
    }
}