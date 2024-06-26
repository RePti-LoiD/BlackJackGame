using System.Net.Sockets;
using System.Net;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using ZXing.Datamatrix.Encoder;
using System.Text;

public class LobbyClient : NetworkManager
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private UserDataVisualization visualization;
    [SerializeField] private ModalFrame modalFrame;
    [SerializeField] private GameObject modalFrameButtonsParent;

    private User remoteUserData;
    private IPEndPoint remoteEndPoint;

    protected void Awake()
    {
        AddNetworkMessageListener("UserData", UserDataReceiveNetworkMethod);
        AddNetworkMessageListener("StartGame", StartGameNetworkMethod);
    }

    protected override void Start()
    { }

    public async void StartClient(IPEndPoint endpoint)
    {
        tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(endpoint.Address, endpoint.Port);
        text.text += "client connected";
        dataStream = tcpClient.GetStream();

        remoteEndPoint = endpoint; 
        
        ListenNetworkStream();

        await dataStream.WriteAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(UserDataWrapper.UserData)));

        SendNetworkMessage(new BJRequestData()
        {
            Header = "UserData",
            State = "UserData",
            UserSenderId = UserDataWrapper.UserData.Id.ToString(),
            Args = new() { JsonConvert.SerializeObject(UserDataWrapper.UserData) }
        });
    }

    private void StartGameNetworkMethod(BJRequestData data)
    {
        print(remoteEndPoint);
        BJGameLoader.Data = new BJGameLoadData(remoteEndPoint, UserDataWrapper.UserData, remoteUserData, new BJClientGameManagerFactory());

        SceneManager.LoadScene("BlackjackSubZero");
    }

    private void UserDataReceiveNetworkMethod(BJRequestData data)
    {
        visualization.gameObject.SetActive(true);
        remoteUserData = JsonConvert.DeserializeObject<User>(data.Args[0]);
        visualization.VisualizeUserData(remoteUserData);
        modalFrameButtonsParent.SetActive(true);


        modalFrame.OnAnswer += (answer) => SendNetworkMessage(new BJRequestData()
        {
            Header = "ConnectionRequest",
            State = "ConnectionRequest",
            UserSenderId = UserDataWrapper.UserData.Id.ToString(),
            Args = new() { ((int)answer).ToString() }
        });
    }

    public void CloseClient()
    {
        Dispose();

        dataStream?.Dispose();
        tcpClient?.Dispose();
    }
}