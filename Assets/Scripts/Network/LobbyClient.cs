using System.Net.Sockets;
using System.Net;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class LobbyClient : NetworkManager
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private UserDataVisualization visualization;
    [SerializeField] private ModalFrame modalFrame;
    [SerializeField] private GameObject modalFrameButtonsParent;

    private User remoteUserData;

    protected override void Start() { }

    public void StartClient(IPEndPoint endpoint)
    {
        tcpClient = new TcpClient();
        tcpClient.Connect(endpoint);
        text.text += "client connected";
        dataStream = tcpClient.GetStream();
        
        Console.WriteLine(tcpClient.Client.RemoteEndPoint);
        
        ListenNetworkStream();

        SendNetworkMessage(new BJRequestData()
        {
            Header = "UserData",
            State = "UserData",
            UserSenderId = UserDataWrapper.UserData.Id.ToString(),
            Args = new() { JsonConvert.SerializeObject(UserDataWrapper.UserData) }
        });
    }

    protected override void HandleNetworkMessage(BJRequestData data)
    {
        switch (data.Header)
        {
            case "UserData":

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
                break;

            case "StartGame":
                string[] endPointParts = tcpClient.Client.RemoteEndPoint.ToString().Split(":");
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(endPointParts[0]), int.Parse(endPointParts[1]));

                BJGameLoader.Data = new BJGameLoadData(endpoint, UserDataWrapper.UserData, remoteUserData, new BJClientGameManagerFactory());

                SceneManager.LoadScene("BlackjackSubZero");
                break;
        }
    }

    public void CloseClient()
    {
        dataStream?.Dispose();
        tcpClient?.Dispose();
    }
}