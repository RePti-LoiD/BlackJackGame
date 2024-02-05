using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;
using System;

public class QrCodeScanner : MonoBehaviour
{
    [SerializeField] private RawImage cameraSource;
    [SerializeField] private AspectRatioFitter aspectRation;
    [SerializeField] private TextMeshProUGUI outputText;
    [SerializeField] private RectTransform scanZone;

    public Action<string> OnQrScan;

    private bool isCamAvaible;
    private WebCamTexture cameraTexture;

    void Start()
    {
        SetUpCamera();
    }

    void Update()
    {
        UpdateCameraRender();
    }

    private void SetUpCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            isCamAvaible = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                cameraTexture = new WebCamTexture(devices[i].name, (int)scanZone.rect.width, (int)scanZone.rect.height);
                break;
            }
        }

        cameraTexture.Play();
        cameraSource.texture = cameraTexture;
        isCamAvaible = true;
    }

    private void UpdateCameraRender()
    {
        if (isCamAvaible == false) return;

        float ratio = cameraTexture.width / cameraTexture.height;
        aspectRation.aspectRatio = ratio;

        int orientation = cameraTexture.videoRotationAngle;
        orientation = orientation * 3;
        cameraSource.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }

    public void OnClickScan()
    {
        Scan();
    }

    private void Scan()
    {
        try
        {
            BarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(cameraTexture.GetPixels32(), cameraTexture.width, cameraTexture.height);
            if (result != null)
            {
                //outputText.text = result.Text;
                OnQrScan?.Invoke(result.Text);
            }
            else
            {
                outputText.text = "Failed to Read QR CODE";
            }
        }
        catch
        {
            outputText.text = "FAILED IN TRY";
        }
    }
}