using TMPro;
using Mirror;
using UnityEngine;
using System.Linq;

public class NetworkPlayer : NetworkBehaviour 
{
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private TextMeshProUGUI userIdText;
    [SerializeField] private GameObject isOwnedText;

    [SerializeField] private RectTransform[] transforms;

    [SerializeField] private TextMeshProUGUI listText;

    [SerializeField] [SyncVar] private string userName = UserData.Data.Name;
    [SerializeField] [SyncVar] private string userId = UserData.Data.Id.ToString();
    
    public void FixedUpdate()
    {
        InitUser();

        if (isOwned)
        {
            userName = UserData.Data.Name;
            userId = UserData.Data.Id.ToString();
        }
        PlaceUserData(userName, userId);
        
        Debug.Log($"{userName} {userId}");
    }

    private void InitUser()
    {
        transform.parent = FindObjectOfType<TargetParent>().transform;
        isOwnedText.SetActive(isOwned);

        transforms = FindObjectsOfType<RectTransformAnchor>()
        .ToList()
        .Select((item) => item.GetComponent<RectTransform>())
        .ToArray();

        RectTransform playerRectTransform = gameObject.GetComponent<RectTransform>();
        
        int index = 0;
        if (isOwned) index = 1;
        
        playerRectTransform.anchoredPosition = transforms[index].anchoredPosition;
        playerRectTransform.anchorMin = transforms[index].anchorMin;
        playerRectTransform.anchorMax = transforms[index].anchorMax;
        playerRectTransform.pivot = transforms[index].pivot;
    }

    [Command]    
    public void PlaceUserData(string userName, string userId) 
    {
        Debug.Log("Server call");
        PlaceUserData2(userName, userId);
    }

    [ClientRpc]
    private void PlaceUserData2(string userName, string userId)
    {
        Debug.Log("Client call");

        userNameText.text = userName;
        userIdText.text = userId;
    }
}