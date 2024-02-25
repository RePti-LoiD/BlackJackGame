using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TcpClientTest : MonoBehaviour
{
    async void Start()
    {
        TcpClient client = new TcpClient();
        await client.ConnectAsync(IPAddress.Loopback, 8888);
        print($"Connected to: {client.Client.RemoteEndPoint}");
        _ = HandleServer(client.GetStream());
    }

    private async Task HandleServer(NetworkStream stream)
    {
        using (stream)
        {
            while (true)
            {
                byte[] buffer = new byte[1024];

                await stream.ReadAsync(buffer);

                print(Encoding.UTF8.GetString(buffer));
            }
        }
    }
}