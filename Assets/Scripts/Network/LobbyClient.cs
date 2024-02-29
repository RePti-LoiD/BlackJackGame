using System.Net.Sockets;
using System.Net;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class LobbyClient : NetworkManager
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private UserDataVisualization visualization;

    public void StartClient(IPEndPoint endpoint)
    {
        tcpClient = new TcpClient();
        tcpClient.Connect(endpoint);
        text.text += "client connected";
        dataStream = tcpClient.GetStream();

        ListenNetworkStream();

        SendNetworkMessage(new("UserData", UserDataWrapper.UserData.Id.ToString(), "UserData", new() { JsonConvert.SerializeObject(UserDataWrapper.UserData) }));
    }

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        switch (data.Header)
        {
            case "UserData":
                visualization.VisualizeUserData(JsonConvert.DeserializeObject<User>(data.Args[0]));
                break;
        }
    }

    public void CloseClient()
    {
        dataStream?.Dispose();
        tcpClient?.Dispose();
    }
}