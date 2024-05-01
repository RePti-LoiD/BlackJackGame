using UnityEngine;

public class BJGameLoader : MonoBehaviour
{
    [SerializeField] public BJBet betHandler;

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
        data.Item1.AddNetworkMessageListener("StartStep", stepVizualization.OnStep);

        betHandler.gameObject.SetActive(true);
        betHandler.OnBetSet += data.Item1.OnBet;
        data.Item1.Bet = betHandler;

        betHandler.OnBetFinished += (betAmount) =>
        {
            localPlayer.PlayerBet = betAmount;
            data.Item2.PlayerBet = betAmount;

            if (data.Item1 is BJServerGameManager serverGameManager)
                serverGameManager.OnBetFinish();
            
            localPlayerVisualization.UpdateData();
            externalPlayerVisualization.UpdateData();
            
            data.Item1.StartGame();
        };

        localPlayerVisualization.VisualizeBJPlayerData(Data.BJLocalUser);
        externalPlayerVisualization.VisualizeBJPlayerData(data.Item2);
    }
}