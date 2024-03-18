using UnityEngine;

public class BJGameLoader : MonoBehaviour
{
    [SerializeField] private BJBet betHandler;

    [SerializeField] private BJPlayer localPlayer;
    [SerializeField] private GameObject externalPlayer;

    [SerializeField] private CardStackHandler cardHandlerExternalPlayer;

    [SerializeField] private BJCardManager cardManager;

    [SerializeField] private BJPlayerDataVizualization localPlayerVisualization;
    [SerializeField] private BJPlayerDataVizualization externalPlayerVisualization;

    [SerializeField] private BJPlayerStepVizualization stepVizualization;

    public static BJGameLoadData Data;

    public async void Start()
    {
        Data.BJCardManager = cardManager;
        Data.CardHandlerExternalPlayer = cardHandlerExternalPlayer;
        Data.BJLocalUser = localPlayer;
        Data.GameManagerObject = gameObject;
        Data.BJExternalPlayerGameObject = externalPlayer;

        print(Data);

        var data = await Data.Factory.CreateManagerAsync(Data);

        data.Item1.AddNetworkMessageListener("StepState", stepVizualization.StepStateVizualize);
        data.Item1.AddNetworkMessageListener("OnBet", stepVizualization.StepStateVizualize);

        betHandler.OnBetFinished += (betAmount) =>
        {
            localPlayer.PlayerBet = betAmount;
            data.Item2.PlayerBet = betAmount;

            data.Item1.StartGame();
        };

        localPlayerVisualization.VisualizeBJPlayerData(Data.BJLocalUser);
        externalPlayerVisualization.VisualizeBJPlayerData(data.Item2);
    }
}