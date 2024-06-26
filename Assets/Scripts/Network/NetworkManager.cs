using BJTcpRequestProtocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class NetworkManager : MonoBehaviour, IDisposable
{
    protected TcpClient tcpClient;
    public NetworkStream dataStream;

    protected Dictionary<string, List<Action<BJRequestData>>> bindedMethods = new();

    protected async virtual void Start()
    {
        await InnerNetworkInitialization();
    }

    protected virtual async Task NetworkInitialization() { }
    protected virtual void PostNetworkInitialization() { }
    protected virtual void HandleNetworkMessage(BJRequestData data) { }

    protected async Task InnerNetworkInitialization()
    {
        await NetworkInitialization();
        dataStream = tcpClient.GetStream();
        ListenNetworkStream();

        PostNetworkInitialization();
    }

    protected async void ListenNetworkStream()
    {
        while (true)
        {
            InvokeHandlers(await ReceiveNetworkMessage());
        }
    }

    protected void InvokeHandlers(BJRequestData data)
    {
        print(data);
        foreach (var item in bindedMethods)
        {
            if (item.Key == data.Header)
            {
                bindedMethods[item.Key].ForEach((handler) => handler?.Invoke(data));

                return;
            }
        }
    }

    protected async void SendNetworkMessage(BJRequestData data)
    {
        byte[] networkMessage = Encoding.UTF8.GetBytes(BJRequestStringDataBuilder.BuildJsonRequestString(data));
        Array.Resize(ref networkMessage, 512);

        await dataStream.WriteAsync(networkMessage);
    }

    protected async Task<BJRequestData> ReceiveNetworkMessage()
    {
        byte[] buffer = new byte[512];
        await dataStream.ReadAsync(buffer, 0, 512);

        var data = new BJRequestDataParser(Encoding.UTF8.GetString(buffer)).Parse();

        return data;
    }

    public void AddNetworkMessageListener(string triggerHeader, Action<BJRequestData> handler)
    {
        if (bindedMethods.ContainsKey(triggerHeader))
            bindedMethods[triggerHeader].Add(handler);
        else
            bindedMethods.Add(triggerHeader, new() { handler });
    }

    public virtual void Dispose()
    {
        tcpClient?.Dispose();
        dataStream?.Dispose();
    }

    protected byte[] FromObjectToByteArray(object data)
    {
        byte[] array = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        Array.Resize(ref array, 512);

        return array;
    }

    protected JProperty FromByteArrayToJProperty(byte[] data) =>
        JObject.Parse(Encoding.UTF8.GetString(data)).Properties().ToList()[0];
}