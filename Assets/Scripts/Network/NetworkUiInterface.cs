using UnityEngine;

public abstract class NetworkUiInterface : MonoBehaviour
{
    public abstract void EnableUi(object data);
    public abstract void DisableUi(object data);
}