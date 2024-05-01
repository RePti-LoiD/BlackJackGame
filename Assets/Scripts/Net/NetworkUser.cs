using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkUser
{
    public User User { get; private set; }
    public TcpClient TcpClient { get; private set; }
    public NetworkStream NetworkStream { get; private set; }

    public Action<NetworkUser> OnClientConnect;
    public Action<NetworkUser, string> OnReceiveMessage;
    public Action<NetworkUser> OnClientDisconnect;

    private int callbackTimeout = 1000;

    public NetworkUser(TcpClient tcpClient, NetworkStream networkStream)
    {
        TcpClient = tcpClient;
        NetworkStream = networkStream;

        _ = InitUser();

        HandleClient();
    }

    private async Task InitUser()
    {
        byte[] buffer = new byte[512];
        await NetworkStream.ReadAsync(buffer, 0, buffer.Length);

        User = JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(buffer));

        OnClientConnect?.Invoke(this);
        Debug.Log($"Init new user {TcpClient.Client.RemoteEndPoint}");

        _ = HandleStream();
    }

    private async Task HandleStream()
    {
        while (true)
        {
            byte[] buffer = new byte[256];

            await NetworkStream.ReadAsync(buffer);
            string message = Encoding.UTF8.GetString(buffer);
            
            OnReceiveMessage(this, message);
        }
    }

    private async void HandleClient()
    {
        await Task.Run(async() =>
        {
            try
            {
                while (true)
                {
                    await Task.Delay(callbackTimeout);

                    if (!TcpClient.Connected)
                    {
                        break;
                    }
                }
            }
            catch (SocketException)
            {
                OnDisconnect();
            }
        });

        OnDisconnect();
    }

    private void OnDisconnect()
    {
        OnClientDisconnect?.Invoke(this);
    }

    public override string ToString()
    {
        return User.ToString();
    }
}