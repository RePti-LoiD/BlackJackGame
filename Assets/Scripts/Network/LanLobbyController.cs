using System.Net;
using UnityEngine;

public class LanLobbyController : MonoBehaviour
{
    [SerializeField] private NetworkUiInterface serverUI;
    [SerializeField] private NetworkUiInterface clientUI;

    [SerializeField] private short serverPort;

    private IPEndPoint endpoint;

    public void Start()
    {
        PrepareServer();
    }

    public void PrepareServer()
    {
        foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                endpoint = new IPEndPoint(ip, serverPort);
                break;
            }

        serverUI.EnableUi(endpoint.ToString());
    }

    public void PrepareClient()
    {
        serverUI.EnableUi(null);
    }
}