using UnityEngine;

public class BJGameLoader : MonoBehaviour
{
    [SerializeField] private BJPlayer localPlayer;
    [SerializeField] private BJPlayer externalPlayer;

    [SerializeField] private BJCardManager cardManager;

    [SerializeField] private UserDataVisualization localPlayerVisualization;
    [SerializeField] private UserDataVisualization externalPlayerVisualization;

    public static BJGameLoadData Data;

    public void Awake()
    {
        localPlayerVisualization.VisualizeUserData(Data.LocalUser);
        externalPlayerVisualization.VisualizeUserData(Data.ExternalUser);

        localPlayer.UserData = Data.LocalUser;
        externalPlayer.UserData = Data.ExternalUser;

        Data.BJCardManager = cardManager;
        Data.BJLocalUser = localPlayer;
        Data.BJExternalUser = externalPlayer;

        Data.Factory.CreateManager(gameObject, Data);
    }
}