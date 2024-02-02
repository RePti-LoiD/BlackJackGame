using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using TMPro;

public class LobbyServer : MonoBehaviour
{
    [SerializeField] private GameObject modalFrame;
    [SerializeField] private TMP_Text serverText;
    [SerializeField] private TMP_Text serverLocation;
    [SerializeField] private short serverPort;

    private IPEndPoint serverEndpoint;
    private TcpListener tcpListener;

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

    private async void StartServer(TcpListener listener)
    {
        serverLocation.text = $"server started at: {listener.Server.LocalEndPoint}";
        while (true)
        {
            print("Waiting next client.. ");
            TcpClient tcpClient = await listener?.AcceptTcpClientAsync();
            print($"Client connected: {tcpClient.Client.LocalEndPoint}");

            await ListenClient(tcpClient);
        }
    }

    private async Task ListenClient(TcpClient client)
    {
        using (var stream = client.GetStream())
        {
            modalFrame.SetActive(true);
            print("Listening client");
            while (true)
            {
                byte[] buffer = new byte[256];
                int byteCount = await stream.ReadAsync(buffer);

                serverText.text = Encoding.UTF8.GetString(buffer);

                if (byteCount == 0)
                {
                    modalFrame.SetActive(false);
                    break;
                }
            }
        }
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
