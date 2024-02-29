using System.Net.Sockets;
using System.Net;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class LobbyServer : NetworkManager
{
    [SerializeField] private GameObject modalFrame;
    [SerializeField] private TMP_Text serverLocation;
    [SerializeField] private short serverPort;
    [SerializeField] private UserDataVisualization visualization;

    private IPEndPoint serverEndpoint;
    private TcpListener tcpListener;

    protected override void Start() { }

    public IPEndPoint GetLocalEndpoint()
    {
        foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return new IPEndPoint(ip, serverPort);

        return new IPEndPoint(IPAddress.Any, serverPort);
    }

    public void PrepareServer()
    {
        serverEndpoint = GetLocalEndpoint();

        if (tcpListener == null)
            tcpListener = new TcpListener(serverEndpoint);

        tcpListener.Start();

        StartServer(tcpListener);
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

    private async void StartServer(TcpListener listener)
    {
        serverLocation.text = $"server started at: {listener.Server.LocalEndPoint}";

        tcpClient = await listener.AcceptTcpClientAsync();
        print($"Client connected: {tcpClient.Client.LocalEndPoint}");

        dataStream = tcpClient.GetStream();

        ListenNetworkStream();
    }

    public void OnDestroy()
    {
        CloseServer();
    }

    public void OnDisable()
    {
        CloseServer();
    }

    public void CloseServer()
    {
        tcpListener?.Stop();
        tcpListener = null;
    }
}
