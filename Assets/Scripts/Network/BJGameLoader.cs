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

    public async void Start()
    {
        localPlayerVisualization.VisualizeUserData(Data.LocalUser);
        externalPlayerVisualization.VisualizeUserData(Data.ExternalUser);

        Data.BJCardManager = cardManager;
        Data.CardHandlerExternalPlayer = cardHandlerExternalPlayer;
        Data.BJLocalUser = localPlayer;
        Data.GameManagerObject = gameObject;
        Data.BJExternalPlayerGameObject = externalPlayer;

        print(Data);

        var data = await Data.Factory.CreateManagerAsync(Data);

        data.Item1.AddNetworkMessageListener("StepState", stepVizualization.StepStateVizualize);
        data.Item1.AddNetworkMessageListener("OnBet", stepVizualization.StepStateVizualize);

        data.Item1.StartGame();
    }
}