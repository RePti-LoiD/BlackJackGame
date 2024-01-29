using UnityEngine;

public class ClientUiHandler : NetworkUiInterface
{
    [SerializeField] private GameObject clientUiFrame;

    public override void DisableUi(object data)
    {
        clientUiFrame.SetActive(false);
    }

    public override void EnableUi(object data)
    {
        clientUiFrame.SetActive(true);
    }
}