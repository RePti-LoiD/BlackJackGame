using Newtonsoft.Json;
using System;

public class NetworkMessageHandler
{
    public Action<User, string> OnMessageReceive;
    private NetServer netServer;

    public NetworkMessageHandler(NetServer netServer) : base()
    {
        this.netServer = netServer;

        netServer.RegistryNetworkObject(this);
    }

    [NetFunc]
    [NetConverter(nameof(Convert))]
    [NetCallback(nameof(CallBack))]
    public void NetworkMessage((User, string) message)
    {
        OnMessageReceive?.Invoke(message.Item1, message.Item2);
    }

    public (User, string) Convert(string message)
    {
        string[] arr = JsonConvert.DeserializeObject<string[]>(message);

        return (netServer.GetUser(Guid.Parse(arr[0])), arr[1]);
    }

    public void CallBack(object message)
    {
        
    }
}