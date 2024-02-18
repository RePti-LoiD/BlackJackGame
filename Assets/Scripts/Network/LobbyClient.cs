using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Text;
using TMPro;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

public class LobbyClient : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private UserDataVisualization modalFrame;

    private TcpClient tcpClient;
    private NetworkStream networkStream;

    public async void StartClient(IPEndPoint endpoint)
    {
        tcpClient = new TcpClient()
        {
            ReceiveTimeout = 1000,
            SendTimeout = 1000
        };

        tcpClient.Connect(endpoint);

        text.text += "client connected";

        networkStream = tcpClient.GetStream();

        if (networkStream.CanWrite)
            await networkStream.WriteAsync(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                        $"{(byte)TCPDataMarkers.UserDataMarker}\n" + UserDataWrapper.UserData, Formatting.None)));

        await ListenServer(tcpClient.GetStream());
    }

    private async Task ListenServer(NetworkStream stream)
    {
        modalFrame.gameObject.SetActive(true);

        while (true)
        {
            byte[] buffer = new byte[256];
            int byteCount = await stream.ReadAsync(buffer);

            string message = Encoding.UTF8.GetString(buffer);
            HandleClientMessage(message);

            if (byteCount == 0)
            {
                modalFrame.gameObject.SetActive(false);
                break;
            }
        }
    }

    private void HandleClientMessage(string message)
    {
        print(Enum.GetName(typeof(TCPDataMarkers), message.Split("\n")[0]));

        if ((TCPDataMarkers)Enum.Parse(typeof(TCPDataMarkers), message.Split("\n")[0]) == TCPDataMarkers.UserDataMarker)
            modalFrame.VisualizeUserData(JsonConvert.DeserializeObject<User>(message));

    }

    public async void AcceptHandshakeWithServer()
    {
        if (tcpClient.Connected) 
            await networkStream.WriteAsync(Encoding.UTF8.GetBytes("1"));
    }
    
    public async void RejectHandshakeWithServer()
    {
        if (tcpClient.Connected) 
            await networkStream.WriteAsync(Encoding.UTF8.GetBytes("0"));
    }

    public void CloseClient()
    {
        tcpClient?.Dispose();
        networkStream?.Dispose();
    }
}