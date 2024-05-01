using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEngine;

public class NetClient
{
    public Action OnConnectedToServer;
    public Action<User[]> OnUsersListHandler;
    public Action<User> OnClientConnect;
    public Action<User> OnClientDisconnect;

    public NetworkStream Stream { get; private set; }
    private TcpClient tcpClient;
    private IPEndPoint targetEndPoint;

    private const int targetPort = 8888;

    private List<User> connectedUsers = new();

    public NetClient() : this(new IPEndPoint(IPAddress.Loopback, targetPort))
    { }

    public NetClient(IPEndPoint targetEndPoint)
    {
        this.targetEndPoint = targetEndPoint;

        tcpClient = new TcpClient();
        tcpClient.Connect(this.targetEndPoint);
        Stream = tcpClient.GetStream();

        OnConnectedToServer?.Invoke();
        ListenServer();

        //TODO: сделать здесь отправку своего пакта пользователя на сервак
    }

    private async void ListenServer()
    {

    }

    private void ClientConnect(User user)
    {
        OnClientConnect?.Invoke(user);

        connectedUsers.Add(user);
    }

    private void ClientDisconnect(User user)
    {
        OnClientDisconnect?.Invoke(user);

        connectedUsers.Remove(user);
    }

    private void HandleClientMessage(string message)
    {
        Debug.Log(message);
    }
}