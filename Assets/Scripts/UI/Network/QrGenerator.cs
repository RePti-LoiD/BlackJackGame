using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QrGenerator : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;

    private Texture2D texture;

    public void CreateQrAndShow(string message)
    {
        texture = new Texture2D(256, 256);
        Color32[] qrArray = QrCreate(message, 256, 256);

        texture.SetPixels32(qrArray);
        texture.Apply();

        rawImage.texture = texture;
    }

    private Color32[] QrCreate(string text, int w, int h)
    {
        BarcodeWriter writer = new BarcodeWriter()
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Width = w,
                Height = h,
                Margin = 2
            }
        };

        return writer.Write(text);
    }
}