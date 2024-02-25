using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

public abstract class BJNetworkGameManager : BJGameManager, IDisposable
{
    protected TcpClient tcpClient;
    protected NetworkStream dataStream;

    protected virtual void HandleNetworkMessage(string message) { }

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
    protected JProperty FromByteArrayToJProperty(byte[] data) => JObject.Parse(Encoding.UTF8.GetString(data)).Properties().ToList()[0];
}