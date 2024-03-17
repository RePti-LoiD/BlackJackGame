using System.Net.Sockets;
using System.Net;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LobbyServer : NetworkManager
{
    [SerializeField] private TMP_Text serverLocation;
    [SerializeField] private short serverPort;
    [SerializeField] private UserDataVisualization visualization;
    [SerializeField] private ModalFrame modalFrame;
    [SerializeField] private TextMeshProUGUI modalFrameWaitText;
    [SerializeField] private Button startGameButton;

    private IPEndPoint serverEndpoint;
    private TcpListener tcpListener;

    private User remoteUserData;

    protected void Awake()
    {
        AddNetworkMessageListener("UserData", UserDataReceiveNetworkMethod);
        AddNetworkMessageListener("ConnectionRequest", ConnectionRequestNetworkMethod);
    }

    protected override void Start() 
    { }

    public IPEndPoint GetLocalEndpoint() =>
        new IPEndPoint(Dns.GetHostAddresses(Dns.GetHostName()).Where((x) => x.AddressFamily == AddressFamily.InterNetwork).ToList()[0], 8888);

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

        tcpClient = await listener?.AcceptTcpClientAsync();

        dataStream = tcpClient.GetStream();
        ListenNetworkStream();

        SendNetworkMessage(new BJRequestData()
        {
            Header = "UserData",
            State = "UserData",
            UserSenderId = UserDataWrapper.UserData.Id.ToString(),
            Args = new () { JsonConvert.SerializeObject(UserDataWrapper.UserData) }
        });
    }

    private void ConnectionRequestNetworkMethod(BJRequestData data)
    {
        var answer = (ModalFrameButton)int.Parse(data.Args[0]);
        string uiAnswer;

        if (answer == ModalFrameButton.Primary)
        {
            uiAnswer = @"<color=#FFD701>accept</color>";
            startGameButton.gameObject.SetActive(true);
            startGameButton.onClick.AddListener(StartGame);
        }
        else
        {
            uiAnswer = @"<color=#FF3B47>reject</color>";
        }

        modalFrameWaitText.text = $"Player {uiAnswer} game";
    }

    private void UserDataReceiveNetworkMethod(BJRequestData data)
    {
        print(data);

        visualization.gameObject.SetActive(true);
        remoteUserData = JsonConvert.DeserializeObject<User>(data.Args[0]);
        visualization.VisualizeUserData(remoteUserData);


        modalFrameWaitText.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        print(serverEndpoint);
        BJGameLoader.Data = new BJGameLoadData(serverEndpoint, UserDataWrapper.UserData, remoteUserData, new BJServerGameManagerFactory());
        
        SendNetworkMessage(new BJRequestData()
        {
            Header = "StartGame",
            State = "StartGame",
            UserSenderId = UserDataWrapper.UserData.Id.ToString(),
            Args = new()
        });

        SceneManager.LoadScene("BlackjackSubZero");
    }

    public void CopyIPEndPointToClipboard() =>
        GUIUtility.systemCopyBuffer = serverEndpoint.ToString();

    public void OnDisable() =>
        CloseServer();

    public void OnDestroy() =>
        CloseServer();

    public void CloseServer()
    {
        Dispose();

        tcpListener?.Stop();
        tcpListener = null;
    }
}
