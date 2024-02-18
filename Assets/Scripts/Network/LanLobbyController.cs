using System;
using System.Net;
using TMPro;
using UnityEngine;

public class LanLobbyController : MonoBehaviour
{
    [SerializeField] private NetworkUiInterface serverUI;
    [SerializeField] private NetworkUiInterface clientUI;
    [SerializeField] private LobbyClient client;
    [SerializeField] private TMP_Text exceptionText;

    [SerializeField] private short serverPort;

    public void StartServerSide() => serverUI.EnableUi();
    public void CloseServerSide() => serverUI.DisableUi();

    public void StartClientSide() => clientUI.EnableUi();
    public void CloseClientSide() => clientUI.DisableUi();

    public void ClientEmulation(GameObject ipEndpoint)
    {
        string[] endpointSplitted = ipEndpoint.GetComponent<TMP_InputField>().text.Split(':');
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(endpointSplitted[0]), int.Parse(endpointSplitted[1]));
        print($"Client connecting to: {endPoint}");

        client.StartClient(endPoint);
    }

    public void DisposeClient()
    {
        client.CloseClient();
    }
}