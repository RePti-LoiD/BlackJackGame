using System;
using System.Net;
using TMPro;
using UnityEngine;

public class ClientUiHandler : NetworkUiInterface
{
    [SerializeField] private GameObject clientUiFrame;
    [SerializeField] private TMP_Text text;
    [SerializeField] private QrCodeScanner qrCodeScanner;
    [SerializeField] private LobbyClient client;

    public void Start()
    {
        qrCodeScanner.OnQrScan += (x) =>
        {
            try
            {
                string[] splittedIpEndpoint = x.Split(":");

                client.StartClient(
                    new IPEndPoint(
                        IPAddress.Parse(splittedIpEndpoint[0]),
                        int.Parse(splittedIpEndpoint[1])
                ));
            }
            catch (Exception ex)
            {
                text.text = ex.Message;
            }
        };
    }

    public override void DisableUi()
    {
        client.CloseClient();
        clientUiFrame.SetActive(false);
    }

    public override void EnableUi()
    {
        clientUiFrame.SetActive(true);
    }
}