using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;
using Newtonsoft.Json;

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

        print(GamePrefs.SavePath + "CamData.json");
        File.WriteAllText(GamePrefs.SavePath + "CamData.json", JsonConvert.SerializeObject(devices, Formatting.Indented));

        if (devices.Length == 0)
        {
            isCamAvaible = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                //, (int)scanZone.rect.width, (int)scanZone.rect.height
                cameraTexture = new WebCamTexture(devices[i].name, 800, 800);
                print(devices[i]);
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

        float ratio = 1;
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