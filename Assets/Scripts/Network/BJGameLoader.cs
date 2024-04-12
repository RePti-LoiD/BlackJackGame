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

        var data = await Data.Factory.CreateManagerAsync(Data);

        data.Item1.AddNetworkMessageListener("StepState", stepVizualization.StepStateVizualize);
        data.Item1.AddNetworkMessageListener("OnBet", stepVizualization.OnBetVizualize);
        data.Item1.AddNetworkMessageListener("OnGameEnd", stepVizualization.OnGameEndVizualize);
        
        betHandler.gameObject.SetActive(true);
        betHandler.OnBetSet += data.Item1.OnBet;


        betHandler.OnBetFinished += (betAmount) =>
        {
            localPlayer.PlayerBet = betAmount;
            data.Item2.PlayerBet = betAmount;

            data.Item1.StartGame();

            localPlayerVisualization.UpdateData();
            externalPlayerVisualization.UpdateData();
        };

        localPlayerVisualization.VisualizeBJPlayerData(Data.BJLocalUser);
        externalPlayerVisualization.VisualizeBJPlayerData(data.Item2);
    }
}