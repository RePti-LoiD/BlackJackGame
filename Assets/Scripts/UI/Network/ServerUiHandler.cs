using UnityEngine;

public class ServerUiHandler : NetworkUiInterface
{
    [SerializeField] private QrGenerator generator;
    [SerializeField] private GameObject serverUiFrame;

    public override void DisableUi(object data)
    {
        serverUiFrame.SetActive(false);
    }

    public override void EnableUi(object data)
    {
        serverUiFrame.SetActive(true);

        generator.CreateQrAndShow(data as string ?? "Server internal error. Try againg.");
    }
}