using UnityEngine;

public class ServerUiHandler : NetworkUiInterface
{
    [SerializeField] private LobbyServer lobbyServer;
    [SerializeField] private QrGenerator generator;
    [SerializeField] private GameObject serverUiFrame;
    [SerializeField] private ActivatableObject activatableObject;

    private void Awake()
    {
        activatableObject.OnActivate += () =>
        {
            var endpoint = lobbyServer.GetLocalEndpoint();
            lobbyServer.PrepareServer();

            generator.CreateQrAndShow(endpoint.ToString() ?? "Server internal error. Try againg.");
        };

        activatableObject.OnDeactivate += () =>
        {
            lobbyServer.CloseServer();
        };
    }

    public override void DisableUi()
    {
        //serverUiFrame.SetActive(false);
    }

    public override void EnableUi()
    {
        //serverUiFrame.SetActive(true);
    }
}