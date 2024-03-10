using System;
using System.Net;
using TMPro;
using UnityEngine;

public class ClientUiHandler : NetworkUiInterface
{
    [SerializeField] private TMP_Text exceptionMessage;
    [SerializeField] private QrCodeScanner qrCodeScanner;
    [SerializeField] private LobbyClient client;
    [SerializeField] private ActivatableObject activatableObject;


    public void Start()
    {
        activatableObject.OnDeactivate += () =>
        {
            client.Dispose();
        };

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
                exceptionMessage.text = ex.Message;
            }
        };
    }

    public override void DisableUi()
    { }

    public override void EnableUi()
    { }
}