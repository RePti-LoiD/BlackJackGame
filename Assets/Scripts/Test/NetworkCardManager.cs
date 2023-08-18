using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NetworkCardManager : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private CardList cardList;
    [SerializeField] private GameObject networkCardPrefab;

    [SerializeField] [SyncVar] private int currentCardIndex = 0; 


    private OverrideNetManager overrideNetManager;

    private void Start()
    {
        overrideNetManager = FindObjectOfType<OverrideNetManager>();
        
        overrideNetManager.OnLastClientConnected += () =>
        {
            if (isServer)
            {
                Button button = GetComponent<NetworkPlayer>().GetComponentInChildren<Button>();

                
            }
        };

        cardList.OnCardListGenerated += (cards) =>
        {
            foreach (CardStruct card in cards)
            {

            }
        };
    }



    [ClientRpc]
    private void GetCard(int index)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}