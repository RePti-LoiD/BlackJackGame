using UnityEngine;

public class BJGameLoader : MonoBehaviour
{
    [SerializeField] private BJPlayer localPlayer;
    [SerializeField] private GameObject externalPlayer;

    [SerializeField] private CardStackHandler cardHandlerExternalPlayer;

    [SerializeField] private BJCardManager cardManager;

    [SerializeField] private UserDataVisualization localPlayerVisualization;
    [SerializeField] private UserDataVisualization externalPlayerVisualization;

    [SerializeField] private BJPlayerStepVizualization stepVizualization;

    public static BJGameLoadData Data;

    public void Awake()
    {
        localPlayerVisualization.VisualizeUserData(Data.LocalUser);
        externalPlayerVisualization.VisualizeUserData(Data.ExternalUser);
        
        var data = Data.Factory.CreateManager(gameObject, externalPlayer, Data);

        localPlayer.UserData = Data.LocalUser;
        data.Item2.UserData = Data.ExternalUser;
        data.Item2.CardHandler = cardHandlerExternalPlayer;

        Data.BJCardManager = cardManager;
        Data.BJLocalUser = localPlayer;
        Data.BJExternalUser = data.Item2;

        data.Item1.AddNetworkMessageListener("StepState", stepVizualization.ReceiveNetworkMessage);

        data.Item1.localPlayer = Data.BJLocalUser;
        data.Item1.enemyPlayer = Data.BJExternalUser;
        data.Item1.cardManager = cardManager;
    }
}