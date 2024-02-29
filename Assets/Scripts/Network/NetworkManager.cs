using BJTcpRequestProtocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//TODO: �������� �������� �� ����� �������������� ������� ��������� ��� ��������� ������, ����� �� �������� switch � �����������
public abstract class NetworkManager : MonoBehaviour, IDisposable
{
    protected TcpClient tcpClient;
    protected NetworkStream dataStream;

    protected async virtual void Start()
    {
        await NetworkInitialization();
        dataStream = tcpClient.GetStream();
        ListenNetworkStream();

        PostNetworkInitialization();
    }

    protected virtual Task NetworkInitialization() {  return Task.CompletedTask; }
    protected virtual void PostNetworkInitialization() { }

    protected async void ListenNetworkStream()
    {
        while (true)
            HandleNetworkMessage(await ReceiveNetworkMessage());
    }

    protected virtual void HandleNetworkMessage(BJRequestData data)
    { }

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

    protected async void SendNetworkMessage(BJRequestData data)
    {
        byte[] networkMessage = Encoding.UTF8.GetBytes(BJRequestStringDataBuilder.BuildJsonRequestString(data));
        Array.Resize(ref networkMessage, 512);

        await dataStream.WriteAsync(networkMessage);
    }

    protected async Task<BJRequestData> ReceiveNetworkMessage()
    {
        byte[] buffer = new byte[512];
        await dataStream.ReadAsync(buffer);

        return new BJRequestJsonParser(Encoding.UTF8.GetString(buffer)).Parse();
    }
}