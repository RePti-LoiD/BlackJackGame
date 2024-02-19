using ZXing;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using ZXing.QrCode;

public class GeneratorQr : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;

    private Texture2D tex;

    private void Start()
    {
        tex = new Texture2D(256, 256);

        foreach (var item in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                ShowTexture(item.ToString());
                break;
            }
    }

    

    private void ShowTexture(string message)
    {
        var colors = GenerateQr(message);

        tex.SetPixels32(colors);
        tex.Apply();

        rawImage.texture = tex;
    }

    private Color32[] GenerateQr(string message)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = 256,
                Width = 256,
                Margin = 1
            }
        };

        return writer.Write(message);
    }
}