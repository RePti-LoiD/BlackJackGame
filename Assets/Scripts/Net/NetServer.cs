using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class NetServer : NetObject
{
    public Action<EndPoint> OnServerStart;
    public Action<NetworkUser> OnClientConnect;
    public Action<NetworkUser> OnClientDisconnect;
    public Action<NetworkUser, NetworkMessage> OnClientMessage;

    private TcpListener listener;
    private IPEndPoint targetEndPoint;

    private const int targetPort = 8888;

    public List<NetworkUser> connectedUsers = new();

    public NetServer() : this(new IPEndPoint(IPAddress.Loopback, targetPort))
    { }

    public NetServer(IPEndPoint targetEndPoint) : base()
    {
        this.targetEndPoint = targetEndPoint;

        listener = new TcpListener(this.targetEndPoint);
        listener.Start();
        OnServerStart?.Invoke(listener.LocalEndpoint);
        Debug.Log("Server init");

        _ = AcceptNewUsers();
    }

    private async Task AcceptNewUsers()
    {
        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            NetworkStream stream = client.GetStream();

            NetworkUser networkUser = new NetworkUser(client, stream);
            networkUser.OnClientConnect += ClientConnect;
            networkUser.OnReceiveMessage += HandleClientMessage;
            networkUser.OnClientDisconnect += ClientDisconnect;
        }
    }

    private void ClientConnect(NetworkUser networkUser)
    {
        OnClientConnect?.Invoke(networkUser);

        connectedUsers.Add(networkUser);
    }

    private void ClientDisconnect(NetworkUser networkUser)
    {
        OnClientDisconnect?.Invoke(networkUser);

        connectedUsers.Remove(networkUser);
    }

    private void HandleClientMessage(NetworkUser networkUser, string message)
    {
        var data = JsonConvert.DeserializeObject<NetworkMessage>(message);

        Debug.Log(data);
        InvokeMethod(data);
        OnClientMessage?.Invoke(networkUser, JsonConvert.DeserializeObject<NetworkMessage>(message));
    }

    public void RegistryNetworkObject(object networkObject)
    {
        funcInvoker.RegistryObject(networkObject);

        foreach (var item in NetFuncInvoker.netMethods)
        {
            foreach (var item2 in item.Value)
                Debug.Log($"{item2}");
        }
    }

    public User GetUser(Guid guid)
    {
        return connectedUsers.Where(x => x.User.Id == guid).FirstOrDefault().User;
    }
}