using UnityEngine;

public class ServerUiHandler : NetworkUiInterface
{
    [SerializeField] private LobbyServer lobbyServer;
    [SerializeField] private QrGenerator generator;
    [SerializeField] private GameObject serverUiFrame;

    public override void DisableUi()
    {
        lobbyServer.CloseServer();
        serverUiFrame.SetActive(false);
    }

    public override void EnableUi()
    {
        var endpoint = lobbyServer.GetLocalEndpoint();
        lobbyServer.PrepareServer();

        serverUiFrame.SetActive(true);

        generator.CreateQrAndShow(endpoint.ToString() ?? "Server internal error. Try againg.");
    }
}