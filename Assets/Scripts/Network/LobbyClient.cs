using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Text;
using TMPro;

public class LobbyClient : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private TcpClient tcpClient;
    private NetworkStream networkStream;

    public async void StartClient(IPEndPoint endpoint)
    {
        print(endpoint.ToString());
        
        tcpClient = new TcpClient()
        {
            ReceiveTimeout = 1000,
            SendTimeout = 1000
        };

        tcpClient.Connect(endpoint);

        text.text += "client connected";

        networkStream = tcpClient.GetStream();

        if (networkStream.CanWrite) 
            await networkStream.WriteAsync(Encoding.UTF8.GetBytes($"Connected client with balance {PlayerPrefs.GetInt(PlayerPrefsKeys.UserScore)}"));
    }

    public async void AcceptHandshakeWithServer()
    {
        if (tcpClient.Connected) await networkStream.WriteAsync(Encoding.UTF8.GetBytes("1"));
    }
    
    public async void RejectHandshakeWithServer()
    {
        if (tcpClient.Connected) await networkStream.WriteAsync(Encoding.UTF8.GetBytes("0"));
    }

    public void CloseClient()
    {
        tcpClient?.Dispose();
        networkStream?.Dispose();
    }
}