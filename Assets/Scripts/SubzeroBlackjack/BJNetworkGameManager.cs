using BJTcpRequestProtocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public abstract class BJNetworkGameManager : BJGameManager, IDisposable
{
    protected TcpClient tcpClient;
    protected NetworkStream dataStream;

    protected async virtual void Start()
    {
        await NetworkInitialization();

        dataStream = tcpClient.GetStream();
        ListenNetworkStream();
    }

    protected abstract Task NetworkInitialization();

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
        Array.Resize(ref array, 128);

        return array;
    }

    protected JProperty FromByteArrayToJProperty(byte[] data) => 
        JObject.Parse(Encoding.UTF8.GetString(data)).Properties().ToList()[0];

    protected async void SendNetworkMessage(BJRequestData data) =>
        await dataStream.WriteAsync(Encoding.UTF8.GetBytes(BJRequestStringDataBuilder.BuildJsonRequestString(data)));

    protected async Task<BJRequestData> ReceiveNetworkMessage()
    {
        byte[] buffer = new byte[128];
        await dataStream.ReadAsync(buffer);

        return new BJRequestJsonParser(Encoding.UTF8.GetString(buffer)).Parse();
    }
}